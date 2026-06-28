using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace water_shop.DTO
{
    public sealed record CreateProductRequest(
        [param: Required, MinLength(2), MaxLength(100)]
        [param: Description("The name of the product")]
        [param: DefaultValue("Drinking Water 19L")]
        string ProductName,

        [param: Description("The category ID of the product")]
        [param: DefaultValue(1)]
        int? CategoryId,

        [param: Required]
        [param: Description("The price per unit")]
        decimal UnitsPrice,

        [param: Required]
        [param: Description("The stock quantity")]
        [param: DefaultValue(100)]
        int SockQuantity,

        [param: Required]
        [param: Description("Is the product returnable")]
        [param: DefaultValue(true)]
        bool IsReturnAble
    );

    public sealed record CreateProductResponse(
        [property: DefaultValue(1)]
        int Id,

        [property: DefaultValue("Drinking Water 19L")]
        string ProductName,

        [property: DefaultValue(1)]
        int? CategoryId,

        decimal UnitsPrice,

        [property: DefaultValue(100)]
        int StockQuantity,

        [property: DefaultValue(true)]
        bool IsReturnAble,

        DateTime CreatedAt
    );

    public sealed record GetProductResponse(
        [property: DefaultValue(1)]
        int Id,
        [property: DefaultValue("Drinking Water 19L")]
        string ProductName,
        [property: DefaultValue(1)]
        int? CategoryId,
        [property: DefaultValue("Water")]
        string? CategoryName,

        decimal UnitsPrice,

        [property: DefaultValue(100)]
        int SockQuantity,
        [property: DefaultValue(true)]
        bool IsReturnAble,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    public sealed record UpdateProductRequest(
        [param: Required, MinLength(2), MaxLength(100)]
        [param: Description("The name of the product")]
        [param: DefaultValue("Drinking Water 19L")]
        string ProductName,

        [param: Description("The category ID of the product")]
        [param: DefaultValue(1)]
        int? CategoryId,

        [param: Required]
        [param: Description("The price per unit")]
        decimal UnitsPrice,

        [param: Required]
        [param: Description("The stock quantity")]
        [param: DefaultValue(100)]
        int SockQuantity,

        [param: Required]
        [param: Description("Is the product returnable")]
        [param: DefaultValue(true)]
        bool IsReturnAble
    );

    public sealed record UpdateProductResponse(
        [property: DefaultValue(1)]
        int Id,
        [property: DefaultValue("Drinking Water 19L")]
        string ProductName,
        [property: DefaultValue(1)]
        int? CategoryId,

        decimal UnitsPrice,

        [property: DefaultValue(100)]
        int SockQuantity,
        [property: DefaultValue(true)]
        bool IsReturnAble,
        DateTime UpdatedAt
    );
    public sealed record DeleteProductResponse(
        [property: DefaultValue("Product deleted successfully.")]
        string Message
    );
}