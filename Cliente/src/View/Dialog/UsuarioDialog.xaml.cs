using Cliente.src.Model;
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

namespace Cliente.src.View.Items
{
    /// <summary>
    /// Lógica de interacción para UsuarioItemDetall.xaml
    /// </summary>
    public partial class UsuarioDialog : UserControl
    {
        public static readonly DependencyProperty UsuarioProperty = DependencyProperty.Register(nameof(Usuario), typeof(Usuario), typeof(UsuarioDialog));
        public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptedCommand), typeof(ICommand), typeof(UsuarioDialog));
        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(UsuarioDialog));

        public Usuario Usuario
        {
            get => (Usuario)GetValue(UsuarioProperty);
            set => SetValue(UsuarioProperty, value);
        }
        public ICommand AceptedCommand
        {
            get => (ICommand)GetValue(AceptedCommandProperty);
            set => SetValue(AceptedCommandProperty, value);
        }

        public string TextHeader
        {
            get => (string)GetValue(TextHeaderProperty);
            set => SetValue(TextHeaderProperty, value);
        }

        public UsuarioDialog()
        {
            InitializeComponent();
        }
    }
}
