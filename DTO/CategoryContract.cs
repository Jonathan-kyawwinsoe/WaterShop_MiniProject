using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace water_shop.DTO
{
    public sealed record CategoryRequest(
        [param:Required,MinLength(2),MaxLength(255)]
        [param:Description("The name of the product category")]
        [param:DefaultValue("Brand of Water")]
        string CategoryName
    );
    public sealed record CategoryResponse(
        [property:Description("The unique identifier of the category")]
        [property:DefaultValue(1)]
        int CategoryId,
        [property:Description("The name of the Product Category")]
        [property:DefaultValue("Brand of Water")]
        string CategoryName
        ); 
}
