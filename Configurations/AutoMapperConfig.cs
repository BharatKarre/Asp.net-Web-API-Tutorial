using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Dtos;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap<Students, StudentDTO>();
            //CreateMap<StudentDTO, Students>();

            //instead of writting Two line we can call reverse mapping
            //If the Properties of both the Source and destination object matches
            CreateMap<Students, StudentDTO>().ReverseMap();

            //If the Properties of both the Source and destination object does not matches Also the position matter whether to put before or after reverseMap
            //CreateMap<Students, StudentDTO>().ForMember(n => n.Name, o => o.MapFrom(x => x.Name)).ReverseMap();

            //If we do not want to map the property from source to destiantion object we can simply use ignore mapping
            //CreateMap<Students, StudentDTO>().ForMember(n => n.Name, o => o.Ignore()).ReverseMap();

            //If we want to send some label instead of data of the property of the object we can use transforming 
            //CreateMap<Students, StudentDTO>().ReverseMap().AddTransform<string>(n => string.IsNullOrEmpty(n) ? "No Data Found." : n);

            //However the below transformation will be applied for every propery of the object, So if you want to apply it on specific property.
            //CreateMap<Students, StudentDTO>().ReverseMap().ForMember(n => n.Phone, o => o.MapFrom(x => string.IsNullOrEmpty(x.Phone) ? "No Data Found." : x.Phone));

        }
    }
}
