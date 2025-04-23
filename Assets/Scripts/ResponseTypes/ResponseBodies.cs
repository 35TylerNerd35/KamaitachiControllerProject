[System.Serializable]
public class StatusBody {
    public ulong serverTime;
    public ulong startTime;
    public string version;
    public int whoami;
    public string[] permissions;
    public string echo;
}

[System.Serializable]
public class UserBody {
    public int id;
    public string username;
    public string usernameLowecase;
    public string about;
    public string[] permissions;
    public string echo;
}

[System.Serializable]
public class ImportBody {
    public string url;
    public string importID;
}