using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace API.Models
{
    public class StringUtilities
    {
        public static string ToEmpty(object objSrc)
        {
            if ((objSrc == null) || (objSrc == DBNull.Value))
            {
                return "";
            }

            return objSrc.ToString();
        }

        public static decimal GetExchange()
        {
            decimal usd = 1;
            string strWrite = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("http://www.vietcombank.com.vn/ExchangeRates/ExrateXML.aspx");

                XmlNodeList list = doc.DocumentElement.SelectNodes("Exrate");
                foreach (XmlNode fnode in list)
                {
                    if (fnode.Attributes["CurrencyCode"].Value.ToUpper() == "USD")
                    {
                        usd = decimal.Parse(fnode.Attributes["Sell"].Value);
                    }
                }

            }
            catch (Exception ex)
            {
                strWrite = ex.Message;
            }
            return usd;

        }

        public static double GetHourFrom2Date(DateTime date1, DateTime date2)
        {
            TimeSpan diff = date2 - date1;
            double hours = diff.TotalHours;
            return hours;
        }



        /// <summary>
        /// Remove all Accents
        /// </summary>
        /// <param name="text"></param>
        /// <returns>String Without Accents</returns>
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string ToCorrectFilmName(object value)
        {
            string my_String = RemoveDiacritics(value.ToString());
            my_String = Regex.Replace(my_String, @"[^0-9a-zA-Z_]+", " ");
            return my_String.Trim()
                .Replace("Đ", "d")
                .Replace("đ", "d")
                .Replace("&", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace(" - ", " ")
                .Replace("  ", " ")
                .Replace("%", "")
                .Replace(",", "")
                .Replace(" ", "_")
                .ToLower();
        }

        public static string ToNameRewrite(object value)
        {
            string my_String = RemoveDiacritics(value.ToString());
            my_String = Regex.Replace(my_String, @"[^0-9a-zA-Z]+", " ");
            return my_String.Trim()
                .Replace("Đ", "d")
                .Replace("đ", "d")
                .Replace("&", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace(" - ", " ")
                .Replace("  ", " ")
                .Replace("%", "")
                .Replace(",", "")
                .Replace(" ", "-")
                .ToLower();
        }

        public static string toVNDate(DateTime date)
        {
            return String.Format("{0: ngày dd tháng MM năm yyyy}", date);
        }

        public static string dayweekVN(String d)
        {
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

        public static string Truncate(object value, int lenght)
        {
            if (value == null)
                return "";
            string val = value.ToString();
            string result = "";
            if (val.Length <= lenght)
            {
                result = val;
            }
            else
            {
                int index = lenght;
                while (val[index] != ' ')
                {
                    index--;
                }
                if (index == 0 || index == -1) index = lenght;

                result = val.Substring(0, index) + "...";
            }
            return result;
        }

        public static string RemoveHtmlTags(object source)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            source = regex.Replace(source.ToString(), @" ");
            return Regex.Replace(source.ToString(), "<.*?>", string.Empty);

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void WriteLog(string message, string path)
        {
            using (StreamWriter w = File.AppendText(path))
            {
                Log(message, w);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
