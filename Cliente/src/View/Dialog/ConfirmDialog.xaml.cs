using Cliente.src.View.Items;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cliente.src.View.Dialog
{
    /// <summary>
    /// Lógica de interacción para MessageDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : UserControl
    {
        public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptedCommand), typeof(IAsyncRelayCommand), typeof(ConfirmDialog));
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(IAsyncRelayCommand), typeof(ConfirmDialog));
        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(ConfirmDialog));
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(ConfirmDialog));


        public IAsyncRelayCommand CancelCommand
        {
            get => (IAsyncRelayCommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }

        public IAsyncRelayCommand AceptedCommand
        {
            get => (IAsyncRelayCommand)GetValue(AceptedCommandProperty);
            set => SetValue(AceptedCommandProperty, value);
        }

        public string TextHeader
        {
            get => (string)GetValue(TextHeaderProperty);
            set => SetValue(TextHeaderProperty, value);
        }
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private async void OnCancel(object sender, RoutedEventArgs e)
        {
            await CancelCommand.ExecuteAsync(null); // Ejecuta la acción que te pasaron
        }

        private async void OnAcepted(object sender, RoutedEventArgs e)
        {
            await AceptedCommand.ExecuteAsync(null);
            DialogHost.Close(DialogService.DialogIdentifierMain);
        }
    }
}
