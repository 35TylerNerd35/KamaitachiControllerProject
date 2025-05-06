using UnityEngine;
using TMPro;
using System;
using System.Globalization;

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
    public TMP_InputField songNameInput, scoreInput, dateInput, timeInput;
    public GameObject unsavedText;
    public TMP_InputField perfects, greats, goods, misses;

    // Saved vars
    int _difficulty, _lamp;
    float _scoreInputFloat;
    string _songName;
    string _timeInput = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00");
    string _dateInput = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
    int _perfects, _greats, _goods, _misses;

    public void UpdateItem(int count)
    {
        // Set name
        string sCount = count < 10 ? "0" + count : count.ToString();
        itemLabel.text = $"Score {sCount}";

        // Position element
        Vector3 tempPos = new(0, -40, 0);
        float offset = -35 * (count-1);
        tempPos.y += offset;
        transform.localPosition = tempPos;
    }

    public void SaveItem()
    {
        // Attempt to parse score input
        bool didParse = float.TryParse(scoreInput.text, out float scoreInputInt);

        if (!didParse) {
            ErrorController.instance.ShowError("400 Bad Request", "Unable to parse score input.");
            return;
        }

        if (!CouldParseDate(out long unixTime)){
            ErrorController.instance.ShowError("400 Bad Request", $"Unable to parse date input: {dateInput.text + "-" + timeInput.text}");
            return;
        }

        if (!CouldParseJudgements()){
            ErrorController.instance.ShowError("400 Bad Request", "Unable to parse judgement input.");
            return;
        }

        // Save vars
        _difficulty = difficultyDropdown.value;
        _lamp = lampDropdown.value;
        _scoreInputFloat = scoreInputInt;
        _songName = songNameInput.text;

        // Grab judgements
        Record judgements = new Record {
            perfect = _perfects,
            great = _greats,
            good = _goods,
            miss = _misses
        };

        // Set payloads
        pPayload = new PercentPayload {
            percent = _scoreInputFloat,
            lamp = lampDropdown.options[_lamp].text,
            matchType = "songTitle",
            identifier = _songName,
            difficulty = difficultyDropdown.options[_difficulty].text,
            judgements = judgements,
            timeAchieved = unixTime
        };

        sPayload = new ScorePayload {
            score = (int)_scoreInputFloat,
            lamp = lampDropdown.options[_lamp].text,
            matchType = "songTitle",
            identifier = _songName,
            difficulty = difficultyDropdown.options[_difficulty].text,
            judgements = judgements,
            timeAchieved = unixTime
        };

        unsavedText.SetActive(false);
    }

    bool CouldParseDate(out long unixTime)
    {
        // Assign values
        string dateAndTime = dateInput.text + "-" + timeInput.text;
        unixTime = 0;

        // Save dateTime values
        _timeInput = timeInput.text;
        _dateInput = dateInput.text;

        try
        {
            // Parse string into datetime
            DateTime dt = DateTime.ParseExact(dateAndTime, "dd/MM/yyyy-HH:mm", CultureInfo.InvariantCulture);

            // Convert to offset
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            DateTimeOffset dto = dt;
            dto.ToUniversalTime();

            // Convert to Unix
            unixTime = dto.ToUnixTimeMilliseconds();

            return true;
        }
        catch(Exception e)
        {
            Debug.LogWarning(e.ToString());
            return false;
        }
    }

    bool CouldParseJudgements()
    {
        bool _perfect = int.TryParse(perfects.text, out int perfect);
        bool _great = int.TryParse(greats.text, out int great);
        bool _good = int.TryParse(goods.text, out int good);
        bool _miss = int.TryParse(misses.text, out int miss);

        if (!_perfect || !_great || !_good || !_miss)
            return false;

        _perfects = perfect;
        _greats = great;
        _goods = good;
        _misses = miss;

        return true;
    }


    public void DisplayItem()
    {
        scoreArrayController.SelectItem(this);

        // Loads vars
        difficultyDropdown.value = _difficulty;
        lampDropdown.value = _lamp;
        songNameInput.text = _songName;
        scoreInput.text = _scoreInputFloat.ToString();
        timeInput.text = _timeInput;
        dateInput.text = _dateInput.ToString();
        perfects.text = _perfects.ToString();
        greats.text = _greats.ToString();
        goods.text = _goods.ToString();
        misses.text = _misses.ToString();


        SaveItem();
    }
}
