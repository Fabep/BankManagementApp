using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.ViewModels
{
    public class CountryViewModel
    {
        public string CountryName { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public int CountryCustomerCount { get; set; }
        public int CountryAccountCount { get; set; }
        public decimal CountryTotalBalance { get; set; }
    }
}
