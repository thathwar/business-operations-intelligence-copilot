using OpenAI;
using OpenAI.Chat;

namespace BusinessOperations.Api.Application.AI
{
    public class OpenAiService
    {
        private readonly ChatClient _chatClient;

        public OpenAiService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            var model = configuration["OpenAI:Model"];

            var client = new OpenAIClient(apiKey);
            _chatClient = client.GetChatClient(model!);
        }

        public async Task<string> AnalyzeIncidentAsync(string prompt)
        {
            var response = await _chatClient.CompleteChatAsync(prompt);

            return response.Value.Content[0].Text;
        }
    }
}
