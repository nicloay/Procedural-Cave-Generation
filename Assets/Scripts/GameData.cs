using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System;

public static class GameData  {
    /// <summary>
    /// The map.
    ///     1 - occupied field
    ///     0 - used field
    /// </summary>
    public static byte[,]   Map;
    public static int       ActiveBulletsNumber = 0;
    public static int       BulletNumbber = 100;
    public static int       MaxBulletOnScreen = 1;

    public static bool      HasSave{
        get{
            return PlayerPrefs.HasKey(MapPlayerPrefsKey);
        }
    }



    const string MapPlayerPrefsKey = "map";

    //on next refactoring i'll extract this code to separated class , but right now is most eficcient
    public static void SaveData(){
        MemoryStream ms = new MemoryStream();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, Map);
        PlayerPrefs.SetString(MapPlayerPrefsKey, Convert.ToBase64String(ms.GetBuffer()));

    }

    public static void LoadData(){
        MemoryStream ms = new MemoryStream(Convert.FromBase64String(PlayerPrefs.GetString(MapPlayerPrefsKey)));
        BinaryFormatter bf = new BinaryFormatter();
        Map = (byte[,])bf.Deserialize(ms);
    }



}
