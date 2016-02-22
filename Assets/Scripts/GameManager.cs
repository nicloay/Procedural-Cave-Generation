using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class UIConfig{
    public Button SaveButton;
    public Button LoadButton;
}

public class GameManager : MonoBehaviour {	
    public UIConfig UI;
    public MapGenerator MapGenerator;

    void Start(){
        UI.SaveButton.onClick.AddListener(new UnityAction(SaveGame));
        UI.LoadButton.onClick.AddListener(new UnityAction(LoadGame));
    }

    void Update(){
        UI.LoadButton.interactable = GameData.HasSave;
    }

    public void SaveGame(){
        GameData.SaveData();
    }

    public void LoadGame(){
        GameData.LoadData();
        MapGenerator.RegenrateMesh();
    }
}
