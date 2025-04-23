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

        // Set position
        Vector3 tempPos = newItem.transform.localPosition;
        float offset = -35 * (arrayItems.Count - 1);
        tempPos.y += offset;
        newItem.transform.localPosition = tempPos;

        // Grab Handle from object
        ScoreItemHandle itemHandle = newItem.GetComponent<ScoreItemHandle>();

        // Set vars
        itemHandle.difficultyDropdown = difficultyDropdown;
        itemHandle.lampDropdown = lampDropdown;
        itemHandle.songNameInput = songNameInput;
        itemHandle.scoreInput = scoreInput;

        itemHandle.unsavedText = unsavedText;
        
        itemHandle.scoreArrayController = this;
        itemHandle.UpdateItem(arrayItems.Count);
    }

    public void RemoveScore()
    {
        // Error if no score to remove
        if (arrayItems.Count <= 0)
        {
            ErrorController.instance.ShowError("400 Bad Request", "You have no scores to remove.");
            return;
        }

        // Remove last item
        int lastItem = arrayItems.Count - 1;
        Destroy(arrayItems[lastItem]);
        arrayItems.RemoveAt(lastItem);
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
