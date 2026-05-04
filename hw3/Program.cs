using hw3;
using hw3.Repository;

Repository<User> users = new();
Repository<Product> products = new();

users.Add(new(1, "John Doe"));
users.Add(new(9999, "K00lHazker"));

products.Add(new(1, "Product1", 700));
products.Add(new(2, "Product2", 1300));
products.Add(new(3, "Product3", 1200));
products.Add(new(4, "Product4", 800));
products.Add(new(5, "Product5", 5000));

try
{
    products.Add(new(2, "ProductX", 9999));
}
catch (InvalidOperationException e)
{
    Console.WriteLine($"Failed to add product: {e.Message}");
}

Console.WriteLine($"Product with id 4 is {products.GetById(4)?.Name ?? "UNKNOWN"}");
Console.WriteLine($"Product with id 10 is {products.GetById(10)?.Name ?? "UNKNOWN"}");

Console.WriteLine("Here are all products:");
foreach (var product in products.All)
{
    Console.WriteLine($"  - id: {product.Id}, name: '{product.Name}', price: {product.Price}");
}

Console.WriteLine("Here are all products which cost more than 1000:");
foreach (var product in products.Filter(p => p.Price > 1000))
{
    Console.WriteLine($"  - id: {product.Id}, name: '{product.Name}', price: {product.Price}");
}

Console.WriteLine("Here are all users:");
foreach (var user in users.All)
{
    Console.WriteLine($"  - id: {user.Id}, name: '{user.Name}'");
}
