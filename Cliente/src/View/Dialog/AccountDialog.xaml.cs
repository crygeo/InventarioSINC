using Cliente.src.Extencions;
using Cliente.src.Model;
using Cliente.src.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
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
using Cliente.src.Services;
using Utilidades.Interfaces;

namespace Cliente.src.View.Items
{
    /// <summary>
    /// Lógica de interacción para UsuarioItemDetall.xaml
    /// </summary>
    public partial class AccountDialog : UserControl, IDialog
    {
        public static readonly DependencyProperty UsuarioProperty = DependencyProperty.Register(nameof(Usuario), typeof(Usuario), typeof(AccountDialog));
        public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptedCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog));
        public static readonly DependencyProperty CloseSeccionCommandProperty = DependencyProperty.Register(nameof(CloseSeccionCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog));
        public static readonly DependencyProperty ChangedPasswordCommandProperty = DependencyProperty.Register(nameof(ChangedPasswordCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog));
        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(AccountDialog));
        public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(AccountDialog), new PropertyMetadata(null));
        public IAsyncRelayCommand CancelarCommand
        {
            get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
            set => SetValue(CancelarCommandProperty, value);
        }
        public Usuario Usuario
        {
            get => (Usuario)GetValue(UsuarioProperty);
            set => SetValue(UsuarioProperty, value);
        }
        public IAsyncRelayCommand AceptedCommand
        {
            get => (IAsyncRelayCommand)GetValue(AceptedCommandProperty);
            set => SetValue(AceptedCommandProperty, value);
        }
        public IAsyncRelayCommand CloseSeccionCommand
        {
            get => (IAsyncRelayCommand)GetValue(CloseSeccionCommandProperty);
            set => SetValue(CloseSeccionCommandProperty, value);
        }
        public IAsyncRelayCommand ChangedPasswordCommand
        {
            get => (IAsyncRelayCommand)GetValue(ChangedPasswordCommandProperty);
            set => SetValue(ChangedPasswordCommandProperty, value);
        }

        public string TextHeader
        {
            get => (string)GetValue(TextHeaderProperty);
            set => SetValue(TextHeaderProperty, value);
        }

        public string DialogNameIdentifier { get; set; }
        public required string DialogOpenIdentifier { get; set; }


        public AccountDialog()
        {
            InitializeComponent();
        }

        private async void ButtonChangedPass_OnClick(object sender, RoutedEventArgs e)
        {
            await ChangedPasswordCommand.TryEjecutarAsync(Usuario);
        }

        private async void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
        }

        private async void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
        {
            await CloseSeccionCommand.TryEjecutarAsync(this);
        }

    }
}
