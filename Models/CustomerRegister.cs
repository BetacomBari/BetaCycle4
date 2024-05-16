namespace BetaCycle4.Models
{
    public class CustomerRegister
    {
        
        public int CustomerId { get; set; }

        public bool NameStyle { get; set; } = false;

        public string? Title { get; set; } = "***";

        public string FirstName { get; set; }

        public string? MiddleName { get; set; } = null;

        public string LastName { get; set; } 

        public string? Suffix { get; set; }

        public string? CompanyName { get; set; } = null;

        public string? SalesPerson { get; set; } = null;
        public string? EmailAddress { get; set; }

        public string? Phone { get; set; }
        public string Password { get; set; } 

        public Guid Rowguid { get; set; }

        public DateTime ModifiedDate { get; set; } 

        //AddressType
        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = null!;

        public string StateProvince { get; set; } = null!;

        public string CountryRegion { get; set; } = null!;

        public string PostalCode { get; set; } = null!;


        //CustomerAddress
        public string AddressType { get; set; } = null!;




    }
}
