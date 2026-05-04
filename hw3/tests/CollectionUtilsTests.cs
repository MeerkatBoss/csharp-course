namespace hw3.tests;

using hw3.Collections;
using NUnit.Framework;

class MockFuzzyEquality(int value, int marker)
{
    public int Value { get; } = value;
    public int Marker { get; } = marker;

    MockFuzzyEquality() : this(-1, -1)
    {

    }

    public override string ToString() => $"MockFuzzyEquality{{Value={Value}, Marker={Marker}}}";

    public override bool Equals(object? obj) => obj switch
    {
        MockFuzzyEquality fuzzy => Value.Equals(fuzzy.Value), // Ignore marker
        _ => false
    };

    public override int GetHashCode() => Value.GetHashCode(); // Ignore marker
}

[TestFixture]
class CollectionUtilsTests
{
    private static T AssertNotCalled<U, T>(U _u)
    {
        Assert.Fail("This method should not be called");
        throw new Exception(); // Unreachable
    }

    private static T AssertNotCalled<U, V, T>(U _u, V _v)
    {
        Assert.Fail("This method should not be called");
        throw new Exception(); // Unreachable
    }

    [Test]
    public void Distinct_EmptyList_ReturnEmpty()
    {
        List<int> items = [];
        List<int> distinct = CollectionUtils.Distinct(items);
        Assert.IsEmpty(distinct);
    }

    [Test]
    public void Distinct_AlreadyDistinct_ReturnUnchanged()
    {
        List<int> items = [1, 2, 3, 4, 5];
        List<int> distinct = CollectionUtils.Distinct(items);
        Assert.AreEqual(items, distinct);
        Assert.AreNotSame(items, distinct);
    }

    [Test]
    public void Distinct_HasDuplicates_ReturnDistinct()
    {
        List<int> items = [1, 2, 3, 1, 3, 3, 4, 2, 2, 5, 4, 5, 5];
        List<int> expected = [1, 2, 3, 4, 5];
        List<int> distinct = CollectionUtils.Distinct(items);
        Assert.AreEqual(expected, distinct);
    }

    [Test]
    public void Distinct_FuzzyEquals_PreserveFirst()
    {
        List<MockFuzzyEquality> items = [
            new(1, 1),
            new(1, 2),
            new(1, 3)
        ];

        List<MockFuzzyEquality> distinct = CollectionUtils.Distinct(items);
        Assert.AreEqual(1, distinct.Count);
        Assert.AreEqual(1, distinct.First()?.Marker);
    }

    [Test]
    public void GroupBy_EmptyList_ReturnEmpty()
    {
        var groups = CollectionUtils.GroupBy<string, int>([], AssertNotCalled<string, int>);
        Assert.IsEmpty(groups);
    }

    [Test]
    public void GroupBy_SingleGroup_ReturnOneList()
    {
        List<string> list = ["a", "b", "c"];
        var groups = CollectionUtils.GroupBy(list, _ => 0);

        Assert.AreEqual(1, groups.Count);
        Assert.That(groups, Does.ContainKey(0));
        var singleGroup = groups[0];
        Assert.AreEqual(list, singleGroup);
        Assert.AreNotSame(list, singleGroup);
    }

    [Test]
    public void GroupBy_MultipleGroups_ReturnMultipleLists()
    {
        List<string> list = [
            "abcd",
            "bcda",
            "cdab",
            "dabc",
            "aaaa",
            "bbbb",
            "cccc",
            "dddd"
        ];

        Dictionary<char, List<string>> expected = new()
        {
            {'a', ["abcd", "aaaa"]},
            {'b', ["bcda", "bbbb"]},
            {'c', ["cdab", "cccc"]},
            {'d', ["dabc", "dddd"]}
        };

        var groups = CollectionUtils.GroupBy(list, str => str[0]);
        Assert.AreEqual(expected, groups);
    }

    [Test]
    public void Merge_TwoEmptyDictionaries_ReturnEmpty()
    {
        Dictionary<int, string> empty = [];
        var merged = CollectionUtils.Merge(empty, empty, AssertNotCalled<string, string, string>);
        Assert.IsEmpty(merged);
    }

    [Test]
    public void Merge_OneEmptyDictionary_ReturnNonEmpty()
    {
        Dictionary<int, string> empty = [];
        Dictionary<int, string> nonEmpty = new()
        {
            {1, "1"},
            {2, "2"},
            {3, "3"}
        };


        var mergedRight = CollectionUtils.Merge(empty, nonEmpty, AssertNotCalled<string, string, string>);
        Assert.AreEqual(nonEmpty, mergedRight);
        Assert.AreNotSame(nonEmpty, mergedRight);

        var mergedLeft = CollectionUtils.Merge(nonEmpty, empty, AssertNotCalled<string, string, string>);
        Assert.AreEqual(nonEmpty, mergedRight);
        Assert.AreNotSame(nonEmpty, mergedRight);
    }

    [Test]
    public void Merge_NoConflicts_ReturnMerged()
    {
        Dictionary<int, string> first = new()
        {
            {1, "1"},
            {2, "2"},
            {3, "3"}
        };

        Dictionary<int, string> second = new()
        {
            {4, "4"},
            {5, "5"},
            {6, "6"}
        };

        var merged = CollectionUtils.Merge(first, second, AssertNotCalled<string, string, string>);

        Assert.AreEqual(6, merged.Count);

        foreach (var item in first)
        {
            Assert.That(merged, Does.Contain(item));
        }
        foreach (var item in second)
        {
            Assert.That(merged, Does.Contain(item));
        }
    }

    [Test]
    public void Merge_HasConflicts_ResolveConflicts()
    {
        Dictionary<int, string> first = new()
        {
            {1, "1"},
            {2, "2"},
            {3, "3"}
        };
        Dictionary<int, string> second = new()
        {
            {1, "a"},
            {3, "c"},
            {4, "d"}
        };
        Dictionary<int, string> expected = new()
        {
            {1, "1a"},
            {2, "2"},
            {3, "3c"},
            {4, "d"}
        };

        var merged = CollectionUtils.Merge(first, second, string.Concat);
        Assert.That(merged, Is.EquivalentTo(expected));
    }

    [Test]
    public void MaxBy_EmptyList_ThrowException()
    {
        List<string> empty = [];
        Assert.Throws<InvalidOperationException>(() => CollectionUtils.MaxBy(empty, AssertNotCalled<string, int>));
    }

    [Test]
    public void MaxBy_SingleElement_ReturnFirst()
    {
        List<string> single = ["abc"];
        var max = CollectionUtils.MaxBy(single, _ => 0);
        Assert.AreEqual(single.First(), max);
    }

    [Test]
    public void MaxBy_SameKey_ReturnFirst()
    {
        List<string> list = ["abc", "def", "ghi"];
        var max = CollectionUtils.MaxBy(list, _ => 0);
        Assert.AreEqual(list.First(), max);
    }

    [Test]
    public void MaxBy_DifferentKeys_ReturnMax()
    {
        List<string> list = ["abc", "defg", "ij", "klmnop", "qrst"];
        var max = CollectionUtils.MaxBy(list, str => str.Length);
        Assert.AreEqual("klmnop", max);
    }
}
