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
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await GetContentModel(GetCurrentRequestFolder(new RequestModel())));
        }

        [HttpPut]
        [Route("folder")]
        public async Task<IActionResult> Put([FromBody]RequestModel model)
        {
            var currentRequestFolder = GetCurrentRequestFolder(model);
            if (!currentRequestFolder.Exists()) 
                return NotFound();

            return Ok(await GetContentModel(currentRequestFolder));
        }

        private string GetCurrentRequestFolder(RequestModel model)
        {
            return $"{_contentModel.Root}/{model.CurrentFolderName}/{model.RequestFolderName}";
        }

        private async Task<ResponseModel> GetContentModel(string currentRequestFolder)
        {
            // TODO: we don't even need session now, which means we also don't need async/await...
            var updatedSession = await _sessionService.UpdateSession(new UserSessionUpdateModel {CurrentFolder = currentRequestFolder, }); 
            var currentFolder = currentRequestFolder.MapContentFolder();
            var folders = currentRequestFolder.GetFolders();
            var files = currentRequestFolder.GetFiles();
            var links = new StringDictionary {{"Folder", "api/content/folder"}, };
            return new ResponseModel {Id = updatedSession.SessionId, CurrentFolder = currentFolder, Folders = folders, Files = files, Links = links, };
        }

    }
}

