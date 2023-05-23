namespace ModelLibrary.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? TransferId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }
}
