using Microsoft.VisualStudio.TestTools.UnitTesting;
using PV.POC.Mapper;
using PV.POC.WEB.API.Controllers;

namespace PV.POC.WEB.API.Tests.Controllers
{

    [TestClass]
    public class NaturalLanguageControllerTests
    {

        private readonly string inputText = "I had my phone stolen while playing with my dog at the park last night.";
        private Claim result;
        private NaturalLanguageController controller;

        [TestInitialize]
        public void Setup()
        {
            controller = new NaturalLanguageController();
            result = controller.GetAnnotatedText(inputText);
        }

        [TestMethod]
        public void GetAnnotatedText_Response_ShoudNotBeNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAnnotatedText_ResponseContainsSentiment_ShouldBeSuccess()
        {
            //Assert.IsTrue(result.Contains("sentiment"));
        }

        [TestMethod]
        public void GetAnnotatedText_ResponseContainsEntities_Success()
        {
            //var jsonResult = (JObject)JsonConvert.DeserializeObject(result);
            //Assert.IsTrue(jsonResult["entities"].Count() > 0);
        }
    }
}
;