using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace WY.Model.Entity
{
    [SugarTable("student")]
    public class Student:EntityBase
    {
        public string Name { get; set; }

        public string StudentNumber { get; set; }

        public int Gener { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }
    }
}
