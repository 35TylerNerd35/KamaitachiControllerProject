using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.IO.Compression;
using TMPro;
using System.Diagnostics;

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
    public TMP_Text testText;

    void Start()
    {
        testText.text = "Checking for updates...";
        StartCoroutine(CheckForUpdates());
    }

    IEnumerator CheckForUpdates()
    {
        UnityWebRequest request = UnityWebRequest.Get(updateInfoURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            UpdateInfo info = JsonUtility.FromJson<UpdateInfo>(request.downloadHandler.text);
            string currentVersion = Application.version;

            // Debug.Log("Current Version: " + currentVersion);
            // Debug.Log("Latest Version: " + info.version);

            if (currentVersion != info.version)
            {
                // Debug.Log("Update available! Starting download...");
                testText.text = "Update available! Starting download...";
                StartCoroutine(DownloadUpdate(info.download_url));
            }
            else
            {
                testText.text = "Game is up to date.";
            }
        }
        else
        {
            testText.text = "Failed to fetch update info: " + request.error;
        }
    }

    IEnumerator DownloadUpdate(string downloadUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);

        // Save to persistent data path
        string fileName = Path.GetFileName(downloadUrl);
        downloadedInstallerPath = Path.GetFullPath("./" + fileName);

        request.downloadHandler = new DownloadHandlerFile(downloadedInstallerPath);
        testText.text = "Downloading from... " + downloadUrl + " to " + downloadedInstallerPath;
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            testText.text = "Download complete! Extracting" + downloadedInstallerPath;

            // ExtractZip(downloadedInstallerPath, Path.GetFullPath("./"));

            testText.text = "Extraction complete!";

            // Launch installer (only works on PC/Mac builds)
            StartInstaller();
        }
        else
        {
            // Debug.LogError("Failed to download update: " + request.error);
            testText.text = "Failed to download update: " + request.error;
        }
    }

    void StartInstaller()
    {
#if UNITY_STANDALONE_WIN
        string batchScriptPath = Path.Combine(Application.persistentDataPath, "update.bat");

        string batchContents = "@echo off\n" +
            "timeout /t 1 /nobreak\n" +
            $"powershell -Command \"Expand-Archive -Path '{downloadedInstallerPath}' -DestinationPath '{Application.dataPath}/..' -Force\"\n" +
            "start \"\" \"KamatachiController.exe\"\n" +
            "exit";

        File.WriteAllText(batchScriptPath, batchContents);

        Process.Start(new ProcessStartInfo(batchScriptPath) { UseShellExecute = true });
        Application.Quit(); // Close the game to allow installer to run
#elif UNITY_STANDALONE_OSX
        Process.Start("open", downloadedInstallerPath);
        Application.Quit();
#else
        Debug.LogWarning("Auto-launching installer not supported on this platform.");
#endif
    }

    void ExtractZip(string zipPath, string extractPath)
    {
        if (Directory.Exists(extractPath) == false)
        {
            Directory.CreateDirectory(extractPath);
        }
        ZipFile.ExtractToDirectory(zipPath, extractPath, true); // Overwrite existing files
    }

}
