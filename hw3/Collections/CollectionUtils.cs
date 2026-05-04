namespace hw3.Collections;

public static class CollectionUtils
{
    public static List<T> Distinct<T>(List<T> source) => source.Distinct().ToList();

    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source)
    {
        HashSet<T> values = [];

        foreach (T item in source)
        {
            bool isNewItem = values.Add(item);
            if (isNewItem)
            {
                yield return item;
            }
        }
    }

    public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
      this IEnumerable<TValue> source,
      Func<TValue, TKey> keySelector
    ) where TKey : notnull
    {
        Dictionary<TKey, List<TValue>> result = [];

        foreach (TValue item in source)
        {
            TKey key = keySelector(item);
            result.TryAdd(key, []);

            result[key].Add(item);
        }

        return result;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
      IReadOnlyDictionary<TKey, TValue> first,
      IReadOnlyDictionary<TKey, TValue> second,
      Func<TValue, TValue, TValue> conflictResolver
    ) where TKey : notnull
    {
        Dictionary<TKey, TValue> result = new(first);

        foreach (var (key, value) in second)
        {
            if (result.TryGetValue(key, out var duplicate))
            {
                result[key] = conflictResolver(value, duplicate);
            }
            else
            {
                result.Add(key, value);
            }
        }

        return result;
    }

    public static T MaxBy<T, TKey>(
      this IEnumerable<T> source,
      Func<T, TKey> selector
    ) where TKey : IComparable<TKey>
    {
        var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException("No maximum in empty list");
        }
        T maxItem = enumerator.Current;
        TKey maxKey = selector(maxItem);

        while (enumerator.MoveNext())
        {
            T newItem = enumerator.Current;
            TKey newKey = selector(newItem);
            if (newKey.CompareTo(maxKey) > 0)
            {
                maxItem = newItem;
                maxKey = newKey;
            }
        }

        return maxItem;
    }
}
