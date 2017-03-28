using System.Web.Http;
using PVpoc.GoogleAPI;
using ClaimsMapping;
using Google.Cloud.Language.V1;

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
        public Claim GetAnnotatedText(string text)
        {
            var claim = new ClaimPopulator();
            return claim.PopulateClaim(NaturalLanguage.AnalyzeEverything(text));
        }
    }
}