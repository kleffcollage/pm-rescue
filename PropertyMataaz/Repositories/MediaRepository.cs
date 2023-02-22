using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PropertyMataaz.ContentServer;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Abstrctions;
using static PropertyMataaz.ContentServer.BaseContentServer;

namespace PropertyMataaz.Repositories
{
    public class MediaRepository : BaseRepository<Media>, IMediaRepository
    {
        private readonly Globals _globals;
        private readonly IUtilityMethods _utilityMethods;
        public MediaRepository(PMContext context, IOptions<Globals> globals, IUtilityMethods utilityMethods) : base(context)
        {
            _globals = globals.Value;
            _utilityMethods = utilityMethods;
        }

        public Media GetTenancyAgreement(int TenancyId)
        {
            try
            {
                return _context.Media.LastOrDefault(m => m.TenancyId == TenancyId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public List<Media> GetByPropertyId(int id)
        {
            try
            {
                return _context.Media.Where(m => m.PropertyId == id).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public List<Media> UploadBulkMedia(List<Media> medias)
        {
            // try
            // {
            //     return _context.Media.ToList<Media>();
            // }
            // catch (Exception ex)
            // {
            //     Logger.Error(ex);
            //     return null;
            // }
            throw new System.NotImplementedException();

        }

        public IEnumerable<Media> List()
        {
            try
            {
                return _context.Media;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public async Task<(bool Succeeded, Media UploadedMedia)> UploadMedia(Media media)
        {
            FileDocument UploadResult = FileDocument.Create();
            try
            {

                if (!string.IsNullOrEmpty(media.Base64String))
                {

                    UploadResult = await
                    Build(ContentServerTypeEnum.GOOGLEDRIVE, _globals, _utilityMethods)
                    .UploadDocumentAsync(FileDocument.Create(media.Base64String, media.Name, "", FileDocumentType.GetDocumentType(media.Extention)));
                    media.Url = UploadResult.Path;
                }

                var CreatedMedia = CreateAndReturn(media);
                return (true, CreatedMedia);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return (false, null);
            }
        }
    }
}