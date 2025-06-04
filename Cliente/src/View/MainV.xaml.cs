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
using System.Windows.Shapes;
using Cliente.src.Services;

namespace Cliente.src.View
{
    /// <summary>
    /// Lógica de interacción para MainV.xaml
    /// </summary>
    public partial class MainV : Window
    {
        public MainV()
        {
            InitializeComponent();
            SnackbarThree.MessageQueue = DialogService.Instance.MensajeQueue;
        }
    }
}
