using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.MyHelper
{
    public class StringHelper
    {
        public static string DayweekVN(DateTime date)
        {
            string d = date.DayOfWeek.ToString();
            switch (d)
            {
                case "Monday":
                    return "Thứ hai";
                case "Tuesday":
                    return "Thứ ba";
                case "Wednesday":
                    return "Thứ tư";
                case "Thursday":
                    return "Thứ năm";
                case "Friday":
                    return "Thứ sáu";
                case "Saturday":
                    return "Thứ bảy";
                case "Sunday":
                    return "Chủ nhật";
                default:
                    return ("");
            }
        }

        public static string StringReadmore(string text, int n=200, string str_end="...") {
            if (text != null && text != "")
            {
                text = text.Trim();
            }
            
            
            string new_text = text;
            if (text != null && text.Length > n)
            {
                int str_limit = text.IndexOf(" ", n);
                if (str_limit > n)
                {
                    new_text = text.Substring(0, str_limit);
                }
                else
                {
                    new_text = text.Substring(0, n);
                }

                if (new_text.LastIndexOf(",") == new_text.Length - 1)
                {
                    new_text = new_text.Remove(new_text.Length - 1);
                }
                return new_text + str_end;
            }
            else {
                return new_text;
            }
            
            
        }
        /// <summary>
        /// Creates a URL And SEO friendly slug
        /// </summary>
        /// <param name="text">Text to slugify</param>
        /// <param name="maxLength">Max length of slug</param>
        /// <returns>URL and SEO friendly string</returns>
        public static string UrlFriendly(string text, int maxLength = 0)
        {
            // Return empty value if text is null
            if (text == null) return "";
            var normalizedString = text
                // Make lowercase
                .ToLowerInvariant()
                // Normalize the text
                .Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            var stringLength = normalizedString.Length;
            var prevdash = false;
            var trueLength = 0;
            char c;
            for (int i = 0; i < stringLength; i++)
            {
                c = normalizedString[i];
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    // Check if the character is a letter or a digit if the character is a
                    // international character remap it to an ascii valid character
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (c < 128)
                            stringBuilder.Append(c);
                        else
                            stringBuilder.Append(ConstHelper.RemapInternationalCharToAscii(c));
                        prevdash = false;
                        trueLength = stringBuilder.Length;
                        break;
                    // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.MathSymbol:
                        if (!prevdash)
                        {
                            stringBuilder.Append('-');
                            prevdash = true;
                            trueLength = stringBuilder.Length;
                        }
                        break;
                }
                // If we are at max length, stop parsing
                if (maxLength > 0 && trueLength >= maxLength)
                    break;
            }
            // Trim excess hyphens
            var result = stringBuilder.ToString().Trim('-');
            // Remove any excess character to meet maxlength criteria
            return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
        }

        public static class ConstHelper
        {
            /// <summary>
            /// Remaps international characters to ascii compatible ones
            /// based of: https://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
            /// </summary>
            /// <param name="c">Charcter to remap</param>
            /// <returns>Remapped character</returns>
            public static string RemapInternationalCharToAscii(char c)
            {
                string s = c.ToString().ToLowerInvariant();
                if ("àåáâäãåą".Contains(s))
                {
                    return "a";
                }
                else if ("èéêëę".Contains(s))
                {
                    return "e";
                }
                else if ("ìíîïı".Contains(s))
                {
                    return "i";
                }
                else if ("òóôõöøőð".Contains(s))
                {
                    return "o";
                }
                else if ("ùúûüŭů".Contains(s))
                {
                    return "u";
                }
                else if ("çćčĉ".Contains(s))
                {
                    return "c";
                }
                else if ("żźž".Contains(s))
                {
                    return "z";
                }
                else if ("śşšŝ".Contains(s))
                {
                    return "s";
                }
                else if ("ñń".Contains(s))
                {
                    return "n";
                }
                else if ("ýÿ".Contains(s))
                {
                    return "y";
                }
                else if ("ğĝ".Contains(s))
                {
                    return "g";
                }
                else if (c == 'ř')
                {
                    return "r";
                }
                else if (c == 'ł')
                {
                    return "l";
                }
                else if (c == 'đ')
                {
                    return "d";
                }
                else if (c == 'ß')
                {
                    return "ss";
                }
                else if (c == 'þ')
                {
                    return "th";
                }
                else if (c == 'ĥ')
                {
                    return "h";
                }
                else if (c == 'ĵ')
                {
                    return "j";
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
