using System;
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
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
            };
            
            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.AreEqual("iPhone", claim.DamagedItem);
            Assert.AreEqual("beach", claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.IsNull(claim.DateOfDamage);
        }

        [Test]
        public void TestDamageMapping_DamagedTokenPresent()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>()
                {
                    new Token()
                    {
                        Lemma = "break",
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "VERB"
                        }
                    }
                },
                Sentences = new List<Sentence>()
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.AreEqual(Claim.DamageType.Damaged, claim.TypeOfDamage);
            Assert.IsNull(claim.DateOfDamage);
        }

        [Test]
        public void TestDamageMapping_LostTokenPresent()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>()
                {
                    new Token()
                    {
                        Lemma = "lose",
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "VERB"
                        }
                    }
                },
                Sentences = new List<Sentence>()
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.AreEqual(Claim.DamageType.Lost, claim.TypeOfDamage);
            Assert.IsNull(claim.DateOfDamage);
        }

        [Test]
        public void TestDamageMapping_StolenTokenPresent()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>()
                {
                    new Token()
                    {
                        Lemma = "steal",
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "VERB"
                        }
                    }
                },
                Sentences = new List<Sentence>()
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.AreEqual(Claim.DamageType.Stolen, claim.TypeOfDamage);
            Assert.IsNull(claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_Today()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>()
                {
                    new Token()
                    {
                        Text = new TextSpan()
                        {
                            Content = "Today",
                        },
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "NOUN"
                        }
                    }
                },
                Sentences = new List<Sentence>()
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(DateTime.Today, claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_Yesterday()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>()
                {
                    new Token()
                    {
                        Text = new TextSpan()
                        {
                            Content = "Yesterday",
                        },
                        PartOfSpeech = new PartOfSpeech()
                        {
                            Tag = "NOUN"
                        }
                    }
                },
                Sentences = new List<Sentence>()
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(DateTime.Today.AddDays(-1), claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_LastWednesday()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "Last Wednesday I went to the shops"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);

            //Look for last Wednesday
            var comparisonDate = DateTime.Today.AddDays(-1);
            while (comparisonDate.DayOfWeek != DayOfWeek.Wednesday)
            {
                comparisonDate = comparisonDate.AddDays(-1);
            }
            Assert.AreEqual(comparisonDate, claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_specificWithDashes()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "On 11-04-2016 I went to the shops"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(new DateTime(2016, 4, 11), claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_specificWithDashesAndSingleDigitDayAndMonthDoubleDigitYear()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "On 6-4-16 I went to the shops"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(new DateTime(2016, 4, 6), claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_specificWithSlashes()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "On 11/04/2016 I went to the shops"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(new DateTime(2016, 4, 11), claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_specificWithSlashesAndSingleDigitDayAndMonthDoubleDigitYear()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "On 6/4/16 I went to the shops"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(new DateTime(2016, 4, 6), claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_specificWithBackslashes()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "On 11\\04\\2016 I went to the shops"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(new DateTime(2016, 4, 11), claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_specificWithBackslashesAndSingleDigitDayAndMonthDoubleDigitYear()
        {
            var atr = new AnnotateTextResponse
            {
                Entities = new List<Entity>(),
                Tokens = new List<Token>(),
                Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "On 6\\4\\16 I went to the shops"
                        }
                    }
                }
            };

            var claim = new ClaimPopulator().PopulateClaim(atr);
            Assert.IsNull(claim.DamagedItem);
            Assert.IsNull(claim.DamageLocation);
            Assert.IsNull(claim.TypeOfDamage);
            Assert.AreEqual(new DateTime(2016, 4, 6), claim.DateOfDamage);
        }

        [Test]
        public void TestDateMapping_dayOfMonth()
        {
            for (var i = 1; i < 12; i++)
            {
                var atr = new AnnotateTextResponse
                {
                    Entities = new List<Entity>(),
                    Tokens = new List<Token>(),
                    Sentences = new List<Sentence>()
                {
                    new Sentence()
                    {
                        Text = new TextSpan()
                        {
                            Content = "On the 6th of " + getMonth(i) + " I went to the shops"
                        }
                    }
                }
                };

                var claim = new ClaimPopulator().PopulateClaim(atr);
                Assert.IsNull(claim.DamagedItem);
                Assert.IsNull(claim.DamageLocation);
                Assert.IsNull(claim.TypeOfDamage);
                Assert.AreEqual(new DateTime(DateTime.Now.Year, i, 6), claim.DateOfDamage);
            }
        }

        private string getMonth(int numberOfMonth)
        {
            switch (numberOfMonth)
            {
                case 1:
                    return "January";
                case 2:
                    return "feb";
                case 3:
                    return "march";
                case 4:
                    return "Apr";
                case 5:
                    return "may";
                case 6:
                    return "June";
                case 7:
                    return "july";
                case 8:
                    return "aug";
                case 9:
                    return "Sep";
                case 10:
                    return "oct";
                case 11:
                    return "November";
                case 12:
                    return "december";
                default:
                    return "WRONG";
            }
        }

    }
}
