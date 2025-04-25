using UnityEngine;
using TMPro;
using Markdig;
using System.IO;

public class MarkdownDisplay : MonoBehaviour
{
    TMP_Text outputText;
    [SerializeField] GameObject releaseNotesPopup;

    void Start()
    {
        outputText = GetComponent<TMP_Text>();
        string releaseNotesPath = Path.GetFullPath("ReleaseNotes.md");
        
        if(!File.Exists(releaseNotesPath))
        {
            releaseNotesPopup.SetActive(false);
            return;
        }

        // Convery string
        string releaseNotesText = File.ReadAllText(releaseNotesPath);
        string releaseNotesHtml = Markdown.ToHtml(releaseNotesText);
        string releaseNotesTmpro = ConvertHtmlToTMPro(releaseNotesHtml);

        outputText.text = releaseNotesTmpro;

        // Delete file
        File.Delete(releaseNotesPath);
    }

    string ConvertHtmlToTMPro(string html)
    {
        return html
            .Replace("<h1>", "<size=200%><b>").Replace("</h1>", "</b></size>\n")
            .Replace("<h2>", "<size=150%><b>").Replace("</h2>", "</b></size>\n")
            .Replace("<h3>", "<size=130%><b>").Replace("</h3>", "</b></size>\n")
            .Replace("<strong>", "<b>").Replace("</strong>", "</b>")
            .Replace("<em>", "<i>").Replace("</em>", "</i>")
            .Replace("<ul>", "").Replace("</ul>", "")
            .Replace("<li>", "• ").Replace("</li>", "\n")
            .Replace("<p>", "").Replace("</p>", "\n\n")
            .Replace("<br>", "\n").Replace("<br/>", "\n")
            .Replace("<a href=\"", "<color=#4AA1FF><u><link=\"")
            .Replace("\">", "\">")
            .Replace("</a>", "</link></u></color>");
    }
}