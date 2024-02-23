using System.Windows.Controls;
using System.Windows.Data;

namespace Yomicchi.Desktop.Converters
{
    public class ItemIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var itemsControl = values[0] as ItemsControl;
            if (itemsControl == null)
            {
                return Binding.DoNothing;
            }

            var item = values[1];
            var itemContainer = itemsControl.ItemContainerGenerator.ContainerFromItem(item);

            if (itemContainer == null)
            {
                return Binding.DoNothing;
            }

            var itemIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer);
            return (itemIndex + 1).ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return targetTypes.Select(t => Binding.DoNothing).ToArray();
        }
    }
}
