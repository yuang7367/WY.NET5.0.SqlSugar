using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WY.Model.Entity;

namespace WY.IService
{
    public interface IStudentService
    {
        Task<bool> CreateStudent(Student student);

        Task<Student> GetStudentById(int id);
    }
}
