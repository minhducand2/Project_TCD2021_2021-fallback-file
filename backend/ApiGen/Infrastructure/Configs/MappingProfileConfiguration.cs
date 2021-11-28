using AutoMapper;
using ApiGen.Data.Entity;
using ApiGen.DTO;
using ApiGen.DTO.Response;
using ApiGen.DTO.Request;

namespace ApiGen.Infrastructure.Configs
{
    public class MappingProfileConfiguration: Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<E500SinhVien, CreateSinhVienRequest>().ReverseMap();
            CreateMap<E500SinhVien, UpdateSinhVienRequest>().ReverseMap();
            CreateMap<E500SinhVien, SinhVienQueryResponse>().ReverseMap(); 

            
        }
    }
}
