﻿using Cliente.src.View.Items;
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
using CommunityToolkit.Mvvm.Input;
using Utilidades.Interfaces;

namespace Cliente.src.View.Dialog
{
    /// <summary>
    /// Lógica de interacción para MessageDialog.xaml
    /// </summary>
    public partial class ChangePassDialog : UserControl, IDialog
    {
        public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptedCommand), typeof(IAsyncRelayCommand), typeof(ChangePassDialog));
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(IAsyncRelayCommand), typeof(ChangePassDialog));
        public static readonly DependencyProperty OldPasswordProperty = DependencyProperty.Register(nameof(OldPassword), typeof(string), typeof(ChangePassDialog), new PropertyMetadata(""));
        public static readonly DependencyProperty NewPasswordProperty = DependencyProperty.Register(nameof(NewPassword), typeof(string), typeof(ChangePassDialog));
        public static readonly DependencyProperty ConfirmPasswordProperty = DependencyProperty.Register(nameof(ConfirmPassword), typeof(string), typeof(ChangePassDialog));
        public static readonly DependencyProperty OldPasswordRequiredProperty = DependencyProperty.Register(nameof(OldPasswordRequired), typeof(Visibility), typeof(ChangePassDialog));
        public static readonly DependencyProperty DialogNameIdentifierProperty = DependencyProperty.Register(nameof(DialogNameIdentifier), typeof(string), typeof(ChangePassDialog));


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

        public string OldPassword
        {
            get => (string)GetValue(OldPasswordProperty);
            set => SetValue(OldPasswordProperty, value);
        }
        public string NewPassword
        {
            get => (string)GetValue(NewPasswordProperty);
            set => SetValue(NewPasswordProperty, value);
        }
        public string ConfirmPassword
        {
            get => (string)GetValue(ConfirmPasswordProperty);
            set => SetValue(ConfirmPasswordProperty, value);
        }
        public Visibility OldPasswordRequired
        {
            get => (Visibility)GetValue(OldPasswordRequiredProperty);
            set => SetValue(OldPasswordRequiredProperty, value);
        }
        public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";

        public required string DialogOpenIdentifier
        {
            get => (string)GetValue(DialogNameIdentifierProperty);
            set => SetValue(DialogNameIdentifierProperty, value);
        }

        public ChangePassDialog()
        {
            InitializeComponent();
        }

        private async void OnCancel(object sender, RoutedEventArgs e)
        {
            await CancelCommand.TryEjecutarYCerrarDialogoAsync(this);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewPassword == ConfirmPassword && !string.IsNullOrEmpty(NewPassword))
                await AceptedCommand.TryEjecutarYCerrarDialogoAsync(this);
        }

    }
}
