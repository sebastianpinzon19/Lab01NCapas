namespace Entities.Models;

[Table("Product")]
[Index("ProductName", Name = "IndexProductName")]
[Index("SupplierId", Name = "IndexProductSupplierId")]
public partial class Product
{
    [Key]
    public int Id { get; set; }

    [Required] // Agrega esta anotación para asegurarte de que el nombre del producto es obligatorio
    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    public int? SupplierId { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Range(0, 999999999999.99)] // Agrega un rango para asegurar que el precio es válido
    public decimal? UnitPrice { get; set; } = 0; // Asigna un valor predeterminado de 0

    [StringLength(30)]
    public string? Package { get; set; }

    public bool IsDiscontinued { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier Supplier { get; set; } = null!;
}
