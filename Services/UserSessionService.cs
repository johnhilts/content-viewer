using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using dotnet.Services.Models;

namespace dotnet.Services
{
    public class UserSessionService
    {
        private readonly HttpContext _context;

        public UserSessionService(HttpContext context)
        {
            _context = context;
        }

        public async Task<UserSessionModel> GetSession(string defaultCurrentFolder)
        {
            var cookieSessionExists = Guid.TryParse(_context.Request.Cookies["UserSessionCookieKey"]?.ToString() ?? string.Empty, out Guid userSessionId);
            if (cookieSessionExists)
            {
                return await GetExistingSession(userSessionId);
            }
            else
                return await CreateNewSession(defaultCurrentFolder);
        }

        public async Task<UserSessionModel> GetExistingSession(Guid userSessionId) =>
            JsonSerializer.Deserialize<UserSessionModel>((await File.ReadAllTextAsync("s.json")));

        public async Task<UserSessionModel> CreateNewSession(string defaultCurrentFolder)
        {
            var model = new UserSessionModel {SessionId = Guid.NewGuid(), CurrentFolder = defaultCurrentFolder, };
            await SaveSession(model);
            return model;
        }

        public async Task UpdateSession(UserSessionUpdateModel updateModel)
        {
            var oldModel = await GetSession(null);
            var writeModel = new UserSessionModel {SessionId = oldModel.SessionId, CurrentFolder = updateModel.CurrentFolder, };
            await SaveSession(writeModel);
        }

        private async Task SaveSession(UserSessionModel model)
        {
            CreateSessionCookie(model);
            await File.WriteAllTextAsync("s.json", JsonSerializer.Serialize(model));
        }

        private void CreateSessionCookie(UserSessionModel model)
        {
            var options = GetCookieOptions();

            _context.Response.Cookies.Append("UserSessionCookieKey", model.SessionId.ToString(), options);
        }

        private void RemoveSessionCookie(UserSessionModel model)
        {
            var cookieRemoveExpiration = DateTime.Now.AddDays(-1);
            var options = new CookieOptions { Expires = cookieRemoveExpiration, };

            _context.Response.Cookies.Append("UserSessionCookieKey", model.SessionId.ToString(), options);
        }

        private CookieOptions GetRemoveCookieOptions()
        {
            var cookieRemoveExpiration = DateTime.Now.AddDays(-1);
            return new CookieOptions { Expires = cookieRemoveExpiration, };
        }

        private CookieOptions GetCookieOptions()
        {
            return GetCookieOptions(null);
        }

        private CookieOptions GetCookieOptions(int? expiration)
        {
            var cookieDefaultExpiration = expiration.HasValue ? DateTime.Now.AddMinutes(expiration.Value) : DateTime.Now.AddYears(50);
            var useSecure = false; // !_applicationService.SiteSettings.KestrelSslBypass;
            var sameSiteMode = useSecure ? SameSiteMode.Strict : SameSiteMode.None;
            return new CookieOptions { Expires = cookieDefaultExpiration, Secure = useSecure, SameSite = sameSiteMode, };
        }

    }
}
