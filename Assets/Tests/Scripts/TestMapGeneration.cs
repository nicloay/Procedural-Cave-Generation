using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


namespace Tests{    
    public class TestMapGeneration : MonoBehaviour {
        public MapGenerator MapGenerator;
        //each row is a line, x - occupied 0 -empty field
        [Multiline]
        [FormerlySerializedAs("Level")]
        public string Map;
        // Use this for initialization
        void Start () {
            GameData.Map = StringToMap(Map);
            MapGenerator.enabled = true;
        }

        static bool GetMapSize(string map, out int width, out int height){
            width = int.MaxValue;
            height = 0;
            foreach(var row in map.Split('\n')){
                height++;
                width = Mathf.Min(width, row.Length);
            }
            if (width == int.MaxValue || height == 0){                
                return false;
            }
            return true;
        }


        public static byte[,] StringToMap(string map){
            int width, height;
            if (GetMapSize(map, out width, out height)){
                byte[,] result = new byte[width,height] ;
                int y = 0;
                foreach(var row in map.Split('\n')){
                    for (int x = 0; x < row.Length; x++)
                    {
                        switch(row[x]){
                            case 'X':
                            case 'x':
                                result[x,y] = 1;
                                break;
                            default:
                                result[x,y] = 0;
                                break;
                        }
                    }
                    y++;
                }
                return result;
            }
            return null;
        }
    }
}
