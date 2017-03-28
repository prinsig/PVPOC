using System.Web.Http;
using PVpoc.GoogleAPI;
using ClaimsMapping;
using Google.Cloud.Language.V1;

namespace PVpoc.Controllers
{
    public class NaturalLanguageController: ApiController
    {

        [Route("AnnotatedText")]
        public Claim GetAnnotatedText(string text)
        {
            var claim = new ClaimPopulator();
            return claim.PopulateClaim(NaturalLanguage.AnalyzeEverything(text));
        }
    }
}