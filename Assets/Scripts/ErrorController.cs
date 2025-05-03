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
    public UnityEvent OnConfirmYes, OnConfirmNo;

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

    public void SubscribeQuitToConfirmation() {
        OnConfirmYes.AddListener(QuitGame);
    }

    void QuitGame() {
        Application.Quit();
    }
}
