using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace API.Areas.Admin.Models.USUsers
{
    public class USUsersService
    {
        public const string SecretXSRF = "7f71f3acb09cf48e22b99c0b694c238ddschangkho"; // your symmetric

        public static List<USUsers> GetListPagination(SearchUSUsers dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<USUsers>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new USUsers
                        {
                            Id = (int)r["Id"],
                            UserName = (string)r["UserName"],
                            Status = (byte)r["Status"],
                            Gender = (byte)r["Gender"],
                            Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                            Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                            GroupsTitle = (string)((r["GroupsTitle"] == System.DBNull.Value) ? "" : r["GroupsTitle"]),
                            Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),                            
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                            IdCoQuan = (int)r["IdCoQuan"],
                            IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                            TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        }).ToList();
            }


        }

        public static List<USUsers> GetListByGroup(int IdGroup)
        {
            
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@IdGroup" },
                new object[] { "GetListByGroup", IdGroup });
            return (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        UserName = (string)r["UserName"],
                        Status = (byte)r["Status"],
                        Gender = (byte)r["Gender"],
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                        Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        GroupsTitle = (string)((r["GroupsTitle"] == System.DBNull.Value) ? "" : r["GroupsTitle"]),
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),                        
                        TotalRows = (int)r["TotalRows"],
                        IdCoQuan = (int)r["IdCoQuan"],
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                        TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                    }).ToList();


        }

        public static List<SelectListItem> GetListItemsAuthor(int IdGroup)
        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@IdGroup" },
                new object[] { "GetListByGroup", IdGroup });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["FullName"] == System.DBNull.Value) ? "Không có tên" : r["FullName"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "Chọn Tác Giả", Value = "0" }));
            return ListItems;
        }


        public static List<SelectListItem> GetListAll()
        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag" },
                new object[] { "GetListAll" });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["FullName"] == System.DBNull.Value) ? "Không có tên" : r["FullName"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "Chọn người đăng", Value = "0" }));
            return ListItems;
        }


        public static async Task<UserToken> BuildUserToken(dynamic item)
        {
            return new UserToken()
            {
                UserName = item.UserName,
                Email = item.Email,
                Id = item.Id,
                IdCoQuan = item.IdCoQuan,
                FullName = item.FullName,
                UserCode = item.UserCode,                
                IdGroup = item.IdGroup,                
                Avatar = item.Avatar,                
            };

        }
       

        public static dynamic ChangePassword(int Id, string Password)
        {
            try
            {
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",                
                    new string[] { "@flag", "@Id", "@Password", "@ModifiedDate" },
                    new object[] { "ChangePassword", Id, Password, DateTime.Now });
                return new { N = Id };
            }
            catch
            {
                return new { N = 0 };
            }
        }

        public static USUsers CheckPassword(int Id, string Password)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag","@Id", "@Password" }, new object[] { "CheckPassword", Id, Password });

            return (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                        Password = (string)((r["Password"] == System.DBNull.Value) ? "" : r["Password"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                    }).FirstOrDefault();
        }


        public static List<SelectListItem> GetStatusSelectItems()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
               new string[] { "@flag" }, new object[] { "GetListTrangThai" });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();          
            return ListItems;
        }
        

        public static USUsers GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] {"@flag", "@Id" }, new object[] {"GetItem", Id });
            USUsers item = (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        IdGroup = (int)r["IdGroup"],
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),                        
                        UserCode = (string)((r["UserCode"] == System.DBNull.Value) ? "" : r["UserCode"]),
                        Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                        Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                        Birthday = (int)((r["Birthday"] == System.DBNull.Value) ? Int32.Parse(DateTime.Now.ToString("yyyyMMdd")) : r["Birthday"]),                        
                        Status = (byte)r["Status"],                        
                        Gender = (byte)r["Gender"],
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),                        
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId)
                    }).FirstOrDefault();

            if (item.Birthday == 0)
            {
                item.BirthdayShow = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else {
                item.BirthdayShow = DateTime.ParseExact(item.Birthday.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            }
            
            return item;
        }

        public static USUsers GetItemByGroup(int Id, int IdGroup)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@IdGroup" }, new object[] { "GetItemByGroup", Id, IdGroup });
            return (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        IdGroup = (int)r["IdGroup"],
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                        UserCode = (string)((r["UserCode"] == System.DBNull.Value) ? "" : r["UserCode"]),
                        Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                        TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                        Status = (byte)r["Status"],
                        Gender = (byte)r["Gender"],
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                        IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),                        
                    }).FirstOrDefault();
        }




        public static USUsers CheckLogin(string UserName, string Password)
        {
            string md5pass = USUsersService.GetMD5(Password);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] {"@flag", "@UserName", "@Password" }, 
                new object[] { "Login", UserName.ToLower(), md5pass });

            return (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        IdGroup = (int)r["IdGroup"],
                        UserName = (string)r["UserName"],
                        Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        UserCode = (string)((r["UserCode"] == System.DBNull.Value) ? "" : r["UserCode"]),
                        Gender = (byte)r["Gender"],
                        IdCoQuan = (int)r["IdCoQuan"],
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(USUsers dto)
        {
            DateTime Birthday = DateTime.Now;
            if (dto.BirthdayShow != null) {
                Birthday = DateTime.ParseExact(dto.BirthdayShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }            
            dto.Birthday= Int32.Parse(Birthday.ToString("yyyyMMdd"));
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag","@Id", "@UserName", "@IdCoQuan", "@Password", "@UserCode", "@FullName", "@Telephone", "@Email", "@Birthday", "@Fax", "@IdGroup", "@Description", "@Status", "@ModifiedBy", "@CreatedBy", "@Gender", "@Specialize", "@IdRegency" }, 
            new object[] { "SaveItem", dto.Id, dto.UserName,dto.IdCoQuan, dto.Password, dto.UserCode, dto.FullName,  dto.Telephone, dto.Email,dto.Birthday,dto.Fax,dto.IdGroup,dto.Description,dto.Status,dto.ModifiedBy,dto.CreatedBy,dto.Gender,dto.Specialize ,dto.IdRegency});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(USUsers dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveAccountInfo(USUsers dto)
        {
            DateTime Birthday = DateTime.Now;
            if (dto.BirthdayShow != null)
            {
                Birthday = DateTime.ParseExact(dto.BirthdayShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            dto.Birthday = Int32.Parse(Birthday.ToString("yyyyMMdd"));
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@FullName", "@Status", "@Telephone", "@Email", "@Description", "@Gender","@Password", "@Birthday" , "@UserCode" , "@Specialize" , "@IdRegency" },
            new object[] { "SaveAccountInfo", dto.Id, dto.FullName, dto.Status, dto.Telephone, dto.Email,dto.Description,dto.Gender ,dto.Password ,dto.Birthday, dto.UserCode,dto.Specialize,dto.IdRegency });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveAccountAvatar(USUsers dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@Avatar" },
            new object[] { "SaveAccountAvatar", dto.Id, dto.Avatar });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        
        public static string GetMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }

        public static XSRF CreateXSRF(string SecurityDateTime = null, string Token = null)
        {
            XSRF item = new XSRF();
            if (SecurityDateTime == null || SecurityDateTime == "")
            {
                item.DateTime = string.Format("{0:ddMMyyHHmmss}", DateTime.Now);
            }
            string Secret = USUsersService.SecretXSRF + item.DateTime + Token;
            item.X_XSRF_TOKEN = USUsersService.GetMD5(Secret);
            return item;
        }

        public static Boolean CheckXSRF(XSRF item)
        {
            Boolean flag = false;
            try
            {
                string Secret = USUsersService.SecretXSRF + item.DateTime + item.Token;
                string X_XSRF_TOKEN = USUsersService.GetMD5(Secret);
                if (X_XSRF_TOKEN == item.X_XSRF_TOKEN)
                { flag = true; }
                return flag;
            }
            catch
            {
                return false;
            }

        }

        public static Boolean ValidateStrongPassword(string password)
        {
            Boolean upp = false;
            Boolean low = false;
            Boolean num = false;
            Boolean spc = false;
            Boolean cnt = false;
            Boolean noRpt = true;//Assume three repeating characters not found            
            Boolean rng = true;// Assume characters are within acceptable range
            if (password.Length > 6)
            {
                cnt = true;
                int pos = 0;
                for (int i = 0; i < password.Length; i++)
                {
                    foreach (char eachChar in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                    {
                        if (password[i] == eachChar) { upp = true; }
                    }

                    foreach (char eachChar in "abcdefghijklmnopqrstuvwxyz")
                    {
                        if (password[i] == eachChar) { low = true; }
                    }
                    foreach (char eachChar in "0123456789")
                    {
                        if (password[i] == eachChar) { num = true; }
                    }
                    foreach (char eachChar in "!@#$%^&*()_-=+/\';><,.>")
                    {
                        if (password[i] == eachChar) { spc = true; }
                    }

                    if (pos < password.Length - 2)
                    {
                        if (password[i] == password[i + 1] && password[i] == password[i + 2])
                        {
                            noRpt = false;
                        }
                        else
                        {
                            pos++;
                        }
                    }

                }
            }
            if (cnt == true && upp == true && low == true && num == true && spc == true && noRpt == true && rng == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async static Task<string> GenerateToken(UserToken UserToken, int expireMinutes = 20)
        {
            Models.ManagerFiles.Settings setting = Models.ManagerFiles.ManagerFilesService.GetSettingsInfo();
            string Secret = setting.Secret;
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", UserToken.Id.ToString()),
                        new Claim("Email", UserToken.Email),
                        new Claim("UserName", UserToken.UserName),                       
                    }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }

        public static bool ValidateToken(string token, out UserToken UserToken)
        {
            UserToken = new UserToken();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return false;
            }

            var a = jwtToken.Claims.GetType();
            var b = jwtToken.Claims.ToArray();
            Dictionary<string, string> dAttributes = new Dictionary<string, string>();
            for (int k = 0; k < b.Count(); k++)
            {
                string type = b[k].Type;
                string value = b[k].Value;
                dAttributes.Add(type, value);

            }
            UserToken.Id = int.Parse(dAttributes["Id"]);
            UserToken.Email = dAttributes["Email"];
            UserToken.UserName = dAttributes["UserName"];
            
            Models.ManagerFiles.Settings setting = Models.ManagerFiles.ManagerFilesService.GetSettingsInfo();
            string Secret = setting.Secret;
            byte[] key = Convert.FromBase64String(Secret);
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            SecurityToken securityToken;
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                Boolean akkk = principal.Identity.IsAuthenticated;
                var clam = principal.Claims;
                // Get the claims values
                var exp = principal.Claims.Where(c => c.Type == "exp")
                                   .Select(c => c.Value).SingleOrDefault();
             
                return akkk;
            }
            catch (Exception)
            {
                return false;
            }


        }

    }
}
