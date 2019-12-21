using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using dotnet.Models;
using dotnet.Services;
using dotnet.Services.Models;
using dotnet.Utils;

namespace dotnet.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/content")]
    public class ContentApiController : ControllerBase
    {
        private readonly HttpContext _context;

        private readonly ContentSettingsModel _contentModel;
        private readonly UserSessionService _sessionService;
        // private readonly UserSessionModel _sessionModel;

        public ContentApiController(IOptions<ContentSettingsModel> model, IHttpContextAccessor contextAccessor)
        {
            _contentModel = model.Value;
            _context = contextAccessor.HttpContext;
            _sessionService = new UserSessionService(_context);
            // _sessionModel = await _sessionService.GetSession(_contentModel.Root); 
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await GetContentModel(string.Empty));
        }

        [HttpGet]
        {
        }

        [HttpGet]
        [Route("folder/{folderName}")]
        public async Task<IActionResult> Get(string folderName)
        {
            return Ok(await GetContentModel(folderName));
        }

        // TODO: implement Route("folder/back")

        private async Task<ResponseModel> GetContentModel(string folderName)
        {
            var currentRequestFolder = $"{_contentModel.Root}/{folderName}";
            var updatedSession = await _sessionService.UpdateSession(new UserSessionUpdateModel {CurrentFolder = currentRequestFolder, }); 
            var currentFolder = FileUtility.MapContentFolder(updatedSession.CurrentFolder);
            var folders = FileUtility.GetFolders(currentRequestFolder);
            var files = FileUtility.GetFiles(currentRequestFolder);
            var links = new StringDictionary {{"Folder", "api/content/folder/{folderName}"}, {"Back", "api/content/folder/back"}, };
            return new ResponseModel {Id = updatedSession.SessionId, CurrentFolder = currentFolder, Folders = folders, Files = files, Links = links, };
        }

    }
}

