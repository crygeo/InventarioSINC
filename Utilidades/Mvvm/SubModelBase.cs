namespace Utilidades.Mvvm;

public class SubViewModelBase : ViewModelBase
{
    public SubViewModelBase(ViewModelBase viewModelBaseMain, string Id)
    {
        ViewModelBaseMain = viewModelBaseMain;
        ID = Id;
    }

    public ViewModelBase ViewModelBaseMain { get; }
    public string ID { get; set; }

    protected override void UpdateChanged()
    {
    }
}