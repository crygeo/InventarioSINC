using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Attributes;

namespace Utilidades.Services;

public class AsyncCollectionStorage<TItem>
{
    private readonly string _fileName;
    private readonly ConcurrentQueue<TItem> _writeQueue = new();
    private bool _isWriting = false;

    private int _isWritingFlag;

    public AsyncCollectionStorage(string fileName)
    {
        _fileName = fileName;
    }

    /// <summary>
    ///     Agrega un elemento y lo encola para guardar en archivo.
    /// </summary>
    public void Add(TItem item)
    {
        _writeQueue.Enqueue(item);
        _ = ProcessQueueAsync();
    }

    /// <summary>
    ///     Implementa la lógica de agregar un item a la colección según el tipo.
    /// </summary>
    /// <summary>
    ///     Extrae la key si es un diccionario. Debes definir cómo obtenerla de TItem.
    /// </summary>
    private object GetKey(TItem item)
    {
        var prop = typeof(TItem).GetProperties()
            .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));
        if (prop == null) throw new InvalidOperationException("No se encontró propiedad con [Key].");
        return prop.GetValue(item)!;
    }

    private async Task ProcessQueueAsync()
    {
        if (Interlocked.Exchange(ref _isWritingFlag, 1) == 1) return;

        try
        {
            while (_writeQueue.TryDequeue(out var item)) await AppendItemAsync(item);
        }
        finally
        {
            Interlocked.Exchange(ref _isWritingFlag, 0);
        }
    }

    public async Task SaveAllAsync(IEnumerable<TItem> items)
    {
        using var stream = new StreamWriter(_fileName, false, Encoding.UTF8);
        foreach (var item in items)
        {
            var json = JsonConvert.SerializeObject(item);
            await stream.WriteLineAsync(json);
        }
    }


    private async Task AppendItemAsync(TItem item)
    {
        using var stream = new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, true);
        var json = JsonConvert.SerializeObject(item) + "\n";
        var bytes = Encoding.UTF8.GetBytes(json);
        await stream.WriteAsync(bytes);
        await stream.FlushAsync();
    }

    public async Task<List<TItem>> LoadAsync()
    {
        var collection = new List<TItem>();
        if (!File.Exists(_fileName)) return collection;

        using var reader = new StreamReader(_fileName);
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            try
            {
                var obj = JsonConvert.DeserializeObject<TItem>(line);
                if (obj != null) collection.Add(obj);
            }
            catch
            {
                /* ignorar líneas corruptas */
            }

        return collection;
    }
}