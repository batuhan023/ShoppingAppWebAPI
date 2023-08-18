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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        
        [HttpGet]
        public List<GetOrderDTO> GetOrders()
        {
            List<EntityLayer.Concrete.Order> baskets = _orderService.GetListAll();

            List<GetOrderDTO> orderDTOs = baskets.Select(order => new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                Status = order.Status,
                ShareCode = order.ShareCode,
                CreatedDate = order.CreatedDate,
                TotalPrice = order.TotalPrice
            }).ToList();

            return orderDTOs;
        }

        
        [HttpGet("get")]
        public EntityLayer.Concrete.Order GetOrder(int id)
        {
            var order = _orderService.GetElementById(id);

            if (order == null)
            {
                throw new Exception("NotFound");
            }

            return order;
        }

        
        [HttpPost("addOrder")]
        public async Task<ActionResult<AddOrderDTO>> AddOrder(AddOrderDTO order)
        {
            _orderService.Insert(new EntityLayer.Concrete.Order()
            {
                UserId = order.UserId,  // şimdilik böyle
                AddressId = order.AddressId,
                Status = order.Status,
                ShareCode = RandomString(10),
                CreatedDate = DateTime.Now,
                TotalPrice = 0
            });

            return order;
        }

        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(GetOrderDTO dto)
        {

            if (ModelState.IsValid)
            {
                var orderToUpdate = _orderService.GetElementById(dto.Id);
                if (orderToUpdate == null)
                {
                    return NotFound();
                }

                orderToUpdate.Status = dto.Status;
                //orderToUpdate.AddressId = dto.AddressId;

                _orderService.Update(orderToUpdate);

                return Ok("Address successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }

        
        [HttpDelete("deleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderID)
        {
            var order = _orderService.GetElementById(orderID);
            if (order == null)
            {
                return NotFound();
            }

            _orderService.Delete(order);

            return Ok("Order deleted successfully");
        }
    }
}