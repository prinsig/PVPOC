
using Google.Cloud.Language.V1;

namespace PVpoc.GoogleAPI
{
    public class NaturalLanguage
    {
        public void AnalyzeEntitiesFromText(string text)
        {
            var client = LanguageServiceClient.Create();
            var response = client.AnalyzeEntities(new Document()
            {
                Content = text,
                Type = Document.Types.Type.PlainText
            });            
        }
    }
}