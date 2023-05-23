using InfrastructureLibrary.Infrastructure.Paging;
using ModelLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface ICustomerService
    {
        int CustomerCount();
        PagedResult<Customer> GetCustomers(string sortOrder, string sortColumn, string q, int page);
        Task<List<Customer>> GetCustomers();
        Task<Customer> GetCustomerById(int id);
        List<Customer> GetCustomersFromAccount(Account account);
        string GetCountryOrCountryCode(string countryCode);
        Task<Customer> CreateCustomer(
            string givenname, string surname, string streetaddress, string city, string zipcode, string country, DateTime birthday, string socialSecurityNumber, string telephoneNumber, string emailaddress, string gender);
        Task<string> UpdateCustomer(
            int id, string givenname, string surname, string streetaddress, string city, string zipcode, string country, DateTime birthday, string socialSecurityNumber, string telephoneNumber, string emailaddress, string gender);
    }
}
