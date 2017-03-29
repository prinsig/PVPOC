using System.Web.Http;
using PV.POC.Mapper;
using PV.POC.WEB.API.GoogleAPI;

namespace PV.POC.WEB.API.Controllers
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