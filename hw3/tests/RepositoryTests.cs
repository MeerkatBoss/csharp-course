namespace hw3.tests;

using hw3.Repository;
using NUnit.Framework;

class MockItem(int id, int value) : IEntity
{
    public MockItem() : this(-1, -1)
    {

    }

    public int Id { get; } = id;
    public int Value { get; } = value;

    public override bool Equals(object? obj) => obj switch
    {
        MockItem item => Id.Equals(item.Id) && Value.Equals(item.Value),
        _ => false
    };

    public override int GetHashCode() => 23 * Id.GetHashCode() + 17 * Value.GetHashCode();

    public override string ToString() => $"MockItem{{Id={Id}, Value={Value}}}";
}

[TestFixture]
public class RepositoryTests
{
    private Repository<MockItem> _repository = new();

    [SetUp]
    public void SetUp()
    {
        _repository = new();
    }

    [Test]
    public void GetById_EmptyRepository_ReturnNull()
    {
        MockItem? value = _repository.GetById(0);
        Assert.IsNull(value);
    }

    [Test]
    public void GetById_AddThenGet_ReturnAdded()
    {
        _repository.Add(new(0, 42));

        MockItem? item = _repository.GetById(0);
        Assert.IsNotNull(item);
        Assert.AreEqual(item?.Value, 42);
    }

    [Test]
    public void GetById_AddThenGetDifferent_ReturnNull()
    {
        _repository.Add(new(0, 42));

        MockItem? item = _repository.GetById(1);
        Assert.Null(item);
    }

    [Test]
    public void GetById_AddRemoveThenGet_ReturnNull()
    {
        _repository.Add(new(0, 42));
        _repository.Remove(0);

        MockItem? item = _repository.GetById(0);
        Assert.IsNull(item);
    }

    [Test]
    public void Add_Duplicate_ThrowException()
    {
        _repository.Add(new(0, 42));

        Assert.Throws<InvalidOperationException>(() => _repository.Add(new(0, 67)));
    }

    [Test]
    public void Remove_EmptyRepository_ReturnFalse()
    {
        bool removed = _repository.Remove(0);
        Assert.IsFalse(removed);
    }

    [Test]
    public void Remove_AddThenRemove_ReturnTrue()
    {
        _repository.Add(new(0, 42));

        bool removed = _repository.Remove(0);
        Assert.IsTrue(removed);
    }

    [Test]
    public void Count_EmptyRepository_ReturnZero()
    {
        Assert.AreEqual(_repository.Count, 0);
    }

    [Test]
    public void Count_AddThenCount_IncrementCount()
    {
        for (int n = 0; n < 10; ++n)
        {
            _repository.Add(new(n, 40 + n));
            Assert.AreEqual(_repository.Count, n + 1);
        }
    }

    [Test]
    public void Count_RemoveThenCount_DecrementCount()
    {
        for (int n = 0; n < 10; ++n)
        {
            _repository.Add(new(n, 40 + n));
        }

        for (int n = 9; n >= 0; --n)
        {
            _repository.Remove(n);
            Assert.AreEqual(_repository.Count, n);
        }
    }

    [Test]
    public void GetAll_EmptyRepository_ReturnsEmpty()
    {
        var items = _repository.GetAll();
        Assert.IsEmpty(items);
    }

    [Test]
    public void All_AddItems_ReturnsAllAdded()
    {
        for (int n = 0; n < 10; ++n)
        {
            _repository.Add(new(n, 40 + n));
        }

        var items = _repository.GetAll();

        Assert.AreEqual(items.Count, 10);

        for (int n = 0; n < 10; ++n)
        {
            Assert.Contains(new MockItem(n, 40 + n), items);
        }

    }

    [Test]
    public void Find_EmptyRepository_ReturnsEmpty()
    {
        var items = _repository.Find(_ => true);
        Assert.IsEmpty(items);
    }

    [Test]
    public void Find_AddItems_ReturnsFiltered()
    {
        for (int n = 0; n < 10; ++n)
        {
            _repository.Add(new(n, 40 + n));
        }

        Predicate<MockItem> predicate = item => item.Value % 2 == 0;

        var items = _repository.Find(predicate);

        Assert.AreEqual(items.Count, 5);

        for (int n = 0; n < 10; ++n)
        {
            MockItem item = new(n, 40 + n);

            if (predicate(item))
            {
                Assert.That(items, Does.Contain(item));
            }
            else
            {
                Assert.That(items, Does.Not.Contain(item));
            }
        }
    }
}
