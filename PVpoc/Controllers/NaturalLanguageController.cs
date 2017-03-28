using System.Web.Http;
using PVpoc.GoogleAPI;

namespace PVpoc.Controllers
{
    public class NaturalLanguageController: ApiController
    {
        //[Route("AnalyzeEntities")]
        //public string GetAnalyzeEntities(string text)
        //{
        //    return NaturalLanguage.AnalyzeEntitiesFromText(text).ToString();
        //}

        [Route("AnnotatedText")]
        public string GetAnnotatedText(string text)
        {
            return NaturalLanguage.AnalyzeEverything(text).ToString();
        }
    }
}