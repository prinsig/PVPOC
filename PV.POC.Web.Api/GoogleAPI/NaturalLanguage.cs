using Google.Cloud.Language.V1;

namespace PV.POC.WEB.API.GoogleAPI
{
    public static class NaturalLanguage
    {
        /// <summary>
        /// language.documents.analyzeEntities
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static AnalyzeEntitiesResponse AnalyzeEntitiesFromText(string text)
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
        public static AnnotateTextResponse AnalyzeEverything(string text)
        {
            var client = LanguageServiceClient.Create();

            if(client != null)
            {
                return client.AnnotateText(new Document()
                {
                    Content = text,
                    Type = Document.Types.Type.PlainText
                },
                new AnnotateTextRequest.Types.Features()
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