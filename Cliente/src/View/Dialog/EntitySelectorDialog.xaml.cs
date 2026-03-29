using System.Windows;
using System.Windows.Controls;
using Cliente.Helpers;
using Cliente.View.Dialog.ViewModel;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

public partial class EntitySelectorDialog : UserControl, IDialog, IDialogLifecycle
{
    private bool _columnsGenerated;

    public EntitySelectorDialog()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    // =========================================================
    // IDialog
    // =========================================================

    public string TextHeader { get; set; } = string.Empty;
    public string DialogNameIdentifier { get; set; } = string.Empty;
    public string DialogOpenIdentifier { get; set; } = string.Empty;
    public IAsyncRelayCommand AceptarCommand { get; set; } = null!;

    // =========================================================
    // Generación de columnas
    // =========================================================

    private EntitySelectorDialogViewModel? Vm =>
        DataContext as EntitySelectorDialogViewModel;

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is EntitySelectorDialogViewModel oldVm)
            oldVm.Results.CollectionChanged -= OnResultsChanged;

        if (e.NewValue is EntitySelectorDialogViewModel newVm)
            newVm.Results.CollectionChanged += OnResultsChanged;
    }

    private void OnResultsChanged(object? sender,
        System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // Generar columnas solo una vez, cuando llega el primer resultado
        if (_columnsGenerated || Vm == null) return;
        if (e.NewItems == null || e.NewItems.Count == 0) return;

        _columnsGenerated = true;
        DataGridColumnBuilder.BuildColumns(ResultsGrid, Vm.EntityType);
    }
    
    public void OnOpened()
    {
        if (DataContext is EntitySelectorDialogViewModel vm)
            _ = vm.LoadDataDefaultAsync();
    }

    public void OnClosed()
    {
        throw new NotImplementedException();
    }
}