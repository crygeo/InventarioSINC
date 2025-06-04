using Cliente.src.View.Items;
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
using Utilidades.Interfaces;

namespace Cliente.src.View.Dialog
{
    /// <summary>
    /// Lógica de interacción para MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : UserControl, IDialog
    {
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(MessageDialog));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public MessageDialog()
        {
            InitializeComponent();
        }

        public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
        public required string DialogOpenIdentifier { get; set; }
    }
}
