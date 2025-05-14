using UnityEngine;

[CreateAssetMenu(fileName = "GameContainer", menuName = "Scriptable Objects/GameContainer")]
public class GameContainer : ScriptableObject
{
    public bool doesUseScore;
    public Meta meta;

    [Space][Space][Space]
    
    public string[] lamps;
    public string[] difficulties;

    [Space][Space][Space]

    public bool doesHaveWarning;

    [Space][Space][Space]

    public string varName;
}
