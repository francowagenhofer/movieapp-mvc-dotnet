
namespace app_movie_mvc.Service
{
    internal class OpenAIResponseClient
    {
        private string model;
        private string apiKey;

        public OpenAIResponseClient(string model, string apiKey)
        {
            this.model = model;
            this.apiKey = apiKey;
        }

        internal async Task<OpenAIResponse> CreateResponseAsync(string prompt)
        {
            throw new NotImplementedException();
        }
    }
}