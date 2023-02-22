using System.Collections;
namespace PropertyMataaz.Models.UtilityModels
{
    public class PagedResult<T>
    {
        public IEnumerable Items {get;set;}
        public int TotalSize {get;set;}
    }
}