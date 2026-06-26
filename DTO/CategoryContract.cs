using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace water_shop.DTO
{
    public sealed record CreateCategoryRequest(
        [param:Required,MinLength(2),MaxLength(255)]
        [param:Description("The name of category")]
        [param:DefaultValue("water")]
        string CategoryName
        );
    public sealed record CreateCategoryResponse(
        [property:Description("The Unique identifier of the category")]
        [property:DefaultValue(1)]
        int CategoryId,

        [property:Description("The name of category")]
        [property:DefaultValue("water")]
        string CategoryName,

        [property:Description("The date and time category was create")]
        DateTime CreateDate
        );
    public sealed record GetCategoryResponse(
        [property:Description("The unique identifier of the category")]
        [property:DefaultValue(1)]
        int CategoryId,

        [property:Description("The name of category")]
        [property:DefaultValue("water")]
        string CategoryName,

        [property:Description("The date and time category was create")]
        DateTime CreatedAt,

        [property:Description("The date and time category last update at time")]
        DateTime UpdatedAt
        );
    public sealed record UpdateCategoryRequest(
        [param:Required,MinLength(2),MaxLength(255)]
        [param:Description("The name of category")]
        [param:DefaultValue("Water")]
        string CategoryName
        );
    public sealed record UpdateCategoryResponse(
        [property:Description("The unique identifier of the category")]
        [property:DefaultValue(1)]
        int id,

        [property:Description("The name of the category")]
        [property:DefaultValue("water")]
        string CategoryName,

        [property:Description("The date and time  category last update at time")]
        DateTime UpDatedAt
        );
    public sealed record DeleteCategoryResponse(
        [property:Description("Delete success message")]
        [property:DefaultValue("Category successfully deleted")]
        string Message
        );
}
