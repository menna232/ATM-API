using System.ComponentModel.DataAnnotations;

namespace ATM_Api.Data
{
    public class Operations
    {
        private readonly ApplicationDbContext _context;
        [Key]
        public int operationId { get; set; }    
        private Customer Customer;

        public Operations(ApplicationDbContext context)
        {
            _context = context;
        }

        // Associate the customer
        public void SetCustomer(int customerId)
        {
            Customer = _context.customers.FirstOrDefault(c => c.UserId == customerId);
            if (Customer == null)
                throw new Exception("Customer not found.");
        }

        // Deposit money
        public void Deposit(double amount)
        {
            if (Customer == null) throw new Exception("Customer is not set.");

            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            Customer.TotalBalance += amount;

            // Add a transaction record
            var transaction = new TransactionInfo
            {
                SenderId = Customer.UserId,
                SenderName = Customer.Username,
                Amount = amount,
                UserBalanceBeforeOperation = Customer.TotalBalance - amount,
                UserBalanceAfterOperation = Customer.TotalBalance,
                OperationDate = DateTime.Now,         
                ReceiverName = "Self",            
                IsCompleted = true
            };

            _context.transactions.Add(transaction);
            _context.SaveChanges();
        }

        //Withdraw money
        public void Withdrawal(double amount)
        {
            if (Customer == null) throw new Exception("Customer is not set.");

            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");

            if (Customer.TotalBalance < amount)
                throw new InvalidOperationException("Insufficient balance.");

            Customer.TotalBalance -= amount;

            // Add a transaction record
            var transaction = new TransactionInfo
            {
                OperationDate = DateTime.Now,
                SenderId = Customer.UserId,
                SenderName = Customer.Username,
                ReceiverName = "Self",
                Amount = -amount,
                UserBalanceBeforeOperation = Customer.TotalBalance + amount,
                UserBalanceAfterOperation = Customer.TotalBalance,
                IsCompleted = true
            };

            _context.transactions.Add(transaction);
            _context.SaveChanges();
        }

        // Balance inquiry
        public double BalanceInquiry()
        {
            if (Customer == null) throw new Exception("Customer is not set.");
            return Customer.TotalBalance;
        }

        // Send Money
        public void SendMoney(string receiverName, double amount)
        {
            if (Customer == null) throw new Exception("Customer is not set.");
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");

            var receiver = _context.customers.FirstOrDefault(c => c.Username == receiverName);
            if (receiver == null) throw new Exception("Receiver not found.");

            if (Customer.TotalBalance < amount)
                throw new InvalidOperationException("Insufficient balance.");

            // Deduct from sender
            var senderBeforeBalance = Customer.TotalBalance;
            Customer.TotalBalance -= amount;

            // Credit to receiver
            var receiverBeforeBalance = receiver.TotalBalance;
            receiver.TotalBalance += amount;

            // Create transaction records
            var transaction = new TransactionInfo
            {
                SenderId = Customer.UserId,
                SenderName = Customer.Username,
                ReceiverName = receiver.Username,
                Amount = amount,
                UserBalanceBeforeOperation = senderBeforeBalance,
                UserBalanceAfterOperation = Customer.TotalBalance,
                OperationDate = DateTime.Now,
                IsCompleted = true
            };

            _context.transactions.Add(transaction);
            _context.SaveChanges();
        }

        // Receive Money
        public string ReceiveMoney()
        {
            if (Customer == null) throw new Exception("Customer is not set.");

            var pending = _context.transactions
                .Where(t => t.ReceiverName == Customer.Username && !t.IsCompleted)
                .FirstOrDefault();

            if (pending != null)
            {
                var beforeBalance = Customer.TotalBalance;
                Customer.TotalBalance += pending.Amount;
                pending.IsCompleted = true;

                // Update balances and transaction status
                _context.transactions.Update(pending);
                _context.customers.Update(Customer);
                _context.SaveChanges();

                return $"You have received {pending.Amount} from {pending.SenderName}. " +
                       $"Your new balance is {Customer.TotalBalance}.";
            }
            else
            {
                throw new Exception("No pending transactions found for you.");
            }
        }

        // Display Pending Transactions
        public List<TransactionInfo> DisplayPendingTransactions()
        {
            if (Customer == null) throw new Exception("Customer is not set.");

            return _context.transactions
                .Where(t => t.ReceiverName == Customer.Username && !t.IsCompleted)
                .ToList();
        }

        // Display Completed Transactions
        public List<TransactionInfo> DisplayMadeTransactions()
        {
            if (Customer == null) throw new Exception("Customer is not set.");

            return _context.transactions
                .Where(t => t.SenderId == Customer.UserId && t.IsCompleted)
                .ToList();
        }


    }

}
