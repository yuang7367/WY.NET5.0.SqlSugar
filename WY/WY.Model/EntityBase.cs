using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace WY.Model.Entity
{
    public abstract partial class EntityBase
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
    }
}
