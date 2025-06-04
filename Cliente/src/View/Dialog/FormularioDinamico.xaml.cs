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
using Cliente.src.Extencions;
using Cliente.src.Services;
using Utilidades.Interfaces;

namespace Cliente.src.View.Dialog
{
    /// <summary>
    /// Lógica de interacción para FormularioDinamico.xaml
    /// </summary>
    public partial class FormularioDinamico : UserControl, IDialog
    {
        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(FormularioDinamico), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty CancelarCommandProperty = DependencyProperty.Register(nameof(CancelarCommand), typeof(IAsyncRelayCommand), typeof(FormularioDinamico), new PropertyMetadata(null));
        public static readonly DependencyProperty AceptarCommandProperty = DependencyProperty.Register(nameof(AceptarCommand), typeof(IAsyncRelayCommand), typeof(FormularioDinamico), new PropertyMetadata(null));
        public static readonly DependencyProperty DialogNameIdentifierProperty = DependencyProperty.Register(nameof(DialogNameIdentifier), typeof(string), typeof(FormularioDinamico), new PropertyMetadata(null));

        public string TextHeader
        {
            get => (string)GetValue(TextHeaderProperty);
            set => SetValue(TextHeaderProperty, value);
        }
        public IAsyncRelayCommand AceptarCommand
        {
            get => (IAsyncRelayCommand)GetValue(AceptarCommandProperty);
            set => SetValue(AceptarCommandProperty, value);
        }
        public IAsyncRelayCommand CancelarCommand
        {
            get => (IAsyncRelayCommand)GetValue(CancelarCommandProperty);
            set => SetValue(CancelarCommandProperty, value);
        }


        public object Objeto { get; }


        public FormularioDinamico(object objeto)
        {
            Objeto = objeto;
            InitializeComponent();
            GenerarCampos();

            if (GetValue(DialogNameIdentifierProperty) == null)
                SetValue(DialogNameIdentifierProperty, $"Dialog_{Guid.NewGuid():N}");
        }

        private void GenerarCampos()
        {
            var propiedades = Objeto.GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(SolicitarAttribute)));

            foreach (var propiedad in propiedades)
            {
                var atributo = propiedad.GetCustomAttribute<SolicitarAttribute>();
                string label = atributo?.Nombre ?? propiedad.Name;

                var componente = ComponetesHelp.CrearComponente(Objeto, propiedad.Name, label);

                if (componente is TextBox)
                {
                    FormTop.Children.Add(componente);
                }
                else
                {
                    FromBot.Children.Add(componente);
                }
            }
        }


        private async void OnClose(object sender, RoutedEventArgs e)
        {
            await CancelarCommand.TryEjecutarYCerrarDialogoAsync(this);
        }
        private async void OnAcepted(object sender, RoutedEventArgs e)
        {
            var errores = Objeto.ValidarCamposSolicitados();

            if (!errores.Any())
                await AceptarCommand.TryEjecutarYCerrarDialogoAsync(this, Objeto);
            else
                foreach (var error in errores)
                {
                    DialogService.Instance.MensajeQueue.Enqueue(error);
                }

            //await DialogServiceI.Instance.MostrarDialogo(string.Join("\n", errores));
        }

        public string DialogNameIdentifier
        {
            get => (string)GetValue(DialogNameIdentifierProperty);
            set => SetValue(DialogNameIdentifierProperty, value);
        }
        public required string DialogOpenIdentifier { get; set; }
    }
}
