using System;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class MultiKeys
{
    //Find number of survivor in game
    public static string SURVIVORCOUNTKEY = "SURVIVORCOUNTKEY";
    //(playerName + thiskey) find number that this playername kill
    public static string COUNTKEY = "COUNTKEY";
    //(playerName + thiskey) Is this playername was kill or not
    public static string ISDEADKEY = "ISDEADKEY";
    //(playerName + thiskey) store skill id that this player currently use
    public static string SKILLIDKEY = "SKILLIDKEY";
    //(playerName + thiskey) store postion x that this player attack
    public static string ATTACKPOSTIONXKEY = "ATTACKPOSTIONXKEY";
    //(playerName + thiskey) store postion z that this player attack
    public static string ATTACKPOSTIONZKEY = "ATTACKPOSTIONZKEY";
    //(playerName + thiskey) store rotation that this player attack
    public static string ATTACKDIRECTIONKEY = "ATTACKDIRECTIONKEY";
    //All player name list in format name| 
    public static string ALLPLAYERKEY = "ALLPLAYERKEY";

    public static T GetValueByKey<T>(string playerNameKey, string key)
    {
        Hashtable roomKey = PhotonNetwork.CurrentRoom.CustomProperties;
        if (roomKey.TryGetValue(playerNameKey + key, out object value))
            return (T)Convert.ChangeType(value, typeof(T));
        return default;
    }

    public static void SetValueByKey<T>(string playerNameKey, string key, T value)
    {
        Hashtable hash = new Hashtable();
        T castValue = (T)Convert.ChangeType(value, typeof(T));
        hash.Add(playerNameKey + key, castValue);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

}
