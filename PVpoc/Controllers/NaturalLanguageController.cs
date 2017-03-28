using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Google.Cloud.Language.V1;
using static Google.Cloud.Language.V1.AnnotateTextRequest.Types;
using PVpoc.GoogleAPI;

namespace PVpoc.Controllers
{
    public class NaturalLanguageController: ApiController
    {
        public string GetAnnotatedText(string text)
        {
            return NaturalLanguage.AnalyzeEverything(text).ToString();
        }
    }
}