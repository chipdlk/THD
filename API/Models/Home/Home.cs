using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
namespace API.Models.Home
{
    public class HomeModel
    {
        public List<Articles> ListArticlesNew { get; set; }
    }

    public class HeaderContentModel
    {
        public int IdCoQuan { get; set; }
        public string Title_1 { get; set; }
        public string Title_2 { get; set; }
        public string Title_3 { get; set; }
    }

}
