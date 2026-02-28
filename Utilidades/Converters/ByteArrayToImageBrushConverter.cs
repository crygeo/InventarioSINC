using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Utilidades.Converter;

public class ByteArrayToImageBrushConverter : IValueConverter
{
    private static readonly Dictionary<int, BitmapImage> _cache = new();

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter">[bool] true si quiere mostrar la imagen por defecto</param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var useDefaultImage = false;
        if (parameter is string paramStr)
            bool.TryParse(paramStr, out useDefaultImage);

        if (value is byte[] bytes && bytes.Length > 0)
        {
            var hash = bytes.GetHashCode(); // o un SHA1 si quieres evitar colisiones
            if (_cache.TryGetValue(hash, out var cached))
                return cached;

            using var stream = new MemoryStream(bytes);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            bitmap.Freeze();

            _cache[hash] = bitmap;
            return bitmap;
        }

        if (useDefaultImage)
        {
            // Devuelves un DrawingImage como default
            var geometry = Geometry.Parse(
                "M256.49 512.611c-39.223 0-76.087-8.051-109.57-23.929-9.98-4.733-14.234-16.66-9.501-26.641s16.661-14.234 26.641-9.501c182.603 84.249 373.961-104.925 287.358-288.361-10.496-24.22 23.859-40.738 36.143-17.138 81.565 167.692-45.06 367.114-231.071 365.57zM52.124 376.652c9.964-4.768 14.175-16.71 9.407-26.674C-25.292 167.823 166.08-25.438 348.923 61.684c9.98 4.732 21.906.477 26.638-9.503s.479-21.908-9.501-26.641C147.806-76.935-76.809 149.894 25.452 367.246c4.782 10.061 16.93 14.133 26.672 9.406zM34.632 506.753l472-472c7.811-7.811 7.811-20.474 0-28.284s-20.474-7.811-28.284 0l-472 472c-7.811 7.811-7.811 20.474 0 28.284 7.811 7.811 20.474 7.811 28.284 0z");
            var drawing = new GeometryDrawing(Brushes.Gray, null, geometry);
            var drawingImage = new DrawingImage(drawing);
            drawingImage.Freeze();
            return drawingImage;
        }

        return new ImageBrush();
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}