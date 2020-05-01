using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.SYSEmail
{
    public class MessageEmail
    {
        public string Subject { set; get; }
        public string Body { set; get; }
        public string Destination { set; get; }
    }
        
    public class MailSetting
    {
        public Boolean MailOnline { get; set; }
        public string MailFrom { get; set; }// Email cấu hình
        public string FromName { get; set; } // 
        public int Mailer { get; set; } // 1:Send mail, 2:smtp 
        public string SendMailPath { get; set; }
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; } // *
        public int SMTPSecure { get; set; } // 1:none,2:SSL/TLS ,3:STARTTLS
        public Boolean SMTPAuth { get; set; } // có sử dụng SMTP Authentication
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public string MailBody { get; set; }
    }

    public class MailFile
    {
        public string TenFileUpload { get; set; }
        public string TenFileLuu { get; set; }
        public string DiaChiFolder { get; set; }
    }

    public class MailInfo
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Files { get; set; }
        public bool SendPrivate { get; set; }
    }

    public class MailDetail
    {
        public string EmailAdress { get; set; }
        public string Subject { get; set; }
        public string TextContent { get; set; }
        public List<string> FileContent { get; set; }
    }
}