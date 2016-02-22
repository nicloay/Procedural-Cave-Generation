using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class BulletNumberTextController : MonoBehaviour {
    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format("Bulltest: {0}", GameData.BulletNumbber);
	}
}
