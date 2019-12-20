using System;

namespace dotnet.Services.Models
{
    public class UserSessionModel
    {
        public Guid SessionId {get; set;}
        public string CurrentFolder {get; set;}
    }
}

