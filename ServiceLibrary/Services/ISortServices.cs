using ModelLibrary.ViewModels;

namespace ServiceLibrary.Services
{
    public interface ISortServices
    {
        public List<SortViewModel> SortIcons(List<string> properties, string q);
    }
}
