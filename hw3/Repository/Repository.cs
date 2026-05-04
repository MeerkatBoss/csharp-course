using System.Collections.Immutable;

namespace hw3.Repository;

public class Repository<T> where T : IEntity
{
    private readonly Dictionary<int, T> storage = [];

    public IEnumerable<T> All { get => storage.Values; }

    public int Count { get => storage.Count; }

    public void Add(T value)
    {
        if (!storage.TryAdd(value.Id, value))
        {
            throw new InvalidOperationException($"Cannot add duplicate key {value.Id}");
        }
    }

    public bool Remove(int id) => storage.Remove(id);

    public T? GetById(int id) => storage.GetValueOrDefault(id);

    public IEnumerable<T> Filter(Predicate<T> predicate)
    {
        foreach (T item in All)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    public List<T> Find(Predicate<T> predicate) => Filter(predicate).ToList();

    public List<T> GetAll() => All.ToList();

}
