using AutoMapper;
using RFIDBLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using RFIDBLL.HelperClasses;
using DocumentFormat.OpenXml.Wordprocessing;
using RFIDDAL.Models;

namespace RFIDBLL.AutoMapperConfig
{
    public class Mapping : Profile
    {
        public Mapping()
        {

            //CreateMap<AssetOdooDTO, UniversityAsset>().ReverseMap()
            //    .ForMember(des => des.);
            //    CreateMap<ItemDTO, Item>().ReverseMap();
            //    CreateMap<BabDTO, Item>().ReverseMap();
            //    CreateMap<BabPricesDto, BabPrice>().ReverseMap();
            //    CreateMap<OrderDTO, Order>().ReverseMap();
            //    CreateMap<OrdersViewDTO, OrdersAllDataView>().ReverseMap()
            //        .ForMember(des => des.ClientPhone, src => src.MapFrom(s=> s.ClientPhoneCode.Trim() + "-" + s.ClientPhone))
            //        .ForMember(des => des.CreationDate, src => src.MapFrom(s=> s.CreationDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
            //        .ForMember(des => des.DeliveryDate, src => src.MapFrom(s=> s.DeliveryDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt")));
            //    CreateMap<OrderDTOWithClient, Order>().ReverseMap();
            //    CreateMap<AddOrderDetailsDTO, OrderDetail>().ReverseMap();
            //    CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();
            //    CreateMap<OrderDetailDTO, OrderDetailsAllDataView>().ReverseMap()
            //        .ForMember(des => des.NotAssignedCount, src=> src.MapFrom(s=> s.NotAssignedCount != null ? s.NotAssignedCount : s.Quantity));
            //    CreateMap<TechnicianOrderDetailDTO, OrderDetalsTechniciansView>().ReverseMap();
            //    CreateMap<PagedResult<OrderDTO>, PagedResult<Order>>().ReverseMap();
            //    CreateMap<PagedResult<OrdersViewDTO>, PagedResult<OrdersAllDataView>>().ReverseMap();

            //    CreateMap<Requests, SiruSolutionsRequestDTO>().ReverseMap()
            //      .ForMember(des => des.RequestTypeId, src => src.MapFrom(s => 7))
            //      .ForMember(des => des.CreationDate, src => src.MapFrom(s => DateTime.Now));

            //    CreateMap<AspNetUser, UserDTO>().ReverseMap();


            //    //CreateMap<OrderDTOWithClient, OrderDTOWithGroup>()
            //    //.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => MapOrderDetails(src.OrderDetails)));

            //    //CreateMap<OrderDTOWithClient, OrderDTOWithGroup>()
            //    // .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => MapOrderDetails(src.OrderDetails)));

            //    //CreateMap<AddOrderDetailsDTO, OrderDetailsDTO>().ReverseMap();

            //    //// Ensure correct mapping for the nested conversion
            //    ////CreateMap<List<AddOrderDetailsDTO>, List<OrderDetailsDTO>>().ReverseMap();

            //}
            ////private List<FatoorahOrderDetailsDTO> MapOrderDetails(List<AddOrderDetailsDTO> orderDetails)
            ////{
            ////    if (orderDetails == null) return null;

            ////    var groupedDetails = orderDetails
            ////        .GroupBy(detail => detail.OrderId)
            ////        .Select(group => new FatoorahOrderDetailsDTO
            ////        {
            ////            Key = group.Key,
            ////            OrderDetailsDTOs = group.Select(detail => new OrderDetailsDTO
            ////            {
            ////                OrderDetailsId = detail.OrderDetailsId,
            ////                OrderId = detail.OrderId,
            ////                ItemId = detail.ItemId,
            ////                ColorId = detail.ColorId,
            ////                ModelId = detail.ModelId,
            ////                FloorId = detail.FloorId,
            ////                Quantity = detail.Quantity,
            ////                DoorHight = detail.DoorHight,
            ////                DoorWidth = detail.DoorWidth,
            ////                Price = detail.Price,
            ////                ModelPrice = detail.ModelPrice,
            ////                DoorHingePrice = detail.DoorHingePrice,
            ////                DoorHingeCount = detail.DoorHingeCount,
            ////                DoorHingeDirection = detail.DoorHingeDirection,
            ////                Notes = detail.Notes
            ////            }).ToList()
            ////        }).ToList();

            ////    return groupedDetails;
            ////}
        }
    }
}
