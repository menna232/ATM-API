using System.ComponentModel.DataAnnotations;

namespace ATM_Api.Data
{
    public class TransactionInfo
    {
        [Key] 
        public int TransId { get; set; }      

        [Required] 
        public DateTime OperationDate { get; set; }

        [Required] 
        public int SenderId { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be non-negative.")]
        public double Amount { get; set; }

        public bool IsCompleted { get; set; } = false;

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be non-negative.")]
        public double UserBalanceBeforeOperation { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be non-negative.")]
        public double UserBalanceAfterOperation { get; set; }
       
    }

}
