using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace WY.Model.Entity
{
    [SugarTable("admin")]
    public class Admin:EntityBase
    {
        [SugarColumn(ColumnDataType="nvarchar(12)")]
        public string Name { get; set; }
        [SugarColumn(ColumnDataType = "nvarchar(16)")]
        public string UserName { get; set; }
        [SugarColumn(ColumnDataType = "nvarchar(64)")]
        public string Pwd { get; set; }
    }
}
