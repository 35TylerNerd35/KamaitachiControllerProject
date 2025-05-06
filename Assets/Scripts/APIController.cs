using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class APIController : MonoBehaviour
{
    string APIKey;
    string sApiKey;

    int currentGameSelection;

    [Header("Games")]
    [SerializeField] GameContainer[] games;
    [SerializeField] TMP_Dropdown difficultyDropdown, lampDropdown;
    [SerializeField] ScoreArrayController scoreArrayController;

    [Space]
    [Header("Profile")]
    [SerializeField] TMP_InputField apiInput;
    [SerializeField] TMP_Text userText, bioText, profileButtonText;
    [Space]
    [SerializeField] TMP_InputField googleSheetsInput;

    [Header("Game Fields")]
    [SerializeField] GameObject warningText;

    void Start()
    {
        OnGameSelectionChange(0);
        SaveSystem.instance.OnLoad.AddListener(OnLoad);
    }

    public void OnLoad() {
        apiInput.text = SaveSystem.instance.saveData.kamaitachiApiKey;
        googleSheetsInput.text = SaveSystem.instance.saveData.googleSheetsApiKey;
        GrabUser();
    }

    public async void GrabUser() {

        // Grab input API key
        APIKey = apiInput.text;
        sApiKey = googleSheetsInput.text;

        // Send request
        APIBridge.instance.SetNewKClientAuth(APIKey);
        UserResponse jsonObj = await APIBridge.instance.SendGetRequest<UserResponse>("https://kamai.tachi.ac/api/v1/users/me");

        // Display profile elements on UI
        userText.text = jsonObj.body.username + "'s Profile";
        profileButtonText.text = "What's up, " + jsonObj.body.username;
        bioText.text = jsonObj.body.about;

        // Save values
        SaveSystem.instance.saveData.kamaitachiApiKey = APIKey;
        SaveSystem.instance.saveData.googleSheetsApiKey = sApiKey;
        SaveSystem.instance.SaveData();

    }

    public async void SendRequest()
    {
        // Error if no scores
        if (scoreArrayController.arrayItems.Count <= 0)
        {
            ErrorController.instance.ShowError("400 Bad Request", "You have no scores to send.");
            return;
        }

        // Create payload
        Payload[] scorePayload = new Payload[scoreArrayController.arrayItems.Count];

        // Set payload vars appropriately
        for (int i = 0; i < scoreArrayController.arrayItems.Count; i++)
        {
            ScoreItemHandle targetScore = scoreArrayController.arrayItems[i].GetComponent<ScoreItemHandle>();
            scorePayload[i] = games[currentGameSelection].doesUseScore ? targetScore.sPayload : targetScore.pPayload;
        }
        
        // Create container for payload
        PayloadContainer payload = new PayloadContainer { 
            meta = games[currentGameSelection].meta,
            scores = scorePayload
        };

        // Send request
        ImportResponse bridgeResponse = await APIBridge.instance.SendPostRequest<PayloadContainer, ImportResponse>("https://kamai.tachi.ac/ir/direct-manual/import", payload);

        if (!bridgeResponse.success)
        {
            ErrorController.instance.ShowError("400 Bad Request", "Scores Not Succesfully Sent.");
        }
        else
        {
            ErrorController.instance.ShowInfo("Scores Succesfully Sent.");
            scoreArrayController.RemoveAllScores();
        }
    }

    public void OnGameSelectionChange(int newSelection)
    {
        // Set current game
        currentGameSelection = newSelection;
        warningText.SetActive(games[currentGameSelection].doesHaveWarning);

        // Clear difficulties and lamps
        difficultyDropdown.ClearOptions();
        lampDropdown.ClearOptions();
        
        // Convert arrays to lists
        List<string> difficulties = new List<string>(games[currentGameSelection].difficulties);
        List<string> lamps = new List<string>(games[currentGameSelection].lamps);

        // Set dropdown options
        difficultyDropdown.AddOptions(difficulties);
        lampDropdown.AddOptions(lamps);
    }
}
