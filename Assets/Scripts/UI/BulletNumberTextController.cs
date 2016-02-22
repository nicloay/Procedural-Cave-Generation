using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class BulletNumberTextController : MonoBehaviour {
    Text text;
    Color originalColor;
    Color secondColor;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        secondColor = originalColor = text.color;
        secondColor.a = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {

        text.color = GameData.ActiveBulletsNumber < GameData.MaxBulletOnScreen ? originalColor : secondColor; //indicate that we can't shoot
        text.text = string.Format("Bulltest: {0}", GameData.BulletNumbber);
	}
}
