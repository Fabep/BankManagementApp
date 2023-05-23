namespace ModelLibrary.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public string Gender { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Streetaddress { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Zipcode { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string CountryCode { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        public string? SocialSecurityNumber { get; set; }

        public string? Telephonecountrycode { get; set; }

        public string? Telephonenumber { get; set; }

        public string? Emailaddress { get; set; }
    }
}
