using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BikeVille.Models.Products;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    public int? ParentProductCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public Guid Rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }
    [JsonIgnore]
    public virtual ICollection<ProductCategory> InverseParentProductCategory { get; set; } = new List<ProductCategory>();
    [JsonIgnore]
    public virtual ProductCategory? ParentProductCategory { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
