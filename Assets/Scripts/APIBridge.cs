using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class APIBridge : MonoBehaviour
{
    public static APIBridge instance;

    public string KamaitachiAPIKey;
    readonly HttpClient kClient = new HttpClient();

    public string SheetsAPIKey;
    readonly HttpClient sClient = new HttpClient();

    // Establish instance
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            kClient.DefaultRequestHeaders.Add("X-User-Intent", "true");
        }
        else
        {
            Destroy(this);
        }   
    }

    public void SetNewKClientAuth(string newKey)
    {
        KamaitachiAPIKey = newKey;

        // Set auth header
        kClient.DefaultRequestHeaders.Remove("Authorization");
        kClient.DefaultRequestHeaders.Add("Authorization","Bearer " + newKey);
    }

    public async Task<R> SendPostRequest<P, R>(string url, P payload, bool _isKClient = true) where R : APIResponse, new()
    {
        // Read the JSON file
        var stringPayload = JsonConvert.SerializeObject(payload);
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

        // Send the request and read the response
        var response = await (_isKClient ? kClient : sClient).PostAsync(url, httpContent);
        string responseString = await response.Content.ReadAsStringAsync();

        // Throw error code if failed
        if (!responseString.CheckAuthSuccess())
            return new R();

        // Convert to requested response type
        R jsonObj  = JsonConvert.DeserializeObject<R>(responseString);
        return jsonObj;
    }

    public async Task<R> SendGetRequest<R>(string url, bool _isKClient = true) where R : APIResponse, new()
    {
        // Send the request and read the response
        HttpResponseMessage response = await (_isKClient ? kClient : sClient).GetAsync(url);
        string responseString = await response.Content.ReadAsStringAsync();

        // Throw error code if failed
        if (!responseString.CheckAuthSuccess())
            return new R();

        // Convert to requested response type
        R jsonObj  = JsonConvert.DeserializeObject<R>(responseString);
        return jsonObj;
    }
}
