using InfrastructureLibrary.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Reflection.Emit;

namespace ServiceLibrary.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BankAppDataContext _bankContext;

        private IQueryable<Customer> Customers => _bankContext.Customers;
        public CustomerService(BankAppDataContext bankContext)
        {
            _bankContext = bankContext;
        }

        public PagedResult<Customer> GetCustomers(string sortOrder, string sortColumn, string? q, int page)
        {
            var query = Customers;

            if (!string.IsNullOrEmpty(q))
            {
                var qSplit = q.Split(' ');
                var qSplitUnique = new HashSet<string>(qSplit);
                foreach (var qForIteration in qSplitUnique)
                {
                    query = query.Where(
                        c => c.City.Contains(qForIteration) ||
                        c.Givenname.Contains(qForIteration) ||
                        c.Surname.Contains(qForIteration));
                }
            }

            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "asc";
            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "Name";


            if (sortColumn == "Id")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.CustomerId);
                else
                    query = query.OrderBy(c => c.CustomerId);
            }

            if (sortColumn == "Name")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Givenname);
                else
                    query = query.OrderBy(c => c.Givenname);
            }
            if (sortColumn == "City")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.City);
                else
                    query = query.OrderBy(c => c.City);
            }

            if (sortColumn == "Address")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Country);
                else
                    query = query.OrderBy(c => c.Country);

            }
            if (sortColumn == "SocialSecurityNumber")
            {
                if (sortOrder == "desc")
                    query = query.OrderByDescending((c) => c.NationalId);
                else
                    query = query.OrderBy((c) => c.NationalId);
            }

            return query.GetPaged(page, 50);
        }
        public Task<List<Customer>> GetCustomers()
        {
            return Customers.ToListAsync();
        }

        public int CustomerCount()
        {
            return Customers.Count();
        }

        public Task<Customer> GetCustomerById(int id)
        {
            return Customers.FirstAsync(c => c.CustomerId == id);
        }
        public List<Customer> GetCustomersFromAccount(Account account)
        {
            return _bankContext.Dispositions.Where(a => a.AccountId == account.AccountId).Select(c => c.Customer).ToList();
        }
        public string GetCountryOrCountryCode(string countryString)
        {
            switch (countryString.ToLower())
            {
                case "no":
                    return "Norway";
                case "se":
                    return "Sweden";
                case "dk":
                    return "Denmark";
                case "fi":
                    return "Finland";
            }
            switch (countryString.ToLower())
            {
                case "norway":
                    return "NO";
                case "sweden":
                    return "SE";
                case "denmark":
                    return "DK";
                case "finland":
                    return "FI";
            }
            return "";
        }
        public string GetPhoneCountryCode(string country)
        {
            switch (country.ToLower())
            {
                case "denmark":
                    return "45";
                case "sweden":
                    return "46";
                case "norway":
                    return "47";
                case "finland":
                    return "358";
            };
            return "";
        }
        public async Task<Customer> CreateCustomer(string givenname, string surname, string streetaddress, string city, string zipcode, string country, DateTime birthday, string socialSecurityNumber, string telephoneNumber, string emailaddress, string gender)
        {
            var cus = new Customer
            {
                Givenname = givenname,
                Surname = surname,
                Streetaddress = streetaddress,
                City = city,
                Zipcode = zipcode,
                Country = country,
                CountryCode = GetCountryOrCountryCode(country),
                Birthday = birthday,
                NationalId = socialSecurityNumber,
                Emailaddress = emailaddress,
                Telephonenumber = telephoneNumber,
                Telephonecountrycode = GetPhoneCountryCode(country),
                Gender = gender,
            };

            await _bankContext.Customers.AddAsync(cus);
            await _bankContext.SaveChangesAsync();

            return cus;
        }
        public async Task<string> UpdateCustomer(int id, string givenname, string surname, string streetaddress, string city, string zipcode, string country, DateTime birthday, string socialSecurityNumber, string telephoneNumber, string emailaddress, string gender)
        {
            var customerToUpdate = await GetCustomerById(id);
            if (customerToUpdate == null)
            {
                return "Customer not found";
            }
            customerToUpdate.Givenname = givenname;
            customerToUpdate.Surname = surname;
            customerToUpdate.Streetaddress = streetaddress;
            customerToUpdate.City = city;
            customerToUpdate.Zipcode = zipcode;
            customerToUpdate.Country = country;
            customerToUpdate.Birthday = birthday;
            customerToUpdate.NationalId = socialSecurityNumber;
            customerToUpdate.Emailaddress = emailaddress;
            customerToUpdate.Telephonenumber = telephoneNumber;
            customerToUpdate.Gender = gender;
            customerToUpdate.CountryCode = GetCountryOrCountryCode(country);
            customerToUpdate.Telephonecountrycode = GetPhoneCountryCode(country);
            await _bankContext.SaveChangesAsync();

            return "Succesfully updated customer.";
        }
    }
}
