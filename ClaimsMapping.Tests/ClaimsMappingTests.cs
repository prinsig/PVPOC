using System.Collections.Generic;
using Google.Apis.CloudNaturalLanguage.v1.Data;
using NUnit.Framework;

namespace ClaimsMapping.Tests
{
    [TestFixture]
    public class ClaimsMappingTests
    {
        [Test]
        public void TestEntityMapping_EntitiesOnly_NoDamage()
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
            Assert.IsNull(claim.TypeOfDamage);
        }

        [Test]
        public void TestDamageMapping_DamagedTokenPresent()
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
                {
                    new Token()
                    {
                        Text = new TextSpan()
                        {
                            Content = "broken",
                        },
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "VERB"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.AreEqual("iPhone", claim.DamagedItem);
            Assert.AreEqual("beach", claim.DamageLocation);
            Assert.AreEqual(Claim.DamageType.Damaged, claim.TypeOfDamage);
        }

        [Test]
        public void TestDamageMapping_LostTokenPresent()
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
                {
                    new Token()
                    {
                        Text = new TextSpan()
                        {
                            Content = "lost",
                        },
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "VERB"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.AreEqual("iPhone", claim.DamagedItem);
            Assert.AreEqual("beach", claim.DamageLocation);
            Assert.AreEqual(Claim.DamageType.Lost, claim.TypeOfDamage);
        }

        [Test]
        public void TestDamageMapping_StolenTokenPresent()
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
                {
                    new Token()
                    {
                        Text = new TextSpan()
                        {
                            Content = "stole",
                        },
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "VERB"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.AreEqual("iPhone", claim.DamagedItem);
            Assert.AreEqual("beach", claim.DamageLocation);
            Assert.AreEqual(Claim.DamageType.Stolen, claim.TypeOfDamage);
        }
    }
}
