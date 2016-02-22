using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class RestartButtonController : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(new UnityAction(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
