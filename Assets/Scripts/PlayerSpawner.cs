using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
    public GameObject PlayerPrefab;
    public MapGenerator MapGenerator;
    GameObject playerInstance;
	// Use this for initialization
	void Awake () {
        if (MapGenerator == null || PlayerPrefab == null ){
            Debug.LogError("PlayerPrefab and mapGenerators must be set");
            Application.Quit();
        }
        MapGenerator.OnMapGenerationDone+=PlacePlayer;
	}
	
    void OnDestroy(){
        MapGenerator.OnMapGenerationDone-=PlacePlayer;
    }


    void PlacePlayer(){
        if (playerInstance == null){
            playerInstance = GameObject.Instantiate(PlayerPrefab);
        }
        playerInstance.transform.position = GetFreePlayerPosition();
    }

    Vector3 GetFreePlayerPosition(){
        int rescueCounter = 1000; //we could be unlucky and unlimited numbers of time take used cound
        int x,y, value;
        while (rescueCounter-- > 0){            
            if (!MapGenerator.GetRandomMapNode(out x, out y, out value)){
                return MapGenerator.GetNodeGlobalPosition(x,y);
            }
        }
        // get first empty map;

        return Vector3.zero;//FIXME - handle something here =);
    }

}
