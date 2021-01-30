using System;
using API_MES.Dtos;
using EF_SQL;
using AutoMapper;
namespace API_MES.Profiles
{
    public class SqlProfile : Profile
    {
        public SqlProfile()
        {
            CreateMap<DA_GROSS, DA_GROSSDtos>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_GROSS, DA_GROSSDtos1>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_ELSEGROSS, DA_ELSEGROSSDtos>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_ELSEGROSS, DA_ELSEGROSSDtos1>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_PAYPERSON, DA_PAYPERSONDtos>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_PAYPERSON, DA_PAYPERSONDtos1>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_PAYLOSS, DA_PAYLOSSDtos>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_PAYLOSS, DA_PAYLOSSDtos1>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM-dd")));
            CreateMap<DA_PAYMONTH, DA_PAYMONTHDtos>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM")));
            CreateMap<DA_PAYMONTH, DA_PAYMONTHDtos1>()
                   .ForMember(
                       dest => dest.DT_DATE,
                       opt => opt.MapFrom(src => src.DT_DATE.Value.ToString("yyyy-MM")));
        }
    }
}
