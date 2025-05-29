<<<<<<< HEAD
﻿ using Cliente.src.Model;
=======
﻿using Cliente.src.Attributes;
using Cliente.src.Model;
>>>>>>> 29/05/2025
using System;
using System.Collections;
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

namespace Cliente.src.View.Items
{

    /// <summary>
    /// Lógica de interacción para ObjectIList.xaml
    /// </summary>
    public partial class ObjectIList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ObjectIList));
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(object), typeof(ObjectIList));
<<<<<<< HEAD
=======
        public static readonly DependencyProperty TypeItemProperty = DependencyProperty.Register(nameof(TypeItem), typeof(Type), typeof(ObjectIList));
>>>>>>> 29/05/2025

        public static readonly DependencyProperty EditarItemCommandProperty = DependencyProperty.Register(nameof(EditarItemCommand), typeof(ICommand), typeof(ObjectIList));
        public static readonly DependencyProperty EliminarItemCommandProperty =DependencyProperty.Register(nameof(EliminarItemCommand), typeof(ICommand), typeof(ObjectIList));



        public ICommand EditarItemCommand
        {
            get => (ICommand)GetValue(EditarItemCommandProperty);
            set => SetValue(EditarItemCommandProperty, value);
        }

        public ICommand EliminarItemCommand
        {
            get => (ICommand)GetValue(EliminarItemCommandProperty);
            set => SetValue(EliminarItemCommandProperty, value);
        }
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
<<<<<<< HEAD
        public object Item
        {
            get => (object)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
=======

        public object Item
        {
            get => (object)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public Type TypeItem
        {
            get => (Type)GetValue(TypeItemProperty);
            set => SetValue(TypeItemProperty, value);
>>>>>>> 29/05/2025
        }

        public ObjectIList()
        {
            InitializeComponent();
            //DataContext = this;
<<<<<<< HEAD
=======

            this.Loaded += ObjectIList_Loaded;

>>>>>>> 29/05/2025
        }

        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && Keyboard.IsKeyDown(Key.LeftShift))
            {
                item.IsSelected = !item.IsSelected;
                e.Handled = true; // Evita que WPF cambie la selección predeterminada
            }
        }

<<<<<<< HEAD
=======
        private void ObjectIList_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateColumns();
        }

        private void GenerateColumns()
        {
            TrySetItemType();

            if (TypeItem == null)
                return;

            var properties = TypeItem.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(SolicitarAttribute)));

            DataGridSolicitados.Columns.Clear();

            foreach (var prop in properties)
            {
                DataGridSolicitados.Columns.Add(new DataGridTextColumn
                {
                    Header = prop.Name,
                    Binding = new Binding(prop.Name)
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

>>>>>>> 29/05/2025
    }
}
