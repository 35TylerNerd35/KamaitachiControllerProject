using UnityEngine;
using UnityEngine.Events;

public class PromptContainer : MonoBehaviour
{
    [SerializeField] public GameObject activeObj;
    [Space][Space]
    [SerializeField] UnityEvent onConfirm;
    [SerializeField] UnityEvent onCancel;
    [SerializeField] string promptText;
    [Space]
    [SerializeField] UnityEvent Condition;

    public void BeginPrompt()
    {
        if (Condition.GetPersistentEventCount() > 0)
        {
            Condition.Invoke();
            return;
        }

        IBeginPrompt();
    }

    void IBeginPrompt()
    {
        ErrorController.instance.ShowConfirmation(promptText);
        ErrorController.instance.OnConfirmYes = onConfirm;
        ErrorController.instance.OnConfirmNo = onCancel;
    }

    public void ActiveCondition(bool showPromptWhenActive)
    {
        if (activeObj == null)
        {
            Debug.LogWarning("NO ACTIVE OBJ");
            if (showPromptWhenActive)
            {
                onConfirm.Invoke();
                return;
            }
            else
            {
                IBeginPrompt();
                return;
            }
        }

        if (showPromptWhenActive == activeObj.activeSelf)
            IBeginPrompt();
        else
            onConfirm.Invoke();
    }
}
