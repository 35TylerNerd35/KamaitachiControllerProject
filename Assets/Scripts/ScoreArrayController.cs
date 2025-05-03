using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreArrayController : MonoBehaviour
{
    [Header("Array Items")]
    [SerializeField] GameObject arrayItemPrefab;
    [SerializeField] Transform arrayParent;
    [HideInInspector] public List<GameObject> arrayItems = new();
    [Header("User Inputs")]
    [SerializeField] TMP_Dropdown difficultyDropdown;
    [SerializeField] TMP_Dropdown lampDropdown;
    [SerializeField] TMP_InputField songNameInput;
    [SerializeField] TMP_InputField scoreInput;
    [Space][Space]
    [SerializeField] GameObject unsavedText;
    [SerializeField] GameObject scoreUnselectedBlocker;

    
    ScoreItemHandle currentSelection;

    public void AddScore()
    {
        // Error if too many scores
        if (arrayItems.Count >= 25)
        {
            ErrorController.instance.ShowError("413 Payload Too Large", "Maximum score entries reached. Please send further scores separately.");
            return;
        }

        // Create new item
        GameObject newItem = Instantiate(arrayItemPrefab, arrayParent);
        arrayItems.Add(newItem);

        // Grab Handle from object
        ScoreItemHandle itemHandle = newItem.GetComponent<ScoreItemHandle>();

        // Set vars
        itemHandle.difficultyDropdown = difficultyDropdown;
        itemHandle.lampDropdown = lampDropdown;
        itemHandle.songNameInput = songNameInput;
        itemHandle.scoreInput = scoreInput;
        itemHandle.unsavedText = unsavedText;
        
        // Update item
        itemHandle.scoreArrayController = this;
        itemHandle.UpdateItem(arrayItems.Count);
        itemHandle.DisplayItem();
        scoreUnselectedBlocker.SetActive(false);
    }

    public void RemoveScore()
    {
        ErrorController.instance.ShowConfirmation($"You Wish To Remove Score #{arrayItems.IndexOf(currentSelection.gameObject) + 1}?");
        ErrorController.instance.OnConfirmYes.AddListener(ActualRemoveScore);
    }

    public void ActualRemoveScore()
    {
        // Error if no score to remove
        if (currentSelection == null)
        {
            ErrorController.instance.ShowError("400 Bad Request", "Please Select a Score to Remove it");
            return;
        }

        // Remove element from array and destroy it
        int i_currentSelection = arrayItems.IndexOf(currentSelection.gameObject);
        arrayItems.RemoveAt(i_currentSelection);
        Destroy(currentSelection.gameObject);

        // Reposition and rename every element
        foreach(GameObject item in arrayItems)
        {
            ScoreItemHandle itemHandle = item.GetComponent<ScoreItemHandle>();
            itemHandle.UpdateItem(arrayItems.IndexOf(item)+1);
        }
    
        scoreUnselectedBlocker.SetActive(true);
    }

    public void RemoveAllScores()
    {
        // Error if no score to remove
        if (arrayItems.Count <= 0)
            return;

        foreach(GameObject item in arrayItems)
            Destroy(item);

        arrayItems.Clear();
        unsavedText.SetActive(false);
        scoreUnselectedBlocker.SetActive(true);
    }

    public void SelectItem(ScoreItemHandle current)
    {
        if (currentSelection != null)
        {
            currentSelection.activeIndicator.SetActive(false);
        }

        unsavedText.SetActive(false);
        currentSelection = current;
        currentSelection.activeIndicator.SetActive(true);
        scoreUnselectedBlocker.SetActive(false);
    }

    public void SaveItem()
    {
        if (currentSelection == null) {
            ErrorController.instance.ShowError("400 Bad Request", "You have no scores selected.");
            return;
        }

        unsavedText.SetActive(false);
        currentSelection.SaveItem();
    }
}
