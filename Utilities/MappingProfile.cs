using AutoMapper;
using backend_API.Dto;
using backend_API.Models;
using backend_API.Services;

namespace backend_API.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Proyecto, ProyectoDto>().ReverseMap();
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Ubicacion, UbicacionDto>().ReverseMap();
            CreateMap<Instalacion, InstalacionDto>().ReverseMap();
            CreateMap<Modulo, ModuloDto>().ReverseMap();

        }
    }
}
