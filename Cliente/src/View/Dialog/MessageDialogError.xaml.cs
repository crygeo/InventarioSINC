using Cliente.src.View.Items;
using Shared.Interfaces.ModelsBase;
using Shared.ObjectsResponse;
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
    public partial class MessageDialogError : UserControl, IDialog
    {
        public static readonly DependencyProperty ErrorResponseProperty = DependencyProperty.Register(nameof(ErrorResponse), typeof(object), typeof(MessageDialogError));

        public object? ErrorResponse
        {
            get => GetValue(ErrorResponseProperty);
            set => SetValue(ErrorResponseProperty, value);
        }

        public MessageDialogError()
        {
            InitializeComponent();
        }

        public IResultResponse<T> GetErrorResponse<T>() => (IResultResponse<T>)ErrorResponse!;
        public void SetErrorResponse<T>(IResultResponse<T> value) => ErrorResponse = value;


        public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
        public required string DialogOpenIdentifier { get; set; }
    }
}
