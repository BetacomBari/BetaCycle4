using System.ComponentModel.DataAnnotations;

namespace BetaCycle4.Models;

public partial class CustomerNew
{
    [Key]
    public int CustomerId { get; set; }

    public string? NameStyle { get; set; }

    public string? Title { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? Suffix { get; set; }

    public string? CompanyName { get; set; }

    public string? SalesPerson { get; set; }

    public string? EmailAddress { get; set; }

    public string? Phone { get; set; }

    public Guid Rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }


    public int Role { get; set; }





}
