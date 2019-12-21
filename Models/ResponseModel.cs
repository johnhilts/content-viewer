using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace dotnet.Models
{
    public class ResponseModel
    {
        public Guid Id {get; set;}
        public String CurrentFolder {get; set;}
        public IEnumerable<ContentModel> Folders {get; set;}
        public IEnumerable<ContentModel> Files {get; set;}
        public StringDictionary Links {get; set;}
    }
}

