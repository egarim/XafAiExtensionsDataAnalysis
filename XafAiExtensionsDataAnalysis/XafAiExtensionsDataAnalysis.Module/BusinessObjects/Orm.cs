using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XafAiExtensionsDataAnalysis.Module.BusinessObjects
{

    [NavigationItem("Business Objects")]
    [DefaultClassOptions]
    [Description("Represents a customer in the system.")]
    public class Customer : BaseObject
    {
        public Customer(Session session) : base(session) { }

        private string name;
        [Size(255)]
        [Description("The name of the customer.")]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        private string email;
        [Size(255)]
        [Description("The email address of the customer.")]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        private string phone;
        [Size(50)]
        [Description("The phone number of the customer.")]
        public string Phone
        {
            get => phone;
            set => SetPropertyValue(nameof(Phone), ref phone, value);
        }

        private Country country;
        [Association("Country-Customers")]
        [Description("The country of the customer.")]
        public Country Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }

        [Association("Customer-Invoices")]
        [Description("The collection of invoices associated with the customer.")]
        public XPCollection<InvoiceHeader> Invoices
        {
            get => GetCollection<InvoiceHeader>(nameof(Invoices));
        }
    }
    [NavigationItem("Business Objects")]
    [DefaultClassOptions]
    [Description("Represents a country in the system.")]
    public class Country : BaseObject
    {
        public Country(Session session) : base(session) { }

        private string name;
        [Size(255)]
        [Description("The name of the country.")]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        private string code;
        [Size(2)]
        [Description("The code of the country.")]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Association("Country-Customers")]
        [Description("The collection of customers associated with the country.")]
        public XPCollection<Customer> Customers
        {
            get => GetCollection<Customer>(nameof(Customers));
        }
    }
    [NavigationItem("Business Objects")]
    [DefaultClassOptions]
    [Description("Represents an invoice header in the system.")]
    public class InvoiceHeader : BaseObject
    {
        public InvoiceHeader(Session session) : base(session) { }

        private DateTime invoiceDate;
        [Description("The date of the invoice.")]
        public DateTime InvoiceDate
        {
            get => invoiceDate;
            set => SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value);
        }

        private Customer customer;
        [Association("Customer-Invoices")]
        [Description("The customer associated with the invoice.")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        private decimal totalAmount;
        [Description("The total amount of the invoice.")]
        public decimal TotalAmount
        {
            get => totalAmount;
            set => SetPropertyValue(nameof(TotalAmount), ref totalAmount, value);
        }

        [Association("Invoice-Details")]
        [Description("The collection of invoice details associated with the invoice.")]
        public XPCollection<InvoiceDetail> Details
        {
            get => GetCollection<InvoiceDetail>(nameof(Details));
        }
    }

    [NavigationItem("Business Objects")]
    [Description("Represents an invoice detail in the system.")]
    public class InvoiceDetail : BaseObject
    {
        public InvoiceDetail(Session session) : base(session) { }

        private InvoiceHeader invoice;
        [Association("Invoice-Details")]
        [Description("The invoice associated with the detail.")]
        public InvoiceHeader Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        private Product product;
        [Association("Product-InvoiceDetails")]
        [Description("The product associated with the detail.")]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }

        private int quantity;
        [Description("The quantity of the product.")]
        public int Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }

        private decimal unitPrice;
        [Description("The unit price of the product.")]
        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }

        private decimal lineTotal;
        [Description("The total line amount for the detail.")]
        public decimal LineTotal
        {
            get => lineTotal;
            set => SetPropertyValue(nameof(LineTotal), ref lineTotal, value);
        }
    }
    [NavigationItem("Business Objects")]
    [DefaultClassOptions]
    [Description("Represents a product in the system.")]
    public class Product : BaseObject
    {
        public Product(Session session) : base(session) { }

        private string name;
        [Size(255)]
        [Description("The name of the product.")]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        private decimal unitPrice;
        [Description("The unit price of the product.")]
        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }

        private ProductCategory category;
        [Association("Category-Products")]
        [Description("The category of the product.")]
        public ProductCategory Category
        {
            get => category;
            set => SetPropertyValue(nameof(Category), ref category, value);
        }

        [Association("Product-InvoiceDetails")]
        [Description("The collection of invoice details associated with the product.")]
        public XPCollection<InvoiceDetail> InvoiceDetails
        {
            get => GetCollection<InvoiceDetail>(nameof(InvoiceDetails));
        }
    }
    [NavigationItem("Business Objects")]
    [DefaultClassOptions]
    [Description("Represents a product category in the system.")]
    public class ProductCategory : BaseObject
    {
        public ProductCategory(Session session) : base(session) { }

        private string name;
        [Size(255)]
        [Description("The name of the product category.")]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Association("Category-Products")]
        [Description("The collection of products associated with the category.")]
        public XPCollection<Product> Products
        {
            get => GetCollection<Product>(nameof(Products));
        }
    }
}
