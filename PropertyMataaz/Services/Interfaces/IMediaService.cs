using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IMediaService
    {
         StandardResponse<Media> UploadMedia(MediaModel media);
         StandardResponse<bool> DeleteMedia(int Id);
    }
}