using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Cloud.Language.V1;

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
                if (entity.Type == Entity.Types.Type.ConsumerGood)
                {
                    claim.DamagedItem = entity.Name;
                }

                if (entity.Type == Entity.Types.Type.Location)
                {
                    claim.DamageLocation = entity.Name;
                }
            }

            return claim;
        }

        private void PopulateFromTokens(AnnotateTextResponse response, Claim claim)
        {
            foreach (var token in response.Tokens.Where(token => token.PartOfSpeech.Tag == PartOfSpeech.Types.Tag.Verb))
            {
                AnalyseVerbForDamage(token, claim);
            }

            foreach (var token in response.Tokens.Where(token => token.PartOfSpeech.Tag == PartOfSpeech.Types.Tag.Noun))
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
            foreach (var sentence in response.Sentences)
            {
                CheckForLastDayOfWeek(sentence, claim);
                CheckForWellFormattedDate(sentence, claim);
                CheckForDateOfMonth(sentence, claim);
            }
        }

        private static void CheckForDateOfMonth(Sentence sentence, Claim claim)
        {
            var lastX = new Regex(@"(\d\d?)\w* of (\w+)");
            if (!lastX.IsMatch(sentence.Text.Content)) return;

            var match = lastX.Match(sentence.Text.Content);
            var date = int.Parse(match.Groups[1].Value);
            var monthStr = match.Groups[2].Value;

            int month;
            switch(monthStr.ToLower())
            {
                case "january":
                case "jan":
                    month = 1;
                    break;
                case "february":
                case "feb":
                    month = 2;
                    break;
                case "march":
                case "mar":
                    month = 3;
                    break;
                case "april":
                case "apr":
                    month = 4;
                    break;
                case "may":
                    month = 5;
                    break;
                case "june":
                case "jun":
                    month = 6;
                    break;
                case "july":
                case "jul":
                    month = 7;
                    break;
                case "august":
                case "aug":
                    month = 8;
                    break;
                case "september":
                case "sep":
                    month = 9;
                    break;
                case "october":
                case "oct":
                    month = 10;
                    break;
                case "november":
                case "nob":
                    month = 11;
                    break;
                case "december":
                case "dec":
                    month = 12;
                    break;
                default:
                    return;
            }
            claim.DateOfDamage = new DateTime(DateTime.Now.Year, month, date);
        }

        private static void CheckForLastDayOfWeek(Sentence sentence, Claim claim)
        {
            var lastX = new Regex(@"[lL]ast (\w+day)");
            if (!lastX.IsMatch(sentence.Text.Content)) return;

            var match = lastX.Match(sentence.Text.Content);
            var dayStr = match.Groups[1].Value;
            DayOfWeek dayOfWeek;
            var date = DateTime.Today.AddDays(-1);
            if (Enum.TryParse<DayOfWeek>(dayStr, out dayOfWeek))
            {
                while (dayOfWeek != date.DayOfWeek)
                {
                    date = date.AddDays(-1);
                }
                claim.DateOfDamage = date;
            }
        }

        private static void CheckForWellFormattedDate(Sentence sentence, Claim claim)
        {
            var lastX = new Regex(@"(\d\d?)[-/\\](\d\d?)[-/\\](\d\d\d?\d?)");
            if (!lastX.IsMatch(sentence.Text.Content)) return;

            var match = lastX.Match(sentence.Text.Content);
            var dayStr = int.Parse(match.Groups[1].Value);
            var monthStr = int.Parse(match.Groups[2].Value);
            var yearStr = int.Parse(match.Groups[3].Value);

            //16 means 2016 if we're in the 2XXX's...
            var yearTruncatedToThousand = (DateTime.Now.Year - (DateTime.Now.Year % 1000));
            if (yearStr < yearTruncatedToThousand)
            {
                yearStr = yearStr + yearTruncatedToThousand;
            }
            claim.DateOfDamage = new DateTime(yearStr, monthStr, dayStr);
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