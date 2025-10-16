using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Address
{
    private string _streetAddress;
    private string _city;
    private string _stateProvince;
    private string _country;

    public string StreetAddress
    {
        get { return _streetAddress; }
        set { _streetAddress = value; }
    }

    public string City
    {
        get { return _city; }
        set { _city = value; }
    }

    public string StateProvince
    {
        get { return _stateProvince; }
        set { _stateProvince = value; }
    }

    public string Country
    {
        get { return _country; }
        set { _country = value; }
    }

    public Address(string streetAddress, string city, string stateProvince, string country)
    {
        _streetAddress = streetAddress;
        _city = city;
        _stateProvince = stateProvince;
        _country = country;
    }

    public bool IsInUSA()
    {
        return _country.Equals("USA");
    }

    public string GetFullAddressString()
    {
        return $"{_streetAddress}\n{_city}, {_stateProvince}\n{_country}";
    }
}

public class Customer
{
    private string _name;
    private Address _address;

    // Properties
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Address Address
    {
        get { return _address; }
        set { _address = value; }
    }

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public bool IsInUSA()
    {
        return _address.IsInUSA();
    }
}

// --- Product Class ---
public class Product
{
    private string _name;
    private string _productId;
    private double _price;
    private int _quantity;

    // Properties
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string ProductId
    {
        get { return _productId; }
        set { _productId = value; }
    }

    public double Price
    {
        get { return _price; }
        set { _price = value; }
    }

    public int Quantity
    {
        get { return _quantity; }
        set { _quantity = value; }
    }

    // Constructor
    public Product(string name, string productId, double price, int quantity)
    {
        _name = name;
        _productId = productId;
        _price = price;
        _quantity = quantity;
    }

    public double GetTotalCost()
    {
        return _price * _quantity;
    }
}

public class Order
{
    private List<Product> _products;
    private Customer _customer;

    private const double US_SHIPPING_COST = 5.00;
    private const double INTERNATIONAL_SHIPPING_COST = 35.00;

    public List<Product> Products
    {
        get { return _products; }
    }

    public Customer Customer
    {
        get { return _customer; }
        set { _customer = value; }
    }

    public Order(Customer customer)
    {
        _customer = customer;
        _products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public double CalculateTotalCost()
    {
        double productsTotal = 0;
        foreach (Product product in _products)
        {
            productsTotal += product.GetTotalCost();
        }

        double shippingCost = GetShippingCost();

        return productsTotal + shippingCost;
    }

    private double GetShippingCost()
    {
        return _customer.IsInUSA() ? US_SHIPPING_COST : INTERNATIONAL_SHIPPING_COST;
    }

    public string GetPackingLabel()
    {
        StringBuilder label = new StringBuilder();
        label.AppendLine("--- Packing Label ---");
        label.AppendLine($"Customer: {_customer.Name}\n");

        foreach (Product product in _products)
        {
            label.AppendLine($"Name: {product.Name} (x{product.Quantity}) | Product ID: {product.ProductId}");
        }

        return label.ToString();
    }

    public string GetShippingLabel()
    {
        StringBuilder label = new StringBuilder();
        label.AppendLine("--- Shipping Label ---");
        label.AppendLine($"Customer Name: {_customer.Name}");
        label.AppendLine("Destination Address:");
        label.AppendLine(_customer.Address.GetFullAddressString());

        return label.ToString();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Order Program\n");

        Address address1 = new Address("123 Main St", "Eagle Mountain", "UT", "USA");
        Customer customer1 = new Customer("Tyler Kraft", address1);

        // Products for Order 1
        Product productA = new Product("Laptop", "LTP-001", 850.50, 1);
        Product productB = new Product("Mouse", "ACC-010", 25.99, 2);

        // Create Order 1
        Order order1 = new Order(customer1);
        order1.AddProduct(productA);
        order1.AddProduct(productB);

        Console.WriteLine("ORDER 1: Tyler Kraft (USA)");
        Console.WriteLine("====================================================\n");

        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());

        double totalCost1 = order1.CalculateTotalCost();
        Console.WriteLine($"Total Order Cost: ${totalCost1:0.00}\n");

        Address address2 = new Address("5 Random", "Paris", "Some-France", "France");
        Customer customer2 = new Customer("Bob Levesque", address2);

        Product productC = new Product("Headphones", "AUD-999", 199.99, 1);
        Product productD = new Product("T-Shirt", "CLO-505", 15.00, 5);
        Product productE = new Product("Book", "EDU-202", 9.50, 1);

        Order order2 = new Order(customer2);
        order2.AddProduct(productC);
        order2.AddProduct(productD);
        order2.AddProduct(productE);

        Console.WriteLine("ORDER 2: Bob Levesque (International)");
        Console.WriteLine("====================================================\n");

        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());

        double totalCost2 = order2.CalculateTotalCost();
        Console.WriteLine($"Total Order Cost: ${totalCost2:0.00}\n");
    }
}