[System.Serializable]
public class PayloadContainer
{
    public Meta meta;
    public Payload[] scores;
}

[System.Serializable]
public class Meta
{
    public string game;
    
    public string playtype;
    
    public string service;
}


[System.Serializable]
public class Payload
{
    public string lamp;

    public string matchType;

    public string identifier;

    public string difficulty;

    public long timeAchieved;
}



[System.Serializable]
public class ScorePayload : Payload
{
    public int score;
}

[System.Serializable]
public class PercentPayload : Payload
{
    public float percent;
}