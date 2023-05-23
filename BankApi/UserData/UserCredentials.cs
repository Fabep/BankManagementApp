using ServiceLibrary.Services;

namespace BankApi.UserData
{
    public class UserCredentials
    {
        private readonly ICustomerService _customerService;
        public static List<UserModel> Users = new List<UserModel>();
        public UserCredentials(ICustomerService customerService)
        {  
            _customerService = customerService;
            FillUserList();
        }
        public void FillUserList()
        {
            Users = _customerService.GetCustomers().Select(c => new UserModel
            {
                Email = c.Emailaddress,
                Givenname = c.Givenname,
                Surname = c.Surname,
                Password = c.Givenname + c.Surname,
                UserName = c.Givenname + "@" + c.Surname + ".net",
                Role = "User"
            }).ToList();
        }
    }
}
