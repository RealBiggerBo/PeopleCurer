using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    internal static class ChatbotManager
    {
        private static readonly string endpoint = "https://beste-app.openai.azure.com/";
        private static readonly string key = "ff11be9aa42b4ff79e5d93fc331a78aa";

        private static readonly AzureOpenAIClient azureClient = new(new Uri(endpoint), new AzureKeyCredential(key));

        private static readonly ChatClient chatClient = azureClient.GetChatClient("gpt-4o");

        private static readonly List<ChatMessage> chatMessages = [];

        public static async Task<string> Talk(string input)
        {
            try
            {
                chatMessages.Add(ChatMessage.CreateUserMessage(ChatMessageContentPart.CreateTextMessageContentPart(input)));
                ChatCompletion chatCompletion = await chatClient.CompleteChatAsync(chatMessages);
                return chatCompletion.Content[0].Text;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
