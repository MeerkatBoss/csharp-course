namespace hw3;

using hw3.Repository;

class User(int id, string name) : IEntity
{
    public int Id { get; } = id;
    
    public string Name { get; } = name;
}
