using System.Globalization;

namespace NetGuardUI.CommonFunction
{
    public class CommonFunctions
    {
        public static bool IsBase64String(string input)
        {
            try
            {
                // Attempt to decode the input string as Base64
                byte[] buffer = Convert.FromBase64String(input);

                return true;
            }
            catch (FormatException)
            {
                // If decoding fails, the input is not a valid Base64 string
                return false;
            }
        }
        public static string CleanImageUrl(string uploadedImageUrl)
        {
            const string prefix = "data:image/png;base64,";
            if (uploadedImageUrl.StartsWith(prefix))
            {
                return uploadedImageUrl.Substring(prefix.Length);
            }
            return uploadedImageUrl;
        }




    }

    public static class StringExtensions
    {
        public static string TitleCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return new CultureInfo("en-US", false).TextInfo.ToTitleCase(text.ToLower());
        }
    }
}
