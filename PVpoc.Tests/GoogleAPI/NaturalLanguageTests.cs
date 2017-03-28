using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PVpoc.GoogleAPI;
using static Google.Cloud.Language.V1.AnnotateTextRequest.Types;

namespace PVpoc.Tests.GoogleAPI
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
            Console.WriteLine(result.ToString());
            Assert.IsTrue(result.ToString().Contains("sentiment"));
        }
    }
}
