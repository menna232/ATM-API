using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace ATM_Api.Data
{
    public class Customer
    {
        [Key] // Marks userId as the primary key
        public int UserId { get; set; }

        [Required] // Makes username mandatory
        public string Username { get; set; }

        [Required] // Makes password mandatory
        public string Password { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total balance must be non-negative.")]
        public double TotalBalance { get; set; }

        public DateTime BirthDate { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Mail { get; set; }

        public string UserCategory { get; set; } // [VIP, Ordinary]
    }
}
