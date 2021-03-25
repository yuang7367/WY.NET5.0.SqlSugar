using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WY.Model.Entity;

namespace WY.IService
{
    public interface IAdminService
    {
        Task<bool> Create(Admin admin);

        Task<Admin> GetAdminById(int id);

        Task<Admin> GetAdminByUserNameAndPwd(string userNmae, string pwd);
    }
}
