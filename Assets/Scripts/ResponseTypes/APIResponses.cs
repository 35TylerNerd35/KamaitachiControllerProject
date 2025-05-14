using System.Diagnostics;
using Newtonsoft.Json;

[System.Serializable]
public class APIResponse
{
    public bool success;
    public string description;
}

// Children
public class StatusResponse : APIResponse { public StatusBody body; }
public class UserResponse : APIResponse { public UserBody body; }
public class ImportResponse : APIResponse { public ImportBody body; }
public class SearchResponse : APIResponse {public SearchBody body;}


public static class CheckErrors
{
    public static bool CheckAuthSuccess(this string responseString)
    {
        // Deserialize into class
        APIResponse response  = JsonConvert.DeserializeObject<APIResponse>(responseString);

        // Throw error code if failed
        if (response.success == false)
        {
            ErrorController.instance.ShowError("401 Unauthorized", "Authorization from API code failed. Please ensure it has been properly set.");

            #if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(response.description);
            #endif

            return false;
        }
        return true;
    }
}