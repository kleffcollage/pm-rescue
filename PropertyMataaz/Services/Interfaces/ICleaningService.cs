using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface ICleaningService
    {
         StandardResponse<CleaningView> CreateRequest(CleaningModel cleaningModel);
         StandardResponse<CleaningQuoteView> AddQuoteToRequest(CleaningQuoteModel cleaningQuote);
         StandardResponse<PagedCollection<CleaningView>> ListMyRequests(PagingOptions options);
         StandardResponse<PagedCollection<CleaningView>> ListAllRequests(PagingOptions options);
         StandardResponse<CleaningView> GetRequestById(int Id);
         StandardResponse<CleaningView> AcceptQuote(int Id); 
         StandardResponse<CleaningView> RejectQuote(int Id); 
         
    }
}