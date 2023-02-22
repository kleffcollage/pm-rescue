using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
//using PropertyMataaz.Models;

namespace PropertyMataaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost("upload", Name = nameof(Upload))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<MediaView>> Upload(MediaModel media)
        {
            return Ok(_mediaService.UploadMedia(media));
        }

        [HttpDelete("delete/{id}", Name = nameof(DeleteMedia))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<MediaView>> DeleteMedia(int id)
        {
            return Ok(_mediaService.DeleteMedia(id));
        }


    }
}