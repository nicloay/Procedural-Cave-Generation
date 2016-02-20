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
        int[] shuffledX = RandomUtils.GetShuffleArray(MapGenerator.width);
        int[] shuffledY = RandomUtils.GetShuffleArray(MapGenerator.height);

        for (int y = 0; y < shuffledY.Length; y++) {
            for (int x = 0; x < shuffledX.Length; x++) {
                if (MapGenerator.IsNodeFree(shuffledX[x], shuffledY[y])){
                    return MapGenerator.GetNodeGlobalPosition(shuffledX[x], shuffledY[y]);
                }    
            }
        }
        Debug.LogError("can't find any freenode at this map, I'll return zero here");
        return Vector3.zero;
    }

}
