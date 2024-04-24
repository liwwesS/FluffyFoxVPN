using System.Globalization;
using System.Windows.Data;
using FluffyFox.Library;

namespace FluffyFox.Helpers;

public class KeyFormatConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not string userKey) return value;
		var formattedKey = KeyFormat.FormatString(userKey);
		return formattedKey;

	}
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}