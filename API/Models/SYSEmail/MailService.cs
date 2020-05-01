using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;


namespace API.Models.SYSEmail
{
    ///<Summary>
    /// MailService
    ///</Summary>
    public class MailService
    {
        
        ///<Summary>
        /// Check validate email
        ///</Summary>
        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            try
            {
                if (re.IsMatch(inputEmail))
                    return true;
                else
                    return false;

            }
            catch {
                return false;
            }
            
        }
    }
}