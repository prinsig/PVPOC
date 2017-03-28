using Google.Cloud.Language.V1;
using static Google.Cloud.Language.V1.AnnotateTextRequest.Types;

namespace PVpoc.GoogleAPI
{
    public class NaturalLanguage
    {
        /// <summary>
        /// language.documents.analyzeEntities
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public AnalyzeEntitiesResponse AnalyzeEntitiesFromText(string text)
        {           
            var client = LanguageServiceClient.Create();

            if(client != null)
            {
                return client.AnalyzeEntities(new Document()
                {
                    Content = text,
                    Type = Document.Types.Type.PlainText
                });
            }           

            return null;           
        }

        /// <summary>
        /// language.documents.annotateText
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public AnnotateTextResponse AnalyzeEverything(string text)
        {
            var client = LanguageServiceClient.Create();

            if(client != null)
            {
                return client.AnnotateText(new Document()
                {
                    Content = text,
                    Type = Document.Types.Type.PlainText
                },
                new Features()
                {
                    ExtractSyntax = true,
                    ExtractDocumentSentiment = true,
                    ExtractEntities = true,
                });
            }

            return null;                    
        }
    }
}