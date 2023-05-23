using ModelLibrary.Models;
namespace ServiceLibrary.Services
{
    public interface IDispositionService
    {
        public Task AddDisposition(int accountId, int customerId);
    }
}
