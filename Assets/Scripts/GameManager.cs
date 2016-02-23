using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public class UIConfig{
    public Button   SaveButton;
    public Button   LoadButton;
    public Button   RestartButton;
    public Text     WallsText;
    public Text     NodeNumberText;

}

public class GameManager : MonoBehaviour {	
    public UIConfig UI;
    public MapGenerator MapGenerator;

    void Start(){
        UI.SaveButton.onClick.AddListener(new UnityAction(SaveGame));
        UI.LoadButton.onClick.AddListener(new UnityAction(LoadGame));
        UI.RestartButton.onClick.AddListener(new UnityAction(RestartGame));
    }

    void RestartGame(){
        GameData.Map = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update(){
        UI.LoadButton.interactable = GameData.HasSave;
        UI.WallsText.text = string.Format("Walls: {0}", MapGenerator.WallsNumber);
        UI.NodeNumberText.text = string.Format("Nodes: {0}", MapGenerator.NodeNumber);
    }

    public void SaveGame(){
        GameData.SaveData();
    }

    public void LoadGame(){

        //TODO: better to restart scene and rebuild from scratch and place player at position
        GameData.LoadData();
        MapGenerator.RegenrateMesh();
        GameObject.FindGameObjectWithTag("Player").transform.position = GameData.PlayerPosition;
    }
}
