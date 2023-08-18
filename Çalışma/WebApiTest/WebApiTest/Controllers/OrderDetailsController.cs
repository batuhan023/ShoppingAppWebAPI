using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Migrations;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        
        [HttpGet]
        public List<GetOrderDetailDTO> GetOrderDetails()
        {
            List<EntityLayer.Concrete.OrderDetail> orderDetails = _orderDetailService.GetListAll();

            List<GetOrderDetailDTO> orderDetailDTOs = orderDetails.Select(orderDetail => new GetOrderDetailDTO
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ItemId = orderDetail.ItemId,
                Amount = orderDetail.Amount,
                UnitPrice = orderDetail.UnitPrice,
                LineTotal = orderDetail.LineTotal,
            }).ToList();

            return orderDetailDTOs;
        }

        
        [HttpGet("get")]
        public OrderDetail GetOrderDetail(int id)
        {
            var orderList = _orderDetailService.GetElementById(id);

            if (orderList == null)
            {
                throw new Exception("NotFound");
            }

            return orderList;
        }

        
        [HttpPost("addOrder")]
        public async Task<ActionResult<AddOrderDetailDTO>> AddOrderDetail(AddOrderDetailDTO orderDetail)
        {
            _orderDetailService.Insert(new OrderDetail()
            {
                OrderId = orderDetail.OrderId,
                ItemId = orderDetail.ItemId,
                Amount = orderDetail.Amount,
                UnitPrice = orderDetail.UnitPrice,
                LineTotal = orderDetail.LineTotal,
            });

            return orderDetail;
        }

        //[HttpPut("update")]
        //public async Task<IActionResult> UpdateOrder(GetOrderDTO dto)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var orderToUpdate = _orderService.GetElementById(dto.Id);
        //        if (orderToUpdate == null)
        //        {
        //            return NotFound();
        //        }


        //        orderToUpdate.AddressId = dto.AddressId;
        //        orderToUpdate.Status = dto.Status;
        //        orderToUpdate.TotalPrice = dto.TotalPrice;

        //        _orderService.Update(orderToUpdate);

        //        return Ok("Address successfully updated");
        //    }
        //    else
        //    {
        //        return BadRequest("Invalid data provided.");
        //    }
        //}

        //
        //[HttpDelete("deleteOrder")]
        //public async Task<IActionResult> DeleteOrder(int orderID)
        //{
        //    var order = _orderService.GetElementById(orderID);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _orderService.Delete(order);

        //    return Ok("Order deleted successfully");
        //}
    }
}