using System.ComponentModel.DataAnnotations;
using System.Reflection; 
using ServiceLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using ModelLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankAssignment.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly ISortServices _sortServices;
        private readonly IMapper _mapper;

        public IndexModel(ICustomerService customerService, ISortServices sortServices, IMapper mapper)
        {
            _customerService = customerService;
            _sortServices = sortServices;
            _mapper = mapper;
        }

        public List<CustomerListView> Customers { get; set; }

        public string Q { get; set; }
        public int CurrentPage { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int PageCount { get; set; }

        public List<SortViewModel> CustomerSortPartial { get; set; }

        public IEnumerable<int> PagesToShow { get; set; }

        public string? Message { get; set; }
        public void OnGet(string q, string sortOrder, string sortColumn, int pageNo, string? message)
        {
            Message = message;

            Q = q;
            SortOrder = sortOrder;
            SortColumn = sortColumn;
            if (pageNo == 0)
                pageNo = 1;
            
            var result = _customerService.GetCustomers(sortOrder, sortColumn, q, pageNo);

            CurrentPage = result.CurrentPage;
            PageCount = result.PageCount;

            Customers = _mapper.Map<List<CustomerListView>>(result.Results);
            

            // Bad way to solve this problem, might fix later idk :)
            var indexUp = CurrentPage;
            var indexDown = indexUp;
            var pagesList = new List<int>();

            while (indexUp <= CurrentPage + 2)
            {
                pagesList.Add(indexUp);
                pagesList.Add(indexDown);
                indexUp += 1;
                indexDown -= 1;
            }
            for (var i  = 0; i < pagesList.Count(); i++)
            {
                if (pagesList[i] < 0 || pagesList[i] > PageCount)
                {
                    pagesList.Remove(i);
                } 
            }
            PagesToShow = pagesList.Distinct().OrderBy(c => c);

            Type type = typeof(CustomerListView);
            var properties = type.GetProperties().Select(c => c.Name).ToList();

            CustomerSortPartial = _sortServices.SortIcons(properties, q);
        }
    }
}
