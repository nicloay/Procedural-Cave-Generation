using UnityEngine;
using System.Collections;

public class TestIfPlayerSpawned : MonoBehaviour {
    public float Timeout;
	// Use this for initialization
	void Start () {
        StartCoroutine(DoTest());
	}
	
    IEnumerator DoTest(){
        yield return new WaitForSeconds(Timeout);
        if (FindObjectOfType<Player>() != null){
            IntegrationTest.Pass(gameObject);
        } else {
            IntegrationTest.Fail(gameObject);
        }
    }
}
