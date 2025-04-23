using UnityEngine;
using TMPro;

public class ScoreItemHandle : MonoBehaviour
{
    [SerializeField] public PercentPayload pPayload;
    [SerializeField] public ScorePayload sPayload;

    [Space][Space][Space]

    [SerializeField] TMP_Text itemLabel;
    [SerializeField] public GameObject activeIndicator;

    [Space][Space][Space]

    public ScoreArrayController scoreArrayController;
    public TMP_Dropdown difficultyDropdown, lampDropdown;
    public TMP_InputField songNameInput, scoreInput;
    public GameObject unsavedText;

    // Saved vars
    int _difficulty, _lamp;
    float _scoreInputFloat;
    string _songName;

    public void UpdateItem(int count)
    {
        // Set name
        string sCount = count < 10 ? "0" + count : count.ToString();
        itemLabel.text = $"Score {sCount}";

        // Set as current focussed
        DisplayItem();
    }

    public void SaveItem()
    {
        // Attempt to parse score input
        bool didParse = float.TryParse(scoreInput.text, out float scoreInputInt);

        if (!didParse) {
            ErrorController.instance.ShowError("400 Bad Request", "Unable to parse score input.");
            return;
        }

        // Save vars
        _difficulty = difficultyDropdown.value;
        _lamp = lampDropdown.value;
        _scoreInputFloat = scoreInputInt;
        _songName = songNameInput.text;

        // Set payloads
        pPayload = new PercentPayload {
            percent = _scoreInputFloat,
            lamp = lampDropdown.options[_lamp].text,
            matchType = "songTitle",
            identifier = _songName,
            difficulty = difficultyDropdown.options[_difficulty].text,
            timeAchieved = 1624324467489
        };

        sPayload = new ScorePayload {
            score = (int)_scoreInputFloat,
            lamp = lampDropdown.options[_lamp].text,
            matchType = "songTitle",
            identifier = _songName,
            difficulty = difficultyDropdown.options[_difficulty].text,
            timeAchieved = 1624324467489
        };

        unsavedText.SetActive(false);
    }

    public void DisplayItem()
    {
        scoreArrayController.SelectItem(this);

        // Loads vars
        difficultyDropdown.value = _difficulty;
        lampDropdown.value = _lamp;
        songNameInput.text = _songName;
        scoreInput.text = _scoreInputFloat.ToString();

        SaveItem();
    }
}
