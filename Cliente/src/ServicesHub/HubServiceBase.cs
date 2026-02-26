using Cliente.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Interfaces.Model;
using Shared.Request;
using Utilidades.Interfaces;

namespace Cliente.ServicesHub;

public class HubServiceBase<TEntity> : IHubService<TEntity>
    where TEntity : IModelObj
{
    private readonly HubConnection _hubConnection;
    private readonly string _hubUrl;
    private readonly List<IDisposable> _subscriptions = new();


    //Events
    public event Action<TEntity>? OnCreated;
    public event Action<TEntity>? OnUpdated;
    public event Action<string>? OnDeleted;
    public event Action<PropertyChangedEventRequest>? OnPropertyUpdated;
    public event Action<PropertyChangedEventRequest>? OnItemAddedToList;
    public event Action<PropertyChangedEventRequest>? OnItemRemovedFromList;


    public HubServiceBase()
    {
        _hubUrl = $"{Config.Url}/hub{typeof(TEntity).Name}";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect()
            .Build();

        SubscribeEvents();
    }

    #region SignalR Subscriptions

    private void SubscribeEvents()
    {
        // ➕ NEW
        SubscribeToEvent<TEntity>($"New{typeof(TEntity).Name}", entity => { OnCreated?.Invoke(entity); });

        // 🔄 UPDATE
        SubscribeToEvent<TEntity>($"Update{typeof(TEntity).Name}", entity => { OnUpdated?.Invoke(entity); });

        // ❌ DELETE
        SubscribeToEvent<TEntity>($"Delete{typeof(TEntity).Name}", entity => { OnDeleted?.Invoke(entity.Id); });

        // 🔄 UPDATE PROPERTY
        SubscribeToEvent<PropertyChangedEventRequest>($"UpdateProperty{typeof(TEntity).Name}", request => { OnPropertyUpdated?.Invoke(request); });

        // ➕ ITEM ADDED TO LIST
        SubscribeToEvent<PropertyChangedEventRequest>($"ItemAdded{typeof(TEntity).Name}",  request => { OnItemAddedToList?.Invoke(request); });

        // ❌ ITEM REMOVED FROM LIST
        SubscribeToEvent<PropertyChangedEventRequest>($"ItemRemoved{typeof(TEntity).Name}",  request => { OnItemRemovedFromList?.Invoke(request); });
    }

    #endregion

    #region Connection control

    public async Task StartConnectionAsync()
    {
        try
        {
            if (_hubConnection.State == HubConnectionState.Connected)
                return;

            await _hubConnection.StartAsync();
            Console.WriteLine($"Conectado a {_hubUrl}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar con {_hubUrl}: {ex.Message}");
        }
    }

    public async Task StopConnectionAsync()
    {
        foreach (var sub in _subscriptions)
            sub.Dispose();

        _subscriptions.Clear();

        if (_hubConnection.State != HubConnectionState.Disconnected)
            await _hubConnection.StopAsync();
    }



    #endregion

    #region Helpers

    protected void SubscribeToEvent<T>(string methodName, Action<T> action)
    {
        var subscription = _hubConnection.On(methodName, action);
        _subscriptions.Add(subscription);
    }

    protected void SubscribeToEvent<T1, T2, T3>(
        string methodName,
        Action<T1, T2, T3> action)
    {
        var subscription = _hubConnection.On(methodName, action);
        _subscriptions.Add(subscription);
    }


    #endregion
}
