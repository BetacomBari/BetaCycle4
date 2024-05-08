namespace BetaCycle4.Models
{
    public class CustomerRegister
    {
        public int CustomerId { get; set; }

        public bool NameStyle { get; set; }

        public string? Title { get; set; }

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string? Suffix { get; set; }

        public string? CompanyName { get; set; }

        public string? SalesPerson { get; set; }

        public string? Phone { get; set; }

        public Guid Rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int Role { get; set; }


        //------------------------------------


        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int CredentialsCnnId { get; set; }
    }
}
