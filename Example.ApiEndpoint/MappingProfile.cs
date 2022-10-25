using AutoMapper;
using Example.ApiEndpoint.DTO;
using Example.Domain.Entities;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonDTO>();
    }
}