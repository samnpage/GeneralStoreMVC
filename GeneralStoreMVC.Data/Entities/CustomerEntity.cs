﻿using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace GeneralStoreMVC.Data.Entities;

public partial class CustomerEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
}
