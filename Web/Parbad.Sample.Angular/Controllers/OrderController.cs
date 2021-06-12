using Microsoft.AspNetCore.Mvc;
using Parbad.Sample.Angular.Repositories;

namespace Parbad.Sample.Angular.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("{id}")]
        public IActionResult Result([FromRoute] int id)
        {
            var order = _orderRepository.GetOrderById(id);

            return Ok(order);
        }
    }
}
