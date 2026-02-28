using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public class ObservableDictionary<TKey, TValue> :
    NotifyProperty,
    IDictionary<TKey, TValue>,
    INotifyCollectionChanged
{
    private readonly Dictionary<TKey, TValue> _dictionary;

    public ObservableDictionary()
    {
        _dictionary = new Dictionary<TKey, TValue>();
    }

    public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = new Dictionary<TKey, TValue>(dictionary);
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            if (_dictionary.TryGetValue(key, out var oldValue))
            {
                OnPropertyChanging(nameof(Values));
                _dictionary[key] = value;

                // Notifica cambio general (la UI se refresca completamente)
                CollectionChanged?.Invoke(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));


                OnPropertyChanged(nameof(Values));
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;
    public int Count => _dictionary.Count;
    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        OnPropertyChanging(nameof(Count));
        _dictionary.Add(key, value);

        CollectionChanged?.Invoke(this,
            new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add,
                new KeyValuePair<TKey, TValue>(key, value)));

        OnPropertyChanged(nameof(Count));
    }

    public bool Remove(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value) && _dictionary.Remove(key))
        {
            OnPropertyChanging(nameof(Count));

            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new KeyValuePair<TKey, TValue>(key, value)));

            OnPropertyChanged(nameof(Count));
            return true;
        }

        return false;
    }

    public void Clear()
    {
        OnPropertyChanging(nameof(Count));
        _dictionary.Clear();

        CollectionChanged?.Invoke(this,
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        OnPropertyChanged(nameof(Count));
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected override void UpdateChanged()
    {
        // Puedes usarlo para disparar lógica adicional si lo deseas
        // Ej: marcar un "IsDirty = true" para auditoría o persistencia
    }
}