using AutoMapper;
using common.Utilities;
using Entities.User;
using System;
using System.Linq;
using ViewModels.User;

namespace Services.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EditUser, User>()
                .ForMember(dest => dest.UserRoles, opt =>
                    opt.Ignore())
                .ForMember(dest => dest.BirthDate, opt =>
                    opt.MapFrom(src => src.BirthDate.ToMiladiDateTime()))
                .ForMember(dest => dest.LastUpdate, opt =>
                    opt.MapFrom(src => src.LastUpdate.Now()))
                .ForMember(dest => dest.LastVisit, opt =>
                    opt.MapFrom(src => src.LastVisit == null ? default : src.LastVisit))
                .ForMember(dest => dest.LockoutEnd, opt =>
                    opt.MapFrom(src => src.LockoutEnd == null ? (DateTime?)null : src.LockoutEnd.ToMiladiDateTime()));
            //========================================================================================//
            CreateMap<User, EditUser>()
                .ForMember(dest => dest.Roles, opt =>
                    opt.MapFrom(s => s.UserRoles.Select(c => c.Role.Name).ToArray()))
                .ForMember(dest => dest.BirthDate, opt =>
                    opt.MapFrom(src => src.BirthDate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss")))
                .ForMember(dest => dest.LastUpdate, opt =>
                    opt.MapFrom(src => src.LastUpdate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss")))
                .ForMember(dest => dest.LastVisit, opt =>
                    opt.MapFrom(src => (src.LastVisit == null) ? (DateTime?)null : src.LastVisit))
                .ForMember(dest => dest.LockoutEnd, opt =>
                    opt.MapFrom(src => src.LockoutEnd == null ? null : src.LockoutEnd
                        .GetValueOrDefault().DateTime.ToLocalTime().ToShamsiDateTime("yyyy-MM-dd")));
            //========================================================================================//
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Roles, opt =>
                    opt.MapFrom(s => s.UserRoles.Select(c => c.Role.Name).ToArray()))
                .ForMember(dest => dest.BirthDate, opt =>
                    opt.MapFrom(src => src.BirthDate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss")))
                .ForMember(dest => dest.CreateDate, opt =>
                    opt.MapFrom(src => src.CreateDate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss")))
                .ForMember(dest => dest.LastUpdate, opt =>
                    opt.MapFrom(src => src.LastUpdate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss")))
                .ForMember(dest => dest.LastVisit, opt =>
                    opt.MapFrom(src => (src.LastVisit == null) ? (DateTime?)null : src.LastVisit))
                .ForMember(dest => dest.LockoutEnd, opt =>
                    opt.MapFrom(src => src.LockoutEnd == default ? null : src.LockoutEnd
                        .GetValueOrDefault().DateTime.ToLocalTime().ToShamsiDateTime("dddd d MMMM yyyy ساعت HH:mm:ss")));
            //========================================================================================//
            CreateMap<IQueryable<User>, IQueryable<UserViewModel>>();
        }
    }
}