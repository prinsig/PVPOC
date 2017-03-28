using System.Web.Http;
using PVpoc.GoogleAPI;

namespace PVpoc.Controllers
{
    public class NaturalLanguageController: ApiController
    {
        //public string GetAnalyzeEntities(string text)
        //{
        //    return NaturalLanguage.AnalyzeEntitiesFromText(text).ToString();
        //}

        public string GetAnnotatedText(string text)
        {
            return NaturalLanguage.AnalyzeEverything(text).ToString();
        }
    }
}