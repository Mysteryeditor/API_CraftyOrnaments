using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_CraftyOrnaments.Models;
using Microsoft.Identity.Client;

namespace API_CraftyOrnaments.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly CraftyOrnamentsContext _context;

        public OrderDetailsController(CraftyOrnamentsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// All The Order Details Are Printed
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<OrderInformation>> GetOrderDetails()
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }
            List<OrderInformation> userOrders = _context.OrderInformations.ToList();
            return userOrders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            var userOrder = await _context.OrderDetails.FindAsync(id);

            if (userOrder == null)
            {
                return null;
            }
            else
            {
                return userOrder;
            }
        }
        [HttpPut]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail userOrder)
        {


            _context.Entry(userOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);

            }

            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateOrderStatus(int id, OrderInformation updatedOrder)
        {
            if (updatedOrder is not null)
            {
                OrderDetail? order = _context.OrderDetails.Find(updatedOrder.OrderId);
                if (order is not null)
                {
                    order.StatusId = updatedOrder.StatusId;
                    if (updatedOrder.StatusId == 4)
                    {
                        order.RemainingAmount = updatedOrder.Finalamount - updatedOrder.Advanceamount;
                        order.Finalamount = updatedOrder.Finalamount;
                        order.FinalWeight = updatedOrder.FinalWeight;
                    }

                    var ok = this.PutOrderDetail(id, order);
                }
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail userOrder)
        {
            _context.OrderDetails.Add(userOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDetail", new { id = userOrder.OrderId }, userOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }
            var userOrder = await _context.OrderDetails.FindAsync(id);
            if (userOrder == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(userOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderInformation>>> GetUserOrder(int userId)
        {
            var userOrder = await _context.OrderInformations.Where(order => order.UserId == userId).ToListAsync();



            if (userOrder == null)
            {
                return NotFound();
            }

            return userOrder;
        }



        [HttpGet]
        public ActionResult<IEnumerable<OrderStatus>> GetStatus()
        {
            var orderStatus = _context.OrderStatuses.Where(x => x.StatusId != 1).OrderBy(status => status.StatusId).ToList();
            return orderStatus;
        }

        [HttpGet]
        public List<OrderStatsModel> GetOrderStats()
        {
                var metalOrderCounts = _context.OrderInformations.GroupBy(o => o.MetalName).
                    Select(g => new OrderStatsModel
                    {
                        metalName = g.Key,
                        orderCount = g.Count()
                    }).ToList();

            return metalOrderCounts;
        }

        private bool OrderDetailExists(int id)
        {
            return (_context.OrderDetails?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
