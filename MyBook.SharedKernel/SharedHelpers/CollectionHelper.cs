using MyBook.Entities;

namespace MyBook.SharedKernel.Extensions;

public static class CollectionHelper<T>
{
    public static bool Contains(ICollection<T> collection, Func<T, bool> predicate)
    {
        return collection.Where(predicate).Any();
    }
}