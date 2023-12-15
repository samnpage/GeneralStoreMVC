using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralStoreMVC.Data.Entities;

public partial class TransactionEntity
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerId { get; set; }

    public int Quantity { get; set; }

    public DateTime DateOfTransaction { get; set; }

    public virtual CustomerEntity Customer { get; set; } = null!;

    public virtual ProductEntity Product { get; set; } = null!;
}
