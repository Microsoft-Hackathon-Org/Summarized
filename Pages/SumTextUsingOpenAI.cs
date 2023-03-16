using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

// use openai to do text summarization

namespace SumTextUsingOpenAI
{
    public class OpenAITextSummarization
    {
        private readonly string _openAIKey;
        private readonly string _modelEngine;

        public OpenAITextSummarization(string openAIKey, string modelEngine)
        {
            _openAIKey = openAIKey;
            _modelEngine = modelEngine;
        }

        public string SummarizeText(string textToSummarize)
        {
            // Create HTTP client and set up request headers
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAIKey}");
                httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                // Define request parameters and data
                string apiUrl = "https://api.openai.com/v1/engines/" + _modelEngine + "/completions";
                JObject requestData = new JObject(
                    new JProperty("prompt", textToSummarize),
                    new JProperty("temperature", 0.5),
                    new JProperty("max_tokens", 60),
                    new JProperty("top_p", 1),
                    new JProperty("frequency_penalty", 0),
                    new JProperty("presence_penalty", 0)
                );
                string requestBody = requestData.ToString();

                // Send request to OpenAI API
                HttpResponseMessage response = httpClient.PostAsync(apiUrl, new StringContent(requestBody, Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"OpenAI API request failed: {response.ReasonPhrase}");
                }

                // Parse response data and return summarized text
                JObject responseData = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                string summarizedText = responseData["choices"][0]["text"].ToString().Trim();
                // string? summarizedText = responseData?["choices"]?[0]?["text"]?.ToString()?.Trim();
                summarizedText = Regex.Replace(summarizedText, @"\s+", " "); // remove extra whitespace
                return summarizedText;
            }
        }
    }
}
