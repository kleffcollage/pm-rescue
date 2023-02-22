using System;
using System.Linq;
using AutoMapper;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Abstrctions;
using PropertyMataaz.Utilities.Constants;

namespace PropertyMataaz.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMapper _mapper;
        private readonly ICodeProvider _codeProvider;
        private readonly IMediaRepository _mediaRepository;

        public MediaService(IMapper mapper, ICodeProvider codeProvider, IMediaRepository mediaRepository)
        {
            _mapper = mapper;
            _codeProvider = codeProvider;
            _mediaRepository = mediaRepository;
        }

        public StandardResponse<bool> DeleteMedia(int Id)
        {
            try
            {
                var thisMedia = _mediaRepository.List().FirstOrDefault(m => m.Id == Id);
                _mediaRepository.Delete(thisMedia);
                return StandardResponse<bool>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<bool>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<Media> UploadMedia(MediaModel media)
        {
            try
            {
                media.Name = _codeProvider.New(0, Constants.NEW_PROPERTY_MEDIA_NAME, 0, 10, Constants.PROPERTY_MATAAZ_MEDIA_PREFIX).CodeString;

                Media NewMedia = _mapper.Map<Media>(media);

                var Result = _mediaRepository.UploadMedia(NewMedia).Result;

                if (!Result.Succeeded)
                    return StandardResponse<Media>.Ok().AddStatusMessage(StandardResponseMessages.MEDIA_UPLOAD_FAILED);

                return StandardResponse<Media>.Ok().AddData(Result.UploadedMedia).AddStatusMessage(StandardResponseMessages.MEDIA_UPLOAD_SUCCESSFUL);
            }
            catch (Exception e)
            {
                return StandardResponse<Media>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }
    }
}