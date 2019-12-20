using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using dotnet.Services;
using dotnet.Services.Models;

namespace dotnet.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentApiController : ControllerBase
    {
        private readonly HttpContext _context;

        private readonly ContentSettingsModel _contentModel;
        private readonly UserSessionService _sessionService;
        private readonly UserSessionModel _sessionModel;

        public ContentApiController(IOptions<ContentSettingsModel> model, IHttpContextAccessor contextAccessor)
        {
            _contentModel = model.Value;
            _context = contextAccessor.HttpContext;
            _sessionService = new UserSessionService(_context);
        }

        public enum ContentType
        {
            Folder,
            File,
        }

        public class ResponseModel
        {
            public Guid Id {get; set;}
            public IEnumerable<ContentInfo> Folders {get; set;}
            public IEnumerable<ContentInfo> Files {get; set;}
        }

        public class ContentInfo
        {
            public string Name {get; set;}
            public ContentType ContentType  {get; set;}
        }

        [Route("folder")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            // TODO: we can just get the session info on-demand - putting this here for now as an example ...
            var session = await _sessionService.GetSession(_contentModel.Root);
            // TODO: put IO logic in a utility ...
            var folders = Directory.GetDirectories(_contentModel.Root);
            var contentFolders = folders.Select(s => new ContentInfo {Name = Path.GetFileName(s), ContentType = ContentType.Folder, });
            var files = Directory.GetFiles(_contentModel.Root);
            var contentFiles = files.Select(s => new ContentInfo {Name = Path.GetFileName(s), ContentType = ContentType.File, });
            return Ok(new ResponseModel {Id = session.SessionId, Folders = contentFolders, Files = contentFiles, });
        }

    }
}

