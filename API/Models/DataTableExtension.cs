using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public static class DataTableExtension
    {
        public static IEnumerable<DataRow> AsEnumerable(this DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i].GetType().Name.Equals("String")) {
                    if (table.Rows[i] == null) {
                        
                    }
                }
                yield return table.Rows[i];
            }
        }
    }
}
