using API_CraftyOrnaments.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;


namespace API_CraftyOrnaments.DAL.Repository
{
    public class PaymentsRepository:IPayments

    {
        private readonly CraftyOrnamentsContext _context;
        public PaymentsRepository(CraftyOrnamentsContext context)
        {

            _context = context;

        }

        public FinalPayment FindTransaction(string? RazorPayId)
        {
            FinalPayment? chosenTransaction=_context.FinalPayments.Where(transactions=>transactions.RazorpayOrderId == RazorPayId).FirstOrDefault();
            if(chosenTransaction is not null)
            {
                return chosenTransaction;
            }
            else
            {
                return null;
            }
        }

        ActionResult<FinalPayment> IPayments.AddTransaction(FinalPayment finalpayment)
        {
            _context.FinalPayments.Add(finalpayment);
            _context.SaveChanges();
            return null;

        }

        ActionResult<FinalPayment> IPayments.UpdateTransaction(FinalPayment finalpayment)
        {
            _context.Entry(finalpayment).State=EntityState.Modified;
            _context.SaveChanges();
            return finalpayment;
        }
    }
}
