using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WY.Data;
using WY.Data.Utility.Caching;
using WY.IService;
using WY.Model.Entity;

namespace WY.Service
{
    public class StudentService : IStudentService
    {
        private readonly IBaseRepository<Student> _baseStudentRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICacheManager _cacheManager;
        public StudentService(IBaseRepository<Student> baseStudentRepository,
                              ICacheManager cacheManager,
                              IStaticCacheManager staticCacheManager)
        {
            _baseStudentRepository = baseStudentRepository;
            _staticCacheManager = staticCacheManager;
            _cacheManager = cacheManager;
        }

        public async Task<bool> CreateStudent(Student student)
        {
            return await _baseStudentRepository.CrateAsync(student);
        }

        public async Task<Student> GetStudentById(int id)
        {
            if (id == 0)
                return null;

            var key = string.Format(CatalogDefaults.ProductsByIdCacheKey, id);
            
            //_cacheManager.Set(key, "TestCacheSet001");


            return await _cacheManager.Get(key, () => _baseStudentRepository.GetAsync(id));
        }
    }
}
