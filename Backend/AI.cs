using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Linq;

namespace LieDetectorGame 
{
    class AI 
    {
        const string ApiUrl = "https://api.openai.com/v1/chat/completions";
        const string Model = "gpt-4o";
        // Use environment variable for your API key (runtime)
        static readonly string ApiKey = Environment.GetEnvironmentVariable("LieDetectorAPI");

        static readonly HttpClient Http = new HttpClient();

        // --- Core API Call -------------------------------------------------------------------------------------------
        // Pass in a system prompt and the conversation history, get back a string response.
        public static async Task<string> CallOpenAI(string systemPrompt, List<(string role, string content)> history) {
            // Build messages array
            var messages = new JsonArray{
                new JsonObject { ["role"] = "system", ["content"] = systemPrompt }
            };

                foreach (var (role, content) in history) {
                    messages.Add(new JsonObject { ["role"] = role, ["content"] = content });
                }

                var body = new JsonObject {
                    ["model"] = Model,
                    ["max_token"] = 400,
                    ["messages"] = messages
                };

                var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl) 
                {
                    Content = new StringContent(body.ToJsonString(), Encoding.UTF8, "application/json")
                };

                request.Headers.Add("Authorization", $"Bearer {ApiKey}");

                // Send request
                var response = await Http.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"OpenAI error {(int)response.StatusCode}: {json}");
                
                // Parse and return response content
                var doc = JsonNode.Parse(json);
                return doc?["choices"]?[0]?["message"]?["content"]?.GetValue<string>()?.Trim() ?? "";
            }

            // ---- Multiple Choice Question Logic -------------------------------------------------

            public record MCQuestion(string Question, string[] Options, int SuspicionDelta, string? Note);

            public static async Task<MCQuestion> AskMultipleChoice(
                List<(string role, string content)> history,
                string playerLie,
                int suspicion,
                int questionNumber)
            {
                string systemPrompt = $@"
You are Detective Raymond Voss, interrogating a suspect.
Suspect claim: ""{playerLie}""
Suspicion level: {suspicion}/100
Question number: {questionNumber}

You MUST analyze the FULL conversation history.

Your job:
- Detect contradictions between answers
- Reference specific previous answers when possible
- Build on earlier responses instead of asking random questions
- Focus on weak or inconsistent statements

Rules:
- Ask ONE sharp question
- It MUST relate to something the suspect previously said
- If no contradiction exists yet, deepen the story with details

Multiple Choice:
- Provide EXACTLY 4 options
- One should be consistent with prior answers
- Others should be suspicious, vague, or contradictory


Respond ONLY with valid JSON:
{{
    ""question"": ""Your question"",
    ""options"": [""A"", ""B"", ""C"", ""D""],
    ""suspicionDelta"": <integer from -5 to +25>,
    ""note"": ""Short reasoning or null""
}}";

            try {
                string raw = await CallOpenAI(systemPrompt, history);
                // Clean possible markdown formatting
                raw = raw.Replace("```json", "").Replace("```", "").Trim();

                var node = JsonNode.Parse(raw);

                string questionText = node?["question"]?.GetValue<string>() ?? "Error: No question";
                string[] options = node?["options"]?.AsArray()?.Select(x => x.GetValue<string>()).ToArray() ?? new string[0];

                // Safety: If AI returns empty options, provide defaults
                if (options.Length == 0)
                    options = new string[] { "Option A", "Option B", "Option C", "Option D" };

                int suspicionDelta = node?["suspicionDelta"]?.GetValue<int>() ?? 5;
                string? note = node?["note"]?.GetValue<string>();

                return new MCQuestion (questionText, options, suspicionDelta, note);
            } 
            catch {
                return new MCQuestion("Error parsing AI response", new string[] { "Option A", "Option B", "Option C", "Option D" }, 0, null);
            };
        }    
    }    
}