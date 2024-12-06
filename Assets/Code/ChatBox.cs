using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using Newtonsoft.Json;  // Import the Newtonsoft.Json namespace

public class ChatBox : MonoBehaviour
{
    public TMP_InputField userInputField;
    public Button sendButton;
    public Transform chatContent;  // The Content area of the ScrollView (not a Text object)
    public GameObject messagePrefab;
    public ScrollRect scrollRect;

    private string apiUrl = "https://api.openai.com/v1/chat/completions"; // Correct API URL for chat completions
    private string apiKey = ""; // Your OpenAI API key

    void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    private void OnSendButtonClicked()
    {
        string userInput = userInputField.text;
        if (!string.IsNullOrEmpty(userInput))
        {
            AppendMessage($"User: {userInput}");
            userInputField.text = "";
            StartCoroutine(GetAIResponse(userInput));
        }
    }

    private IEnumerator GetAIResponse(string prompt)
    {
        AppendMessage("Assistant: Typing...");
        yield return new WaitForSeconds(1); // Simulate typing delay

        Debug.Log("Fetching AI response...");

        // Call the asynchronous function to get the response
        Task<string> task = GetAIResponseAsync(prompt);
        yield return new WaitUntil(() => task.IsCompleted);  // Wait until the task is finished

        if (task.Exception != null)
        {
            Debug.LogError("Exception during API request: " + task.Exception);
            AppendMessage("Assistant: Error retrieving response.");
        }
        else
        {
            string response = task.Result;

            if (string.IsNullOrEmpty(response))
            {
                AppendMessage("Assistant: No response received.");
            }
            else
            {
                AppendMessage($"Assistant: {response}");
            }
        }
    }

    private async Task<string> GetAIResponseAsync(string prompt)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Create the message object to send
            var messages = new List<object>
            {
                new { role = "user", content = prompt }
            };

            var jsonRequestBody = new
            {
                model = "gpt-3.5-turbo", // The model you're using
                messages = messages,
                max_tokens = 100,
                temperature = 0.5
            };

            // Serialize the JSON request body using Newtonsoft.Json
            var content = new StringContent(JsonConvert.SerializeObject(jsonRequestBody), System.Text.Encoding.UTF8, "application/json");

            try
            {
                var result = await client.PostAsync(apiUrl, content);
                if (!result.IsSuccessStatusCode)
                {
                    Debug.LogError($"API call failed with status code {result.StatusCode}");
                    return "Error: Failed to fetch response.";
                }

                string response = await result.Content.ReadAsStringAsync();
                Debug.Log($"Response: {response}");

                // Deserialize the response JSON using Newtonsoft.Json
                var jsonResponse = JsonConvert.DeserializeObject<OpenAIResponse>(response);
                if (jsonResponse != null && jsonResponse.choices.Length > 0)
                {
                    return jsonResponse.choices[0].message.content.Trim();
                }
                else
                {
                    Debug.LogError("Invalid response format or no choices found.");
                    return "No response from Assistant.";
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception during API call: {ex.Message}");
                return "Error: Exception during API call.";
            }
        }
    }

    private void AppendMessage(string message)
    {
        Debug.Log($"Appending message: {message}");

        // Instantiate a new message prefab (TextMeshProUGUI or any UI element)
        GameObject newMessage = Instantiate(messagePrefab, chatContent);

        // Get the TextMeshProUGUI component from the newly instantiated prefab
        TextMeshProUGUI messageText = newMessage.GetComponent<TextMeshProUGUI>();

        if (messageText != null)
        {
            // Set the message text
            messageText.text = message;
        }
        else
        {
            Debug.LogError("Message prefab does not have a TextMeshProUGUI component!");
        }

        // Scroll to the bottom
        Canvas.ForceUpdateCanvases();
        RectTransform contentRect = chatContent.GetComponent<RectTransform>();
        contentRect.anchoredPosition = new Vector2(0, 0);  // Scroll to the bottom
        scrollRect.verticalNormalizedPosition = 0f;  // Make sure the scroll view scrolls to the bottom
    }

    [System.Serializable]
    public class OpenAIResponse
    {
        public Choice[] choices;

        [System.Serializable]
        public class Choice
        {
            public Message message;

            [System.Serializable]
            public class Message
            {
                public string content;
            }
        }
    }
}
