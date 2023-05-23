using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankAssignment.Enumerators
{
    public class GenericEnumHandler
    {
        public static List<SelectListItem> ToSelectListItem<T>(T _) where T : struct, Enum
        {
            return Enum.GetValues<T>().Select(g => new SelectListItem
            {
                Value = g.ToString(),
                Text = g.ToString()
            }).ToList();
        }
    }
}
