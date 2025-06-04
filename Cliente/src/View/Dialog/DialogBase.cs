using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utilidades.Interfaces;

namespace Cliente.src.View.Dialog
{
    public class DialogBase : UserControl, IDialog
    {

        public static readonly DependencyProperty DialogNameIdentifierProperty = DependencyProperty.Register(nameof(DialogNameIdentifier), typeof(string), typeof(DialogBase), new PropertyMetadata(null));
        public static readonly DependencyProperty DialogOpenIdentifierProperty = DependencyProperty.Register(nameof(DialogOpenIdentifier), typeof(string), typeof(DialogBase), new PropertyMetadata(null));

        public string DialogNameIdentifier
        {
            get => (string) GetValue(DialogNameIdentifierProperty);
            set => SetValue(DialogNameIdentifierProperty, value);
        }

        public string DialogOpenIdentifier
        {
            get => (string)GetValue(DialogOpenIdentifierProperty);
            set => SetValue(DialogOpenIdentifierProperty, value);
        }
    }
}
