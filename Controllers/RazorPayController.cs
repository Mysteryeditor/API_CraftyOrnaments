using API_CraftyOrnaments.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Razorpay.Api;
using System.Text;

namespace API_CraftyOrnaments.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RazorPayController : ControllerBase
    {
        private readonly RazorpayClient _razorpayClient;
        private readonly CraftyOrnamentsContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPayments _paymentsService;
        public RazorPayController(CraftyOrnamentsContext context, IHttpContextAccessor contextAccessor, IPayments payments)
        {
            _razorpayClient = new RazorpayClient("rzp_test_xPSgWk2BqWWbX5",
                "drfZAYMtn4LooQhfnGe5LLPr");
            _context = context;
            _contextAccessor = contextAccessor;
            _paymentsService = payments;
        }

        [HttpPost]
        [Route("initialize")]
        public async Task<IActionResult> InitializePayment()
        {
            try
            {
                using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    dynamic? requestData = JsonConvert.DeserializeObject<dynamic>(requestBody);
                    if (requestData is not null && requestData.orderId != null)
                    {
                        decimal? finalremainingAmount = requestData.remainingamount;

                        var finalOptions = new Dictionary<string, object>
                        {
                            { "amount", finalremainingAmount * 100 },
                            { "currency", "INR" },
                            { "payment_capture", true }
                        };

                        try
                        {
                            var finalPayment = _razorpayClient.Order.Create(finalOptions);
                            var finalorderId = finalPayment["id"].ToString();
                            var orderJson = finalPayment.Attributes.ToString();
                            FinalPayment payment = new FinalPayment
                            {
                                OrderId = requestData.orderId,
                                RazorpayOrderId = finalorderId,
                                AmountPaid = finalremainingAmount,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                IsSuccess = false,
                                UserId = requestData.userId,
                            };
                            _paymentsService.AddTransaction(payment);
                            return Ok(orderJson);
                        }

                        catch { }
                    }


                    decimal advanceAmount = Convert.ToDecimal(requestData.advanceAmount);
                    decimal remainingAmount = Convert.ToDecimal(requestData.remainingAmount);
                    int weight = Convert.ToInt32(requestData.weight);


                    var options = new Dictionary<string, object> {
                        { "amount", advanceAmount*100},
                        { "currency", "INR" },
                        { "payment_capture", true }
                                                                     };


                    try
                    {
                        var order = _razorpayClient.Order.Create(options);
                        var orderId = order["id"].ToString();
                        OrderDetail orderDetail = new()
                        {
                            GeneratedId = orderId,
                            OrnamentId = requestData.ornamentId,
                            MetalId = requestData.metalId,
                            UserId = requestData.userId,
                            Weight = requestData.weight,
                            Quantity = requestData.quantity,
                            CreatedDate = DateTime.Now,
                            DueDate = DateTime.Now.AddDays((weight) * 5),
                            Length = requestData.length,
                            Width = requestData.width,
                            Size = requestData.sizeId,
                            AdvanceAmount = advanceAmount,
                            RemainingAmount = remainingAmount,
                            TotalAmount = requestData.estimatedAmount,
                            Finalamount = 0,
                            IsCustomized = false,
                            Image = null,
                            AdvancePaid = false,
                            FullAmountPaid = false,
                            StatusId = 1
                        };

                        var orderJson = order.Attributes.ToString();
                        _context.OrderDetails.Add(orderDetail);
                        _context.SaveChanges();

                        return Ok(orderJson);
                    }
                    catch (Exception ex)
                    {
                        string a = ex.Message;
                        return BadRequest(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }




        public class ConfirmPaymentPayload
        {
            public string? razorpay_payment_id { get; set; }
            public string razorpay_order_id { get; set; }
            public string razorpay_signature { get; set; }

        }

        [HttpPost]
        [Route("confirm")]
        public IActionResult ConfirmPayment(ConfirmPaymentPayload confirmPayment)
        {
            var attributes = new Dictionary<string, string>
            {
                { "razorpay_payment_id", confirmPayment.razorpay_payment_id },
                { "razorpay_order_id", confirmPayment.razorpay_order_id },
                { "razorpay_signature", confirmPayment.razorpay_signature }
            };
            try
            {
                Utils.verifyPaymentSignature(attributes);
                // OR
                var isValid = Utils.ValidatePaymentSignature(attributes);
                if (isValid)
                {
                    var order = _razorpayClient.Order.Fetch(confirmPayment.razorpay_order_id);
                    var payment = _razorpayClient.Payment.Fetch(confirmPayment.razorpay_payment_id);
                    if (payment["status"] == "captured")
                    {
                        OrderDetail? currentOrder = _context.OrderDetails.FirstOrDefault(x => x.GeneratedId == confirmPayment.razorpay_order_id);
                        if (currentOrder != null)
                        {
                            currentOrder.StatusId = 2;
                            currentOrder.AdvancePaid = true;
                            currentOrder.RemainingAmount = currentOrder.TotalAmount - currentOrder.AdvanceAmount;
                            OrderDetailsController putOrder = new(_context);
                            putOrder.PutOrderDetail(currentOrder.OrderId, currentOrder);
                            return Ok();
                        }
                        else
                        {
                            OrderDetailsController deleteOrder = new(_context);
                            if (currentOrder is not null)
                            {
                                deleteOrder.DeleteOrderDetail(currentOrder.OrderId);
                            }
                            return BadRequest("Payment Failed");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public IActionResult FinalPayment(ConfirmPaymentPayload confirmPayment)
        {

            var attributes = new Dictionary<string, string>
            {
                { "razorpay_payment_id", confirmPayment.razorpay_payment_id },
                { "razorpay_order_id", confirmPayment.razorpay_order_id },
                { "razorpay_signature", confirmPayment.razorpay_signature }
            };

            try
            {
                Utils.verifyPaymentSignature(attributes);
                // OR
                var isValid = Utils.ValidatePaymentSignature(attributes);
                if (isValid)
                {
                    var order = _razorpayClient.Order.Fetch(confirmPayment.razorpay_order_id);
                    var payment = _razorpayClient.Payment.Fetch(confirmPayment.razorpay_payment_id);
                    if (payment["status"] == "captured")
                    {
                        FinalPayment existingTransaction = _paymentsService.FindTransaction(confirmPayment.razorpay_order_id);
                        existingTransaction.IsSuccess=true;
                       var result= _paymentsService.UpdateTransaction(existingTransaction);
                        
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch
            {
                return BadRequest();
            }

            return BadRequest();

        }



    }
}


