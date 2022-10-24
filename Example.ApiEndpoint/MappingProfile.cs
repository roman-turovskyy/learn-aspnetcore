using AutoMapper;
using Example.ApiEndpoint.DTO;
using Example.Domain.Entities;
using Example.Domain.Enums;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonDTO>();
        //    .ForMember(x => x.Occupation2, options => new PersonOccupation2("qwerty"));
    }
}