using Cliente.src.Attributes;
using Cliente.src.Extencions;
using Cliente.src.Model;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
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

namespace Cliente.src.View.Items
{

    /// <summary>
    /// Lógica de interacción para ObjectIList.xaml
    /// </summary>
    public partial class ObjectIList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ObjectIList));
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(object), typeof(ObjectIList));
        public static readonly DependencyProperty TypeItemProperty = DependencyProperty.Register(nameof(TypeItem), typeof(Type), typeof(ObjectIList));

        public static readonly DependencyProperty EditarItemCommandProperty = DependencyProperty.Register(nameof(EditarItemCommand), typeof(IAsyncRelayCommand), typeof(ObjectIList));
        public static readonly DependencyProperty EliminarItemCommandProperty =DependencyProperty.Register(nameof(EliminarItemCommand), typeof(IAsyncRelayCommand), typeof(ObjectIList));



        public IAsyncRelayCommand EditarItemCommand
        {
            get => (IAsyncRelayCommand)GetValue(EditarItemCommandProperty);
            set => SetValue(EditarItemCommandProperty, value);
        }
        public IAsyncRelayCommand EliminarItemCommand
        {
            get => (IAsyncRelayCommand)GetValue(EliminarItemCommandProperty);
            set => SetValue(EliminarItemCommandProperty, value);
        }
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object Item
        {
            get => (object)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public Type TypeItem
        {
            get => (Type)GetValue(TypeItemProperty);
            set => SetValue(TypeItemProperty, value);
        }

        public ObjectIList()
        {
            InitializeComponent();
            //DataContext = this;

            this.Loaded += ObjectIList_Loaded;

        }

        private void ObjectIList_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateColumns();
        }

        private void GenerateColumns()
        {
            TrySetItemType();

            if (TypeItem == null)
                throw new Exception($"TypeItem is null");

            var properties = TypeItem.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(SolicitarAttribute)));

            DataGridSolicitados.Columns.Clear();

            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<SolicitarAttribute>();
                var displayName = attr?.Nombre ?? prop.Name;

                var isEnumerable = typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
                                   && prop.PropertyType != typeof(string);

                Binding binding;
                if (isEnumerable)
                {
                    binding = new Binding($"{prop.Name}.Count");
                }
                else
                {
                    binding = new Binding(prop.Name);
                }

                DataGridSolicitados.Columns.Add(new DataGridTextColumn
                {
                    Header = displayName,
                    Binding = binding
                });
            }
        }


        private void TrySetItemType()
        {
            if (TypeItem != null)
                return;

            var item = ItemsSource?.Cast<object>()?.FirstOrDefault();
            if (item != null)
                TypeItem = item.GetType();
        }

    }
}
