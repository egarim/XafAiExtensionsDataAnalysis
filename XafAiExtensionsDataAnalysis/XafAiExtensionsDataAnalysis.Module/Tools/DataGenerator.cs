using DevExpress.ExpressApp;
using System.Linq;
using XafAiExtensionsDataAnalysis.Module.BusinessObjects;

namespace XafAiExtensionsDataAnalysis.Module.Tools
{
    /// <summary>
    /// Generates sample data for the application.
    /// </summary>
    public class DataGenerator
    {
        private readonly Random _random = new Random(42);
        private readonly IObjectSpace _objectSpace;

        public DataGenerator(IObjectSpace objectSpace)
        {
            _objectSpace = objectSpace;
        }

        /// <summary>
        /// Generates sample data for countries, categories, products, customers, and invoices.
        /// </summary>
        public void GenerateData()
        {
            GenerateCountries();
            _objectSpace.CommitChanges();
            GenerateCategories();
            _objectSpace.CommitChanges();
            GenerateProducts();
            _objectSpace.CommitChanges();
            GenerateCustomers();
            _objectSpace.CommitChanges();
            GenerateInvoices();

            _objectSpace.CommitChanges();
        }

        private void GenerateCountries()
        {
            var countries = new[]
            {
                CreateCountry("United States", "US"),
                CreateCountry("Canada", "CA"),
                CreateCountry("United Kingdom", "UK"),
                CreateCountry("Germany", "DE"),
                CreateCountry("France", "FR")
            };
        }

        private Country CreateCountry(string name, string code)
        {
            var country = _objectSpace.CreateObject<Country>();
            country.Name = name;
            country.Code = code;
            return country;
        }

        private void GenerateCategories()
        {
            var categories = new[]
            {
                CreateCategory("Electronics"),
                CreateCategory("Office Supplies"),
                CreateCategory("Furniture")
            };
        }

        private ProductCategory CreateCategory(string name)
        {
            var category = _objectSpace.CreateObject<ProductCategory>();
            category.Name = name;
            return category;
        }

        private void GenerateProducts()
        {
            var categories = _objectSpace.GetObjects<ProductCategory>().ToList();

            var products = new[]
            {
                CreateProduct("Laptop", 999.99m, categories[0]),
                CreateProduct("Smartphone", 699.99m, categories[0]),
                CreateProduct("Desk Chair", 199.99m, categories[2]),
                CreateProduct("Paper Pack", 9.99m, categories[1]),
                CreateProduct("Desk", 299.99m, categories[2])
            };
        }

        private Product CreateProduct(string name, decimal unitPrice, ProductCategory category)
        {
            var product = _objectSpace.CreateObject<Product>();
            product.Name = name;
            product.UnitPrice = unitPrice;
            product.Category = category;
            return product;
        }

        private void GenerateCustomers()
        {
            var countries = _objectSpace.GetObjects<Country>().ToList();

            var customers = new[]
            {
                CreateCustomer("Acme Corp", "contact@acme.com", "+1-555-0101", countries[0]),
                CreateCustomer("Global Tech", "info@globaltech.com", "+1-555-0102", countries[1]),
                CreateCustomer("Euro Solutions", "sales@eurosolutions.com", "+49-555-0103", countries[3]),
                CreateCustomer("UK Traders", "info@uktraders.co.uk", "+44-555-0104", countries[2]),
                CreateCustomer("French Dynamics", "contact@frenchdyn.fr", "+33-555-0105", countries[4]),
                CreateCustomer("US Systems", "info@ussystems.com", "+1-555-0106", countries[0]),
                CreateCustomer("Canadian Tech", "sales@cantech.ca", "+1-555-0107", countries[1]),
                CreateCustomer("German Solutions", "info@germansol.de", "+49-555-0108", countries[3]),
                CreateCustomer("British Services", "contact@britishserv.uk", "+44-555-0109", countries[2]),
                CreateCustomer("Paris Systems", "info@parissys.fr", "+33-555-0110", countries[4])
            };
        }

        private Customer CreateCustomer(string name, string email, string phone, Country country)
        {
            var customer = _objectSpace.CreateObject<Customer>();
            customer.Name = name;
            customer.Email = email;
            customer.Phone = phone;
            customer.Country = country;
            return customer;
        }

        private void GenerateInvoices()
        {
            var customers = _objectSpace.GetObjects<Customer>().ToList();
            var products = _objectSpace.GetObjects<Product>().ToList();

            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime endDate = new DateTime(2025, 12, 31);

            foreach (var customer in customers)
            {
                int invoiceCount = _random.Next(20, 51);

                for (int i = 0; i < invoiceCount; i++)
                {
                    TimeSpan timeSpan = endDate - startDate;
                    TimeSpan randomSpan = new TimeSpan((long)(_random.NextDouble() * timeSpan.Ticks));
                    DateTime invoiceDate = startDate + randomSpan;

                    var invoice = CreateInvoiceHeader(customer, invoiceDate);
                    int itemCount = _random.Next(1, 6);

                    for (int j = 0; j < itemCount; j++)
                    {
                        var product = products[_random.Next(products.Count)];
                        int quantity = _random.Next(1, 11);
                        CreateInvoiceDetail(invoice, product, quantity);
                    }

                    // Update total amount
                    invoice.TotalAmount = invoice.Details.Sum(d => d.LineTotal);
                }
            }
        }

        private InvoiceHeader CreateInvoiceHeader(Customer customer, DateTime invoiceDate)
        {
            var invoice = _objectSpace.CreateObject<InvoiceHeader>();
            invoice.Customer = customer;
            invoice.InvoiceDate = invoiceDate;
            return invoice;
        }

        private InvoiceDetail CreateInvoiceDetail(InvoiceHeader invoice, Product product, int quantity)
        {
            var detail = _objectSpace.CreateObject<InvoiceDetail>();
            detail.Invoice = invoice;
            detail.Product = product;
            detail.Quantity = quantity;
            detail.UnitPrice = product.UnitPrice;
            detail.LineTotal = product.UnitPrice * quantity;
            return detail;
        }
    }
}
