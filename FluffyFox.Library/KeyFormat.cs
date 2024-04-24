using System.Text;

namespace FluffyFox.Library;

public static class KeyFormat
{
    public static string FormatString(string originalString)
    {
        StringBuilder formattedBuilder = new();

        for (var i = 0; i < originalString.Length; i++)
        {
            formattedBuilder.Append(originalString[i]);
            if ((i + 1) % 4 == 0 && i != originalString.Length - 1)
            {
                formattedBuilder.Append(' ');
            }
        }

        return formattedBuilder.ToString();
    }
}