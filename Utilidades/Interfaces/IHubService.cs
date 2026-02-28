using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Interfaces;
using Shared.Request;

namespace Utilidades.Interfaces;

public interface IHubService<TEntity> where TEntity : IIdentifiable, IUpdatable
{
    event Action<TEntity> OnCreated;
    event Action<TEntity> OnUpdated;
    event Action<string> OnDeleted;
    
    event Action<PropertyChangedEventRequest> OnPropertyUpdated;
    event Action<PropertyChangedEventRequest> OnItemAddedToList;
    event Action<PropertyChangedEventRequest> OnItemRemovedFromList;
    
    Task StartConnectionAsync();
    Task StopConnectionAsync();


}