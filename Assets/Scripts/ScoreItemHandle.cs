using UnityEngine;
using TMPro;
using System;

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
    public TMP_InputField songNameInput, scoreInput, hourInput, dateInput, monthInput, yearInput;
    public GameObject unsavedText;
    public TMP_InputField perfects, greats, goods, misses;

    // Saved vars
    int _difficulty, _lamp;
    float _scoreInputFloat;
    string _songName;
    int _hourInput = DateTime.Now.Hour, _dateInput = DateTime.Now.Day, _monthInput = DateTime.Now.Month, _yearInput = DateTime.Now.Year;
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

        if (!CouldParseDate()){
            ErrorController.instance.ShowError("400 Bad Request", "Unable to parse date input.");
            return;
        }

        if (!CouldConvertToUnix(out long unixTime, out string error)){
            ErrorController.instance.ShowError("400 Bad Request", "Unable to convert date input to UNIX.");
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

    bool CouldParseDate()
    {
        // Parse all inputs
        bool _hour = int.TryParse(hourInput.text, out int hour);
        bool _date = int.TryParse(dateInput.text, out int date);
        bool _month = int.TryParse(monthInput.text, out int month);
        bool _year = int.TryParse(yearInput.text, out int year);

        if (!_hour || !_date || !_month || !_year)
            return false;

        // Save vars
        _hourInput = hour;
        _dateInput = date;
        _monthInput = month;
        _yearInput = year;

        return true;
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

    bool CouldConvertToUnix(out long unixTime, out string error)
    {
        // Declare out vars
        unixTime = 0;
        error = "";

        try
        {
            // Convert to DateTime
            DateTimeOffset dt = new DateTimeOffset(_yearInput, _monthInput, _dateInput, _hourInput, 0, 0, TimeSpan.Zero);
            dt.ToUniversalTime();
            unixTime = dt.ToUnixTimeMilliseconds();
        }
        catch (Exception e)
        {
            error = e.ToString();
            return false;
        }

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
        hourInput.text = _hourInput.ToString();
        dateInput.text = _dateInput.ToString();
        monthInput.text = _monthInput.ToString();
        yearInput.text = _yearInput.ToString();
        perfects.text = _perfects.ToString();
        greats.text = _greats.ToString();
        goods.text = _goods.ToString();
        misses.text = _misses.ToString();


        SaveItem();
    }
}
