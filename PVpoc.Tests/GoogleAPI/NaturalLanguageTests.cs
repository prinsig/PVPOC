using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PV.POC.WEB.API.GoogleAPI;

namespace PV.POC.WEB.API.Tests.GoogleAPI
{
    [TestClass]
    public class NaturalLanguageTests
    {
        private readonly string inputText = "I had my phone stolen while playing with my dog at the park last night.";
        private Google.Cloud.Language.V1.AnnotateTextResponse result;

        [TestInitialize]
        public void Setup()
        {
            result = NaturalLanguage.AnalyzeEverything(inputText);
        }

        [TestMethod]
        public void AnalyzeEverything_Response_ShoudNotBeNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AnalyzeEverything_ResponseContainsSentiment_ShouldBeSuccess()
        {
            Assert.IsTrue(result.ToString().Contains("sentiment"));
        }

        [TestMethod]
        public void AnalyzeEverything_ResponseContainsPhoneEntity_Success()
        {
            Assert.IsTrue(result.Entities.Any(e => e.Name == "phone"));
        }
    }
}
