using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace FluffyFox.Helpers;

public class KeyFormatConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is string userKey)
		{
			string formattedKey = FormatString(userKey);
			return formattedKey;
		}

		return value;
	}

	public static string FormatString(string originalString)
	{
		StringBuilder formattedBuilder = new();

		for (int i = 0; i < originalString.Length; i++)
		{
			formattedBuilder.Append(originalString[i]);
			if ((i + 1) % 4 == 0 && i != originalString.Length - 1)
			{
				formattedBuilder.Append(' ');
			}
		}

		return formattedBuilder.ToString();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}