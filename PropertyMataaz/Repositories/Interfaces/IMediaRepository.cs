using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IMediaRepository 
    {
         Task<(bool Succeeded, Media UploadedMedia)> UploadMedia(Media media);
         Media GetTenancyAgreement(int TenancyId);
         List<Media> UploadBulkMedia(List<Media> medias);
         List<Media> GetByPropertyId(int id);
         void Delete(Media media);
         IEnumerable<Media> List();
    }
}