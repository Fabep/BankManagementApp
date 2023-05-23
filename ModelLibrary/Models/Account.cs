using System;
using System.Collections.Generic;

namespace ModelLibrary.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Frequency { get; set; } = null!;

    public DateTime Created { get; set; }

    public decimal Balance { get; set; }

    public virtual ICollection<Disposition> Dispositions { get; } = new List<Disposition>();

    public virtual ICollection<Loan> Loans { get; } = new List<Loan>();

    public virtual ICollection<PermenentOrder> PermenentOrders { get; } = new List<PermenentOrder>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}
