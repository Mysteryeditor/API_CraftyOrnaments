using Microsoft.AspNetCore.Mvc;


namespace API_CraftyOrnaments.DAL.Contracts
{
    
    public interface IPayments
    {
       
       public ActionResult<FinalPayment> AddTransaction(FinalPayment finalpayment);
       public ActionResult<FinalPayment> UpdateTransaction(FinalPayment finalpayment);

        public FinalPayment FindTransaction(string? RazorPayId);   
    }
}
