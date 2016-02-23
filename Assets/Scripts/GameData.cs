using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace CaveTestTask{

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
        public static Vector3   PlayerPosition;

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
            PlayerPrefs.SetFloat("playerx", PlayerPosition.x);//yes it's not good for long purpose so later would be better to use some tools for serialization
            PlayerPrefs.SetFloat("playery", PlayerPosition.y);
            PlayerPrefs.SetFloat("playerz", PlayerPosition.z);
        }

        public static void LoadData(){
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(PlayerPrefs.GetString(MapPlayerPrefsKey)));
            BinaryFormatter bf = new BinaryFormatter();
            Map = (byte[,])bf.Deserialize(ms);

            PlayerPosition.x = PlayerPrefs.GetFloat("playerx");
            PlayerPosition.y = PlayerPrefs.GetFloat("playery");
            PlayerPosition.z = PlayerPrefs.GetFloat("playerz");
        }
    }
}