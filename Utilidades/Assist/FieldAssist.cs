using System.Windows;
using Utilidades.Controls;

namespace Utilidades.Assist;

public static class FieldAssist
{
    public static string GetLabel(DependencyObject obj) => (string)obj.GetValue(LabelProperty);

    public static void SetLabel(DependencyObject obj, string value) => obj.SetValue(LabelProperty, value);

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.RegisterAttached(
            "Label",
            typeof(string),
            typeof(FieldAssist),
            new PropertyMetadata(default(string)));



    public static InputBoxType GetFormat(DependencyObject obj)
        => (InputBoxType)obj.GetValue(FormatProperty);

    public static void SetFormat(DependencyObject obj, InputBoxType value)
        => obj.SetValue(FormatProperty, value);

    public static readonly DependencyProperty FormatProperty =
        DependencyProperty.RegisterAttached(
            "Format",
            typeof(InputBoxType),
            typeof(FieldAssist),
            new PropertyMetadata(InputBoxType.None));
}