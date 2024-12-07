using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using Newtonsoft.Json;

public class ChatBox : MonoBehaviour
{
    public TMP_InputField userInputField;
    public Button sendButton;
    public Transform chatContent;  // The Content area of the ScrollView (not a Text object)
    public GameObject messagePrefab;
    public ScrollRect scrollRect;

    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string apiKey = "";
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
        yield return new WaitForSeconds(1);

        Task<string> task = GetAIResponseAsync(prompt);
        yield return new WaitUntil(() => task.IsCompleted);

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

            var messages = new List<object>
            {
                new { role = "user", content = prompt }
            };

            var jsonRequestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = messages,
                max_tokens = 100,
                temperature = 0.5
            };

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
        GameObject newMessage = Instantiate(messagePrefab, chatContent);

        // Get the TextMeshProUGUI component from the prefab
        TextMeshProUGUI messageText = newMessage.GetComponent<TextMeshProUGUI>();

        if (messageText != null)
        {
            messageText.text = message;

            // Optional: Adjust the RectTransform for spacing
            RectTransform rectTransform = newMessage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 50); // Adjust height
            }
        }
        else
        {
            Debug.LogError("Message prefab does not have a TextMeshProUGUI component!");
        }

        // Force layout rebuild and scroll to the bottom
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;  // Ensure the scroll stays at the bottom
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