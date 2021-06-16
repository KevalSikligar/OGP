using AutoMapper;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Data.DbModel;
using OGP_Portal.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OGP_Portal.AutoMapperProfileConfiguration
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {

            //CreateMap<dbmodel, dto>().ReverseMap();
            //CreateMap<CompanyMasterDto,CompanyMaster>().ReverseMap()
            //.Ignore(x=>x.Password)
            //.Ignore(x=>x.FirstName)
            //.Ignore(x=>x.LastName);
            CreateMap<BUserDto, ApplicationUser>()
                .ForMember(src=>src.FirstName,opt=>opt.MapFrom(src=>src.Name))
                .ReverseMap();

        }

    }

    public static class MappingExpression
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }


}
