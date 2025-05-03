using UnityEngine;

public class InitialSetup : MonoBehaviour
{
    [SerializeField] GameObject[] openOnStart;
    [SerializeField] GameObject[] closeOnStart;

    void Awake()
    {
        foreach(GameObject obj in openOnStart)
            obj.SetActive(true);

        foreach(GameObject obj in closeOnStart)
            obj.SetActive(false);
        
    }
}
