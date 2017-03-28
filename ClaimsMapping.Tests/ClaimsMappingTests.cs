using System.Collections.Generic;
using Google.Apis.CloudNaturalLanguage.v1.Data;
using NUnit.Framework;

namespace ClaimsMapping.Tests
{
    [TestFixture]
    public class ClaimsMappingTests
    {
        [Test]
        public void TestEntityMapping()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>()
                {
                    new Entity()
                    {
                        Name = "iPhone",
                        Type = "CONSUMER_GOOD"
                    },
                    new Entity()
                    {
                        Name = "beach",
                        Type = "LOCATION"
                    }
                },
                Tokens = new List<Token>()
            };
            
            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.AreEqual("iPhone", claim.DamagedItem);
            Assert.AreEqual("beach", claim.DamageLocation);
        }
    }
}
