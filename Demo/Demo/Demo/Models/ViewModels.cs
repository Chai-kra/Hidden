using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

#nullable disable warnings

public class ProductVM
{
    [StringLength(4)]
    [RegularExpression(@"P\d{3}", ErrorMessage = "Invalid {0} format.")]
    [Remote("CheckId", "Product", ErrorMessage = "Duplicated {0}.")]
    public string Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [Range(0.01, 99.99)]
    [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid {0} format.")]
    public decimal Price { get; set; }

    [DisplayName("Category")]
    [StringLength(4)]
    [Remote("CheckCategoryId", "Product", ErrorMessage = "Invalid {0}.")]
    public string CategoryId { get; set; }
}

public class CategoryVM
{
    [StringLength(4)]
    [RegularExpression(@"C\d{3}", ErrorMessage = "Invalid {0} format.")]
    [Remote("CheckId", "Category", ErrorMessage = "Duplicated {0}.")]
    public string Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }
}
