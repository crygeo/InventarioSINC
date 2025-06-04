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
using Cliente.src.Extencions;
using Utilidades.Interfaces;

namespace Cliente.src.View.Dialog
{
    /// <summary>
    /// Lógica de interacción para MessageDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : UserControl, IDialog
    {
        public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(ConfirmDialog));
        public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(ConfirmDialog));
        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(ConfirmDialog));
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(ConfirmDialog));


        public IAsyncRelayCommand CancelarCommand
        {
            get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
            set => SetValue(CancelarCommandProperty, value);
        }

        public IAsyncRelayCommand AceptarCommand
        {
            get => (IAsyncRelayCommand)GetValue(AceptarCommandProperty);
            set => SetValue(AceptarCommandProperty, value);
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

        public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
        public required string DialogOpenIdentifier { get; set; }

        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private async void OnCancel(object sender, RoutedEventArgs e)
        {
            await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
        }

        private async void OnAcepted(object sender, RoutedEventArgs e)
        {
            await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this);
        }
    }
}
