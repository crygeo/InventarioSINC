using CommunityToolkit.Mvvm.Messaging.Messages;
using Utilidades.Mvvm;

namespace Cliente.Messages;

public class NavigateToDetailMessage : ValueChangedMessage<ViewModelBase>
{
    public NavigateToDetailMessage(ViewModelBase vm) : base(vm) { }
}