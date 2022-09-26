using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public interface IToggleActive
{
    void ToggleActive(bool value);
}

public class GenericPool<T> where T : IToggleActive
{
    public List<T> ActiveItems { get; } = new();
    public int ActiveItemCount => ActiveItems.Count;

    private readonly ConcurrentBag<T> objects;
    private readonly Func<T> objectGenerator;

    public GenericPool(Func<T> objectGenerator)
    {
        if (objectGenerator == null)
            return;

        objects = new ConcurrentBag<T>();
        this.objectGenerator = objectGenerator;
    }

    public T Get()
    {
        if (!objects.TryTake(out var item))
            item = objectGenerator();

        item.ToggleActive(true);
        ActiveItems.Add(item);
        return item;
    }

    public void Release(T item)
    {
        if (!ActiveItems.Contains(item))
            return;

        item.ToggleActive(false);
        objects.Add(item);
        ActiveItems.Remove(item);
    }
}
