

using Newtonsoft.Json;

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

[System.Serializable]
public class SearchBody {
    public UserBody[] users;
    public SongDocument charts;
}

[System.Serializable]
public class SongDocument {
    [JsonProperty("museca:Single")]
    public ChartsResponse[] musecaSingle;
    [JsonProperty("wacca:Single")]
    public ChartsResponse[] waccaSingle;
    [JsonProperty("maimai:Single")]
    public ChartsResponse[] maimaiSingle;
    [JsonProperty("ongeki:Single")]
    public ChartsResponse[] ongekiSingle;
    [JsonProperty("gitadora:Gita")]
    public ChartsResponse[] gitadoraGita;
    [JsonProperty("gitadora:Dora")]
    public ChartsResponse[] gitadoraDora;
    [JsonProperty("chunithm:Single")]
    public ChartsResponse[] chunithmSingle;
    [JsonProperty("popn:9B")]
    public ChartsResponse[] popn9B;
    [JsonProperty("jubeat:Single")]
    public ChartsResponse[] jubeatSingle;
    [JsonProperty("ddr:DP")]
    public ChartsResponse[] ddrDp;
    [JsonProperty("ddr:SP")]
    public ChartsResponse[] ddrSp;
    [JsonProperty("maimaidx:Single")]
    public ChartsResponse[] maimaidxSingle;
    [JsonProperty("sdvx:Single")]
    public ChartsResponse[] sdvxSingle;
    [JsonProperty("iidx:DP")]
    public ChartsResponse[] iidxDp;
    [JsonProperty("iidx:SP")]
    public ChartsResponse[] iidxSp;
}

[System.Serializable]
public class ChartsResponse {
    public Song song;
    public Chart chart;
}

[System.Serializable]
public class Song {
    public string[] altTitles;
    public string artist;
    public SongData data;
    public int id;
    public string[] searchTerms;
    public string title;
    public string __textScore;
}

[System.Serializable]
public class SongData {
    public string displayVersion;
    public string genre;
}

[System.Serializable]
public class Chart {
    public string chartID;
    public ChartData data;
    public string difficulty;
    public bool isPrimary;
    public string level;
    public float levelNum;
    public string playtype;
    public int songID;
    public string[] versions;
    public int playcount;
}   

[System.Serializable]
public class ChartData {
    public int inGameID;
}