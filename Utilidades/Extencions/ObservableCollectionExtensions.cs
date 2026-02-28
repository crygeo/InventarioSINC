using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Utilidades.Interfaces;

namespace Utilidades.Extencions;

public static class ObservableCollectionExtensions
{
    public static void ReplaceWith<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems)
    {
        collection.Clear();
        foreach (var item in newItems)
            collection.Add(item);
    }

    public static void ReplaceWith<TKey, TValue>(this Dictionary<TKey, TValue> collection,
        IDictionary<TKey, TValue> newItems)
    {
        collection.Clear();
        foreach (var item in newItems)
            collection.Add(item.Key, item.Value);
    }

    public static void SelectAll<T>(this ObservableCollection<T> collection) where T : ISelectable
    {
        foreach (var item in collection) item.IsSelect = true;
    }

    public static void DeselectAll<T>(this ObservableCollection<T> collection) where T : ISelectable
    {
        foreach (var item in collection) item.IsSelect = false;
    }

    public static void ToggleSelection<T>(this ObservableCollection<T> collection) where T : ISelectable
    {
        foreach (var item in collection) item.IsSelect = !item.IsSelect;
    }

    public static IEnumerable<T> GetSelected<T>(this ObservableCollection<T> collection) where T : ISelectable
    {
        return collection.Where(i => i.IsSelect);
    }

    public static void DeselectAll<T>(this IEnumerable<T> collection) where T : ISelectable
    {
        foreach (var item in collection) item.IsSelect = false;
    }

    public static void AddRangeDistinctBy<T, TKey>(this ObservableCollection<T> collection, IEnumerable<T>? itemsToAdd,
        Func<T, TKey> keySelector)
    {
        if (itemsToAdd == null) return;

        var existingKeys = new HashSet<TKey>(collection.Select(keySelector));

        foreach (var item in itemsToAdd)
        {
            var key = keySelector(item);
            if (existingKeys.Add(key)) collection.Add(item);
        }
    }

    public static void AddRange<T, TKey>(this ObservableDictionary<TKey, T> collection,
        ObservableDictionary<TKey, T>? itemsToAdd)
        where TKey : notnull
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (itemsToAdd == null || itemsToAdd.Count == 0)
            return;

        foreach (var kvp in itemsToAdd)
            // Si quieres sobreescribir si ya existe:
            if (collection.ContainsKey(kvp.Key))
                collection[kvp.Key] = kvp.Value;
            else
                collection.Add(kvp.Key, kvp.Value);
    }

    public static void RemoveRange<T, TKey>(this ObservableDictionary<TKey, T> collection,
        ObservableDictionary<TKey, T>? keysToRemove)
        where TKey : notnull
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (keysToRemove == null)
            return;

        foreach (var key in keysToRemove) collection.Remove(key); // no falla si no existe la key
    }
}