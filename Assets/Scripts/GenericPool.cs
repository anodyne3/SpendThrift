using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public interface IToggleActive
{
    void ToggleActive(bool value);
}

public class GenericPool<T> where T : IToggleActive
{
    public List<T> activeItems { get; } = new List<T>();
    public int activeItemCount => activeItems.Count;

    private readonly ConcurrentBag<T> _objects;
    private readonly Func<T> _objectGenerator;

    public GenericPool(Func<T> objectGenerator)
    {
        if (objectGenerator == null)
            return;

        _objects = new ConcurrentBag<T>();
        _objectGenerator = objectGenerator;
    }

    public T Get()
    {
        if (!_objects.TryTake(out var item))
        {
            item = _objectGenerator();
        }

        item.ToggleActive(true);
        activeItems.Add(item);
        return item;
    }

    public void Release(T item)
    {
        if (!activeItems.Contains(item))
            return;

        item.ToggleActive(false);
        _objects.Add(item);
        activeItems.Remove(item);
    }
}
