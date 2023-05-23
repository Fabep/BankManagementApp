using ModelLibrary.ViewModels;

namespace ServiceLibrary.Services
{
    public class SortServices : ISortServices
    {
        public List<SortViewModel> SortIcons(List<string> properties, string q)
        {
            var sortIcons = new List<SortViewModel>();
            foreach (var property in properties)
            {
                sortIcons.Add( new SortViewModel { Property = property, Q = q });
            }
            return sortIcons;
        }
    }
}
