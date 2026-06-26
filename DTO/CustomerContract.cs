using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using water_shop.Entity;

namespace water_shop.DTO
{
    public sealed record CreateCustomerRequest(
        [param:Required, MinLength(2),MaxLength(255)]
        [param:Description("The full name of the customer")]
        [param:DefaultValue("mg mg")]
        string CustomerName,

        [param:Required,MinLength(5),MaxLength(20)]
        [param:Description("Customer phone number")]
        [param:DefaultValue("09123456789")]
        string Phone,

        [param:Required,MinLength(2),MaxLength(255)]
        [param:Description("customer full address")]
        [param:DefaultValue("Hlaing Townshi, Yangon")]
        string Address
        );
    public sealed record CreateCustomerResponse(
        [property:Description("Customer Id")]
        [property:DefaultValue(1)]
        int CustomerId,

        [property:Description("The full name of customer")]
        [property:DefaultValue("mg mg")]
        string CustomerName,

        [property:Description("Customer phone number")]
        [property:DefaultValue("09123456789")]
        string Phone,

        [property:Description("Customer Address")]
        [property:DefaultValue("Hlaing Township, Yangon")]
        string Address
        );
    public sealed record GetCustomerResponse(
        [property:Description("The unque identifier of the customer")]
        [property:DefaultValue(1)]
        int CustomerId,

        [property:Description("the full name of the customer")]
        [property:DefaultValue("mg mg")]
        string CustomerName,

        [property:Description("customer Phone number")]
        [property:DefaultValue("09123456789")]
        string phone,

        [property:Description("the detail deliver address of customer")]
        [property:DefaultValue("Hlaing township, Yangon")]
        string address,

        [property:Description("The total number of empty bottle this customer current own")]
        [property:DefaultValue(3)]
        int EmptyBottle
        
        );
    public sealed record UpdateCustomerRequest(
        [param:Required, MinLength(2), MaxLength(255)]
        [param:Description("the full name of customer")]
        [param:DefaultValue("mg mg")]
        string CustomerName,

        [param:Required, MinLength(5),MaxLength(20)]
        [param:Description("That is customer phone number")]
        [param:DefaultValue("09123456789")]
        string phoneNumber,

        [param:Required,MinLength(2),MaxLength(255)]
        [param:Description("the detail deliver address of customer")]
        [param:DefaultValue("Hlaing township, Yango")]
        string Address
        );
    public sealed record UpdateCustomerResponse(
        [property:Description("this unique of customer ID")]
        [property:DefaultValue(1)]
        int CustomerId,

        [property:Description("The full of customer")]
        [property:DefaultValue("mg mg")]
        string CustomerName,

        [property:Description("the customer of phoneNumber")]
        [property:DefaultValue("09123456789")]
        string Phone,

        [property:Description("the detail deliver address of customer")]
        [property:DefaultValue("haling township , yangon")]
        string Addrress
        );
    public sealed record DeleteCustomerResponse(
        [property: Description("Delete success message")]
        [property: DefaultValue("Customer deleted successfully.")]
        string Message
        );

}
