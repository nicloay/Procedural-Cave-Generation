using UnityEngine;
using System.Collections;


namespace Tests{    
    public class TestMapGeneration : MonoBehaviour {
        public MapGenerator MapGenerator;
        //each row is a line, x - occupied 0 -empty field
        [Multiline]
        public string Level;
        // Use this for initialization
        void Start () {
            int width, height;
            if (GetMapSize(out width, out height)){
                GameData.Map = new byte[width,height] ;
                int y = 0;
                foreach(var row in Level.Split('\n')){
                    for (int x = 0; x < row.Length; x++)
                    {
                        switch(row[x]){
                            case 'X':
                            case 'x':
                                GameData.Map[x,y] = 1;
                                break;
                            default:
                                GameData.Map[x,y] = 0;
                                break;
                        }
                    }
                    y++;
                }
                MapGenerator.enabled = true;
                //MapGenerator.RegenrateMesh();
            }
        }

        bool GetMapSize(out int width, out int height){
            width = int.MaxValue;
            height = 0;
            foreach(var row in Level.Split('\n')){
                height++;
                width = Mathf.Min(width, row.Length);
            }
            if (width == int.MaxValue || height == 0){
                IntegrationTest.Fail(gameObject, "map is wrong");
                return false;
            }
            return true;
        }
    }
}
