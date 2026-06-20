using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace water_shop.DTO
{
    public sealed record CustomerRequest(
        [param: Required, MinLength(2), MaxLength(100)]
        [param: Description("The full name of the customer")]
        [param: DefaultValue("Ma Ma")]
        string CustomerName,

        [param: Required, MinLength(5), MaxLength(20)]
        [param: Description("The primary phone number of the customer")]
        [param: DefaultValue("09123456789")]
        string Phone,

        [param: Required, MinLength(5), MaxLength(255)]
        [param: Description("The detailed delivery address of the customer")]
        [param: DefaultValue("Yangon")]
        string Address
    );
    public sealed record CustomerResponse(
        [property: Description("The unique identifier of the customer")]
        [property: DefaultValue(1)]
        int CustomerId,

        [property: Description("The full name of the customer")]
        [property: DefaultValue("Ma Ma")]
        string CustomerName,

        [property: Description("The primary phone number of the customer")]
        [property: DefaultValue("09123456789")]
        string Phone,

        [property: Description("The detailed delivery address of the customer")]
        [property: DefaultValue("Yangon")]
        string Address,

        [property: Description("The total number of empty bottles this customer currently owes")]
        [property: DefaultValue(3)]
        int EmptyBottleOwed
);

}
