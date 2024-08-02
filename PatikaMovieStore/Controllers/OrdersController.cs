using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.BusinessLayer.Concrete;
using PatikaMovieStore.DtoLayer.OrderDtos;
using PatikaMovieStore.DtoLayer.PurchaseDtos;
using PatikaMovieStore.EntityLayer.Entities;
using System.Security.Claims;

namespace PatikaMovieStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMovieService _movieService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(IOrderService orderService, IMovieService movieService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _movieService = movieService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("PurchaseMovie")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PurchaseMovie([FromBody] PurchaseDto purchaseDto)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Customer")
                return Forbid("Only customers can purchase movies");
            
            var customerId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var movie = await _movieService.GetMovieByIdAsync(purchaseDto.MovieId);
            if (movie == null)
                return NotFound("Movie not found");

            var order = new CreateOrderDto
            {
                CustomerId = customerId,
                MovieId = movie.Id,
                Price = movie.Price,
                PurchaseDate = DateTime.UtcNow
            };

            await _orderService.CreateOrderAsync(order);

            return Ok("Movie purchased successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            var orders = await _orderService.GetAllOrderAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            await _orderService.CreateOrderAsync(createOrderDto);
            return Ok("Order created successfully !");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            await _orderService.UpdateOrderAsync(updateOrderDto);
            return Ok("Order updated successfully !");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok("Order deleted successfully !");
        }
    }
}
