using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WY.Model.DTO;
using WY.Model.Entity;

namespace WY.Data.Utility._AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            base.CreateMap<Admin, AdminDto>();
            base.CreateMap<AdminDto, Admin>();

            base.CreateMap<Student, StudentDto>();
            base.CreateMap<StudentDto, Student>()
                .ForMember(model=>model.StudentNumber,options=>options.Ignore());
        }
    }
}
