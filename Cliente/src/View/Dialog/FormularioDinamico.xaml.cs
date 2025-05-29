using Cliente.src.Attributes;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Lógica de interacción para FormularioDinamico.xaml
    /// </summary>
    public partial class FormularioDinamico : UserControl
    {
        public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(FormularioDinamico), new PropertyMetadata(null));
        public IAsyncRelayCommand AceptarCommand
        {
            get => (IAsyncRelayCommand)GetValue(AceptarCommandProperty);
            set => SetValue(AceptarCommandProperty, value);
        }

        public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(FormularioDinamico), new PropertyMetadata(null));
        public IAsyncRelayCommand CancelarCommand
        {
            get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
            set => SetValue(CancelarCommandProperty, value);
        }

        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(FormularioDinamico), new PropertyMetadata(string.Empty));
        public string TextHeader
        {
            get => (string)GetValue(TextHeaderProperty);
            set => SetValue(TextHeaderProperty, value);
        }

        public object Objeto { get; }

        public FormularioDinamico(object objeto)
        {
            Objeto = objeto;
            InitializeComponent();
            GenerarCampos();
        }

        private void GenerarCampos()
        {
            var propiedades = Objeto.GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(SolicitarAttribute)));

            foreach (var propiedad in propiedades)
            {
                var atributo = propiedad.GetCustomAttribute<SolicitarAttribute>();
                string label = atributo?.Label ?? propiedad.Name;

                var lbl = new TextBlock { Text = label, Margin = new Thickness(0, 5, 0, 0) };
                var txt = new TextBox { DataContext = Objeto, Margin = new Thickness(0, 0, 0, 10) };

                var binding = new Binding(propiedad.Name)
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                txt.SetBinding(TextBox.TextProperty, binding);

                Form.Children.Add(lbl);
                Form.Children.Add(txt);
            }
        }

        private async void OnClose(object sender, RoutedEventArgs e)
        {
            if (CancelarCommand?.CanExecute(null) == true)
                await CancelarCommand.ExecuteAsync(null);

            await DialogService.Instance.CerrarSiEstaAbiertoYEsperar(DialogService.DialogIdentifierMain);
        }

        private async void OnAcepted(object sender, RoutedEventArgs e)
        {
            if (AceptarCommand?.CanExecute(Objeto) == true)
                await AceptarCommand.ExecuteAsync(Objeto);
         
            await DialogService.Instance.CerrarSiEstaAbiertoYEsperar(DialogService.DialogIdentifierMain);
        }
    }
}
