using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UpdateInfo
{
    public string version;
    public string download_url;
}

public class AutoUpdater : MonoBehaviour
{
    public string updateInfoURL = "https://raw.githubusercontent.com/35TylerNerd35/KamaitachiController/main/update.json";
    private string downloadedInstallerPath;
    [SerializeField] GameObject updatePrompt, doesHidePromptButton;

    Color green = new Color(0.09803922f, 0.5294118f, 0.3294118f);
    Color red = new Color(0.509804f, 0.1921569f, 0.2705882f);

    UpdateInfo info;

    void Start()
    {
        SaveSystem.instance.OnLoad.AddListener(SetHideUpdateSwitch);

        if (SaveSystem.instance.saveData.doesHideUpdatePrompt)
            return;

        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad() {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CheckForUpdates());
    }

    public void SwitchDoesHideUpdatePrompt()
    {
        // Invert bool
        bool doesHide = !SaveSystem.instance.saveData.doesHideUpdatePrompt;
        SaveSystem.instance.saveData.doesHideUpdatePrompt = doesHide;

        SetHideUpdateSwitch();
        SaveSystem.instance.SaveData();
    }

    public void SetHideUpdateSwitch() {
        doesHidePromptButton.GetComponent<Image>().color = SaveSystem.instance.saveData.doesHideUpdatePrompt ? red : green;
        doesHidePromptButton.transform.GetChild(0).GetComponent<TMP_Text>().text = SaveSystem.instance.saveData.doesHideUpdatePrompt ? "Not Checking For Updates" : "Checking For Updates";
    }

    public void CheckUpdates() {
        StartCoroutine(CheckForUpdates());
    }

    IEnumerator CheckForUpdates()
    {
        // Send request to server
        UnityWebRequest request = UnityWebRequest.Get(updateInfoURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Grab current and recent version
            info = JsonUtility.FromJson<UpdateInfo>(request.downloadHandler.text);
            string currentVersion = Application.version;

            // Alert player to available update
            if (currentVersion == info.version)
                yield break;

            // Alert player to available update
            updatePrompt.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Failed to fetch update info: " + request.error);
        }
    }

    public void BeginUpdate()
    {
        StartCoroutine(DownloadUpdate(info.download_url));
    }

    IEnumerator DownloadUpdate(string downloadUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);

        // Assign install location
        string fileName = Path.GetFileName(downloadUrl);
        downloadedInstallerPath = Path.GetFullPath("./" + fileName);

        // Download from server
        request.downloadHandler = new DownloadHandlerFile(downloadedInstallerPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            StartInstaller();
        }
        else
        {
            Debug.LogWarning("Failed to download update: " + request.error);
        }
    }

    void StartInstaller()
    {
        string batchScriptPath = Path.Combine(Application.persistentDataPath, "update.bat");

    #if UNITY_STANDALONE_WIN
        BeginBatchInstall(batchScriptPath);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(batchScriptPath) { UseShellExecute = true });
    #elif UNITY_STANDALONE_OSX
        BeginBatchInstall(batchScriptPath);
        Process.Start("open", batchScriptPath);
    #else
        Debug.LogWarning("Auto-launching installer not supported on this platform.");
    #endif

        // Close game to allow installer to run
        Application.Quit();
    }

    void BeginBatchInstall(string path)
    {
        // Create batch file
        string batchContents = "@echo off\n" +
            "timeout /t 1 /nobreak\n" +
            $"powershell -Command \"Expand-Archive -Path '{downloadedInstallerPath}' -DestinationPath '{Application.dataPath}/..' -Force\"\n" +
            "start \"\" \"KamaitachiController.exe\"\n" +
            "exit";

        // Write batch file
        File.WriteAllText(path, batchContents);
    }

}
