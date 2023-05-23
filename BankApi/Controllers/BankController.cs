using AutoMapper;
using BankApi.UserData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.DTO;
using ModelLibrary.Models;
using ModelLibrary.ViewModels;
using ServiceLibrary.Services;
using System.Security.Claims;

namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class BankController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public BankController(ICustomerService customerService, IAccountService accountService, IMapper mapper)
        {
            _customerService = customerService;
            _accountService = accountService;
            _mapper = mapper;
        }

        // READ ALL 
        /// <summary>
        /// Retrieves a customer from the database
        /// </summary>
        /// <returns>
        /// A customer.
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/me/{id}
        /// </remarks>
        /// <response code="200">
        /// Succesfully retrieved the customer. 
        /// </response>
        /// <response code="400">
        /// Could not find the customer.
        /// </response>
        /// <response code="401">
        /// User can't view other users profiles.
        /// </response>
        [HttpGet]
        [Route("me/{id}")]
        [Authorize(Roles = "User, Cashier")]
        public async Task<ActionResult<CustomerDTO>> Me(int id)
        {
            try
            {
                var cus = _mapper.Map<CustomerDTO>(await _customerService.GetCustomerById(id));
                var userCheck = $"{cus.Name.Split(' ')[0]}@{cus.Name.Split(' ')[1]}.net";
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != userCheck)
                {
                    return Unauthorized();
                }
                
                return Ok(cus);
            }
            catch
            {
                return BadRequest("Customer not found.");
            }
        }

        // READ ALL 
        /// <summary>
        /// Retrieves a specific amount of transactions from an account
        /// </summary>
        /// <returns>
        /// A list of transactions.
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/account/{id}/{limit}/{offset}
        /// </remarks>
        /// <response code="200">
        /// Succesfully retrieved transactions.
        /// </response>
        /// <response code="400">
        /// Account not found.
        /// </response>
        [HttpGet]
        [Route("account/{id}/{limit}/{offset}")]
        [Authorize(Roles = "User, Cashier")]
        public async Task<ActionResult<List<TransactionDTO>>> Account(int id, int limit, int offset)
        {
            try
            {
                var acc = await _accountService.GetAccountById(id);
                foreach(var cus in _mapper.Map<List<CustomerDTO>>(_customerService.GetCustomersFromAccount(acc)))
                {
                    var userCheck = $"{cus.Name.Split(' ')[0]}@{cus.Name.Split(' ')[1]}.net";
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userId != userCheck)
                    {
                        return Unauthorized();
                    }
                }
                var tForMap = acc.Transactions.OrderByDescending(a => a.Date).Skip(offset).Take(limit).ToList();
                var transactions = _mapper.Map<List<TransactionDTO>>(tForMap);

                return Ok(transactions);
            }
            catch
            {
                return BadRequest("Account not found.");
            }
        }
    }
}