using ModelLibrary.Models;
using ServiceLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLibrary.ViewModels;

namespace BankAssignment.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ICountryService _countryService;
        public CountryViewModel SwedenInfo { get; set; } = null!;
        public CountryViewModel NorwayInfo { get; set; } = null!;
        public CountryViewModel DenmarkInfo { get; set; } = null!;
        public CountryViewModel FinlandInfo { get; set; } = null!;

        public IndexModel(ILogger<IndexModel> logger, ICountryService countryService)
        {
            _logger = logger;
            _countryService = countryService;
        }

        public void OnGet()
        {
            SwedenInfo = new CountryViewModel
            {
                CountryCode = "SE",
                CountryName = "Sweden",
                CountryAccountCount = _countryService.GetAccountsCountry("Sweden").Count(),
                CountryCustomerCount = _countryService.GetCustomersCountry("Sweden").Count(),
                CountryTotalBalance = _countryService.GetTotalBalanceCountry("Sweden")
            };

            DenmarkInfo = new CountryViewModel
            {
                CountryCode = "DK",
                CountryName = "Denmark",
                CountryAccountCount = _countryService.GetAccountsCountry("Denmark").Count(),
                CountryCustomerCount = _countryService.GetCustomersCountry("Denmark").Count(),
                CountryTotalBalance = _countryService.GetTotalBalanceCountry("Denmark")
            };

            NorwayInfo = new CountryViewModel
            {
                CountryCode = "NO",
                CountryName = "Norway",
                CountryAccountCount = _countryService.GetAccountsCountry("Norway").Count(),
                CountryCustomerCount = _countryService.GetCustomersCountry("Norway").Count(),
                CountryTotalBalance = _countryService.GetTotalBalanceCountry("Norway")
            };

            FinlandInfo = new CountryViewModel
            {
                CountryCode = "FI",
                CountryName = "Finland",
                CountryAccountCount = _countryService.GetAccountsCountry("Finland").Count(),
                CountryCustomerCount = _countryService.GetCustomersCountry("Finland").Count(),
                CountryTotalBalance = _countryService.GetTotalBalanceCountry("Finland")
            };



        }
    }
}