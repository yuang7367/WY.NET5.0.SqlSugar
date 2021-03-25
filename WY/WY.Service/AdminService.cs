using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WY.Data;
using WY.IService;
using WY.Model.Entity;

namespace WY.Service
{
    public class AdminService : IAdminService
    {
        private readonly IBaseRepository<Admin> _baseAdminRepository;
        public AdminService(IBaseRepository<Admin> baseAdminRepository)
        {
            _baseAdminRepository = baseAdminRepository;
        }

        public async Task<bool> Create(Admin admin)
        {
            return await _baseAdminRepository.CrateAsync(admin);
        }

        public async Task<Admin> GetAdminById(int id)
        {
            return await _baseAdminRepository.GetAsync(id);
        }

        public async Task<Admin> GetAdminByUserNameAndPwd(string userNmae, string pwd)
        {
            return await _baseAdminRepository.GetAsync(x => x.UserName == userNmae && x.Pwd == pwd);
        }
    }
}
