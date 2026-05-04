namespace hw3;

using hw3.Repository;

class Product(int id, string name, int price) : IEntity
{
    public int Id { get; } = id;
    
    public string Name { get; } = name;

    public int Price { get; } = price;
}
