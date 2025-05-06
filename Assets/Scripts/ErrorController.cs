using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ErrorController : MonoBehaviour
{
    public static ErrorController instance;

    [Header("Errors")]
    [SerializeField] GameObject errorPopupGraphic;
    [SerializeField] TMP_Text errorPopupCode, errorPopupDescription;

    [Header("Confirmations")]
    [SerializeField] GameObject confirmationPopupGraphic;
    [SerializeField] TMP_Text confirmationPopupDescription;
    [HideInInspector] public UnityEvent OnConfirmYes, OnConfirmNo;

    [Header("Information")]
    [SerializeField] GameObject infoPopupGraphic;
    [SerializeField] TMP_Text infoPopupDescription;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }   
    }

    public void ShowError(string code, string description)
    {
        errorPopupCode.text = code;
        errorPopupDescription.text = description;
        errorPopupGraphic.SetActive(true);
    }

    public void HideErrorPopup()
    {
        errorPopupGraphic.SetActive(false);
    }
    
    
    public void ShowConfirmation(string description)
    {
        confirmationPopupDescription.text = description;
        confirmationPopupGraphic.SetActive(true);
    }

    public void ConfirmationYes()
    {
        OnConfirmYes?.Invoke();
        ResetConfirmation();
    }

    public void ConfirmationNo()
    {
        OnConfirmNo?.Invoke();
        ResetConfirmation();
    }

    public void ResetConfirmation()
    {
        // Hide popup
        confirmationPopupGraphic.SetActive(false);

        // Reset events
        OnConfirmYes.RemoveAllListeners();
        OnConfirmNo.RemoveAllListeners();
    }

    public void ShowInfo(string description)
    {
        infoPopupDescription.text = description;
        infoPopupGraphic.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
