using System.Collections.Generic;
using System.Linq;
using Google.Apis.CloudNaturalLanguage.v1.Data;

namespace ClaimsMapping
{
    public class ClaimPopulator
    {
        public Claim PopulateClaim(AnnotateTextResponse response)
        {
            var claim = PopulateClaim(response.Entities);

            PopulateFromTokens(response, claim);

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
        }

        private void AnalyseVerbForDamage(Token token, Claim claim)
        {
            var tokenTextLower = token.Text.Content.ToLower();

            if (_damagedVerbsLower.Any(s => tokenTextLower.StartsWith(s)))
            {
                claim.TypeOfDamage = Claim.DamageType.Damaged;
            }
            else if (_lostVerbsLower.Any(s => tokenTextLower.StartsWith(s)))
            {
                claim.TypeOfDamage = Claim.DamageType.Lost;
            }
            else if (_stolenVerbsLower.Any(s => tokenTextLower.StartsWith(s)))
            {
                claim.TypeOfDamage = Claim.DamageType.Stolen;
            }
        }

        private readonly List<string> _damagedVerbsLower = new List<string>()
        {
            "damage",
            "crack",
            "smash",
            "destroy",
            "broke"
        };

        private readonly List<string> _lostVerbsLower = new List<string>()
        {
            "lose",
            "lost",
            "gone"
        };

        private readonly List<string> _stolenVerbsLower = new List<string>()
        {
            "take",
            "stole",
            "took",
            "stole",
            "nick",
            "pinch"
        };
    }
}