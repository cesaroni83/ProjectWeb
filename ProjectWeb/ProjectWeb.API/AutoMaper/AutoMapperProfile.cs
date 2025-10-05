using AutoMapper;
using ProjectWeb.Shared.Modelo.DTO.Ciudad;
using ProjectWeb.Shared.Modelo.DTO.Pais;
using ProjectWeb.Shared.Modelo.DTO.Provincia;
using ProjectWeb.Shared.Modelo.Entidades;

namespace ProjectWeb.API.AutoMaper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<Pais, PaisDTO>().ReverseMap();
            CreateMap<Pais, PaisDropDTO>().ReverseMap();
            CreateMap<Provincia, ProvinciaDTO>().ReverseMap();
            CreateMap<Provincia, ProvinciaDropDTO>().ReverseMap();
            CreateMap<Ciudad, CiudadDTO>().ReverseMap();
            CreateMap<Ciudad, CiudadDropDTO>().ReverseMap();
            //CreateMap<Menu, MenuDTO>().ReverseMap();
            //CreateMap<Menu, MenuDropDTO>().ReverseMap();
            //CreateMap<Empresa, EmpresaDTO>().ReverseMap();
            //CreateMap<Empresa, EmpresaDropDTO>().ReverseMap();
            //CreateMap<Sucursal, SucursalDTO>().ReverseMap();
            //CreateMap<Sucursal, SucursalDropDTO>().ReverseMap();
            //CreateMap<Persona, PersonaDTO>().ReverseMap();
            //CreateMap<Persona, PersonaDropDTO>().ReverseMap();
        }
    }
}
