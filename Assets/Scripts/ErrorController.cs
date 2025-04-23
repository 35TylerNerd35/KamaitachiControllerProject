using UnityEngine;
using TMPro;

public class ErrorController : MonoBehaviour
{
    public static ErrorController instance;
    [SerializeField] GameObject popupGraphic;
    [SerializeField] TMP_Text popupCode, popupDescription;

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
        popupCode.text = code;
        popupDescription.text = description;
        popupGraphic.SetActive(true);
    }
    
    public void HidePoppup() {
        popupGraphic.SetActive(false);
    }
}
