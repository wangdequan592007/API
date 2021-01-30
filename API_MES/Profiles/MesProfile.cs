using System;
using API_MES.Dtos;
using AutoMapper;
using EF_ORACLE.Model;
using EF_ORACLE.Model.TMES;

namespace API_MES.Profiles
{
    public class MesProfile:Profile
    {
        public MesProfile()
        {
            CreateMap<FQA_FIRSTARTICLELINE, FQA_FIRSTARTICLELINELOG>();
            CreateMap<FQA_FIRSTARTICLEDtos, FQA_FIRSTARTICLE>();
            CreateMap<ModelMO, ModelDot>()
                .ForMember(
                    dest => dest.INPUTDATE,
                    opt => opt.MapFrom(src =>src.INPUTDATE.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<FQA_MIDTIME, FQA_MIDTIMEDto>()
                .ForMember(
                    dest => dest.INPUTDATE,
                    opt => opt.MapFrom(src => src.INPUTDATE.Value.ToString("yyyy-MM-dd HH:mm:ss")));
            CreateMap<ODM_IMEILINKNETCODE, ODM_IMEILINKNETCODEDtos>()
                .ForMember(
                    dest => dest.PHYSICSNO,
                    opt => opt.MapFrom(src => src.PHYSICSNO.Substring(0,8)));
            CreateMap<ODM_IMEILINKNETCODE, IMEILNTOBARCODEDtos>()
                .ForMember(
                    dest => dest.BARCODE,
                    opt => opt.MapFrom(src => src.SN))
                .ForMember(
                    dest => dest.WORKCODE,
                    opt => opt.MapFrom(src => src.WORKORDER));
            CreateMap<WORKPRODUCE, LineDtos>()
                .ForMember(
                    dest => dest.ID,
                    opt => opt.MapFrom(src => src.LINE))
                .ForMember(
                    dest => dest.NAME,
                    opt => opt.MapFrom(src => src.LINE));
            CreateMap<FQA_CK_SNLOG, FQA_CK_SNLOGDtos>().ForMember(
                    dest => dest.ISOK,
                    opt => opt.MapFrom(src =>src.ISOK==1? "已检验	" : "未检验"));
            CreateMap<ODM_BEATE, ODM_BEATEDtos>()
                .ForMember(
                    dest => dest.ID,
                    opt => opt.MapFrom(src => src.LINECODE))
                .ForMember(
                    dest => dest.NAME,
                    opt => opt.MapFrom(src => src.LINECODE));

        }
    }
}
