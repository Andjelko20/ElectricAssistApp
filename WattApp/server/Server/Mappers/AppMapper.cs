using AutoMapper;
using Microsoft.AspNetCore.Components;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Services;
using Server.Services.Impl;
using System.Security.Cryptography.X509Certificates;

namespace Server.Mappers
{
    public class AppMapper : Profile
    {
        /*[Inject]
        public DeviceCategoryService deviceCategoryService { get; set; }
        [Inject]
        public DeviceTypeService deviceTypeService { get; set; }
        [Inject]
        public DeviceBrandService deviceBrandService { get; set; }
        [Inject]
        public DeviceModelService deviceModelService { get;  set; }*/

        public AppMapper()
        {
            CreateMap<Device, DeviceResponseDTO>()
                        .ForMember(dest => dest.DeviceCategory, opt => opt.MapFrom(src => src.DeviceCategoryId))
                        .ForMember(dest => dest.DeviceType, opt => opt.MapFrom(src => src.DeviceTypeId))
                        .ForMember(dest => dest.DeviceBrand, opt => opt.MapFrom(src => src.DeviceBrandId))
                        .ForMember(dest => dest.DeviceModel, opt => opt.MapFrom(src => src.DeviceModelId));

            /*CreateMap<Device, DeviceResponseDTO>()
                .ForMember(dest => dest.DeviceCategory, opt => opt.MapFrom(src => deviceCategoryService.getCategoryNameById(src.DeviceCategoryId)))
                .ForMember(dest => dest.DeviceType, opt => opt.MapFrom(src => deviceTypeService.getTypeNameById(src.DeviceTypeId)))
                .ForMember(dest => dest.DeviceBrand, opt => opt.MapFrom(src => deviceBrandService.getBrandNameById(src.DeviceBrandId)))
                .ForMember(dest => dest.DeviceModel, opt => opt.MapFrom(src => deviceModelService.getModelNameById(src.DeviceModelId)));*/
        }
    }
}
