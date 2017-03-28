using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Apis.CloudNaturalLanguage.v1.Data;

namespace ClaimsMapping
{
    public class ClaimPopulator
    {
        public Claim PopulateClaim(AnnotateTextResponse response)
        {
            var claim = PopulateClaim(response.Entities);

            PopulateFromTokens(response, claim);

            PopulateFromSentences(response, claim);

            return claim;
        }

        private static Claim PopulateClaim(IEnumerable<Entity> entities)
        {
            var claim = new Claim();

            foreach (var entity in entities)
            {
                if (entity.Type == "CONSUMER_GOOD")
                {
                    claim.DamagedItem = entity.Name;
                }

                if (entity.Type == "LOCATION")
                {
                    claim.DamageLocation = entity.Name;
                }
            }

            return claim;
        }

        private void PopulateFromTokens(AnnotateTextResponse response, Claim claim)
        {
            foreach (var token in response.Tokens.Where(token => token.PartOfSpeech.Tag == "VERB"))
            {
                AnalyseVerbForDamage(token, claim);
            }

            foreach (var token in response.Tokens.Where(token => token.PartOfSpeech.Tag == "NOUN"))
            {
                AnalyseNounForTime(token, claim);
            }
        }

        private void PopulateFromSentences(AnnotateTextResponse response, Claim claim)
        {
            CheckTime(response, claim);
        }

        private static void CheckTime(AnnotateTextResponse response, Claim claim)
        {
            CheckForLastDayOfWeek(response, claim);
        }

        private static void CheckForLastDayOfWeek(AnnotateTextResponse response, Claim claim)
        {
            Regex lastX = new Regex(@"[lL]ast (\w+day)");
            foreach (var sentence in response.Sentences)
            {
                if (lastX.IsMatch(sentence.Text.Content))
                {
                    Match match = lastX.Match(sentence.Text.Content);
                    var dayStr = match.Groups[1].Value;
                    DayOfWeek dayOfWeek;
                    DateTime date = DateTime.Today.AddDays(-1);
                    if (Enum.TryParse<DayOfWeek>(dayStr, out dayOfWeek))
                    {
                        while (dayOfWeek != date.DayOfWeek)
                        {
                            date = date.AddDays(-1);
                        }
                        claim.DateOfDamage = date;
                    }
                }
            }
        }

        private void AnalyseVerbForDamage(Token token, Claim claim)
        {
            var tokenBaseWord = token.Lemma;

            if (_damagedVerbsLower.Any(s => tokenBaseWord.StartsWith(s)))
            {
                claim.TypeOfDamage = Claim.DamageType.Damaged;
            }
            else if (_lostVerbsLower.Any(s => tokenBaseWord.StartsWith(s)))
            {
                claim.TypeOfDamage = Claim.DamageType.Lost;
            }
            else if (_stolenVerbsLower.Any(s => tokenBaseWord.StartsWith(s)))
            {
                claim.TypeOfDamage = Claim.DamageType.Stolen;
            }
        }

        private static void AnalyseNounForTime(Token token, Claim claim)
        {
            var tokenTextLower = token.Text.Content.ToLower();

            if (tokenTextLower == "today")
            {
                claim.DateOfDamage = DateTime.Today;
            }
            else if (tokenTextLower == "yesterday")
            {
                claim.DateOfDamage = DateTime.Today.AddDays(-1);
            }
        }

        private readonly List<string> _damagedVerbsLower = new List<string>()
        {
            "damage",
            "crack",
            "smash",
            "destroy",
            "break"
        };

        private readonly List<string> _lostVerbsLower = new List<string>()
        {
            "lose",
            "gone"
        };

        private readonly List<string> _stolenVerbsLower = new List<string>()
        {
            "take",
            "steal",
            "nick",
            "pinch"
        };
    }
}