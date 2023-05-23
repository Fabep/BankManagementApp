namespace ModelLibrary.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Frequency { get; set; }
        public decimal Balance { get; set; }
        public int LoanCount { get; set; } = 0;
        public int PermOrderCount { get; set; } = 0;
        public int TransactionCount { get; set; } = 0;
    }
}
