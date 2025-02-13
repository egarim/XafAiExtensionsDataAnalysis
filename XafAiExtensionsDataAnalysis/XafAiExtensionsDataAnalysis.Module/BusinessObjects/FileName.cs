using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace YourNamespace.Module.BusinessObjects
{
    public class DataGenerator
    {
        private readonly Random _random = new Random(42);
        private readonly IObjectSpace _objectSpace;

        public DataGenerator(IObjectSpace objectSpace)
        {
            _objectSpace = objectSpace;
        }

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

    // Extension method to help with data initialization
    public static class DataGeneratorExtensions
    {
        public static void GenerateDataIfEmpty(this IObjectSpace objectSpace)
        {
            if (!objectSpace.GetObjects<Customer>().Any())
            {
                var generator = new DataGenerator(objectSpace);
                generator.GenerateData();
            }
        }
    }
    [DefaultClassOptions]
    public class Customer : BaseObject
    {
        public Customer(Session session) : base(session) { }

        private string name;
        [Size(255)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        private string email;
        [Size(255)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        private string phone;
        [Size(50)]
        public string Phone
        {
            get => phone;
            set => SetPropertyValue(nameof(Phone), ref phone, value);
        }

        private Country country;
        [Association("Country-Customers")]
        public Country Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }

        [Association("Customer-Invoices")]
        public XPCollection<InvoiceHeader> Invoices
        {
            get => GetCollection<InvoiceHeader>(nameof(Invoices));
        }
    }

    [DefaultClassOptions]
    public class Country : BaseObject
    {
        public Country(Session session) : base(session) { }

        private string name;
        [Size(255)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        private string code;
        [Size(2)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Association("Country-Customers")]
        public XPCollection<Customer> Customers
        {
            get => GetCollection<Customer>(nameof(Customers));
        }
    }

    [DefaultClassOptions]
    public class InvoiceHeader : BaseObject
    {
        public InvoiceHeader(Session session) : base(session) { }

        private DateTime invoiceDate;
        public DateTime InvoiceDate
        {
            get => invoiceDate;
            set => SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value);
        }

        private Customer customer;
        [Association("Customer-Invoices")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        private decimal totalAmount;
        public decimal TotalAmount
        {
            get => totalAmount;
            set => SetPropertyValue(nameof(TotalAmount), ref totalAmount, value);
        }

        [Association("Invoice-Details")]
        public XPCollection<InvoiceDetail> Details
        {
            get => GetCollection<InvoiceDetail>(nameof(Details));
        }
    }

    [DefaultClassOptions]
    public class InvoiceDetail : BaseObject
    {
        public InvoiceDetail(Session session) : base(session) { }

        private InvoiceHeader invoice;
        [Association("Invoice-Details")]
        public InvoiceHeader Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        private Product product;
        [Association("Product-InvoiceDetails")]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }

        private decimal unitPrice;
        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }

        private decimal lineTotal;
        public decimal LineTotal
        {
            get => lineTotal;
            set => SetPropertyValue(nameof(LineTotal), ref lineTotal, value);
        }
    }

    [DefaultClassOptions]
    public class Product : BaseObject
    {
        public Product(Session session) : base(session) { }

        private string name;
        [Size(255)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        private decimal unitPrice;
        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }

        private ProductCategory category;
        [Association("Category-Products")]
        public ProductCategory Category
        {
            get => category;
            set => SetPropertyValue(nameof(Category), ref category, value);
        }

        [Association("Product-InvoiceDetails")]
        public XPCollection<InvoiceDetail> InvoiceDetails
        {
            get => GetCollection<InvoiceDetail>(nameof(InvoiceDetails));
        }
    }

    [DefaultClassOptions]
    public class ProductCategory : BaseObject
    {
        public ProductCategory(Session session) : base(session) { }

        private string name;
        [Size(255)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Association("Category-Products")]
        public XPCollection<Product> Products
        {
            get => GetCollection<Product>(nameof(Products));
        }
    }
}