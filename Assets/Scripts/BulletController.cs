using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour {
    public float Speed = 0.1f;
    public float LifeTimeInSeconds = 3.0f;
    float currentTime = 0.0f;

    Rigidbody rb;
    void Start(){
        GameData.ActiveBulletsNumber++;
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        rb.AddForce (transform.TransformDirection( Vector3.forward) * Speed * Time.deltaTime, ForceMode.VelocityChange);
        currentTime+=Time.deltaTime;
        if (currentTime>=LifeTimeInSeconds){
            SelfDestroy();
        }
	}

    void OnCollisionEnter(Collision collision) {        
        SelfDestroy();
    } 

    void SelfDestroy(){
        Destroy(gameObject);
        GameData.ActiveBulletsNumber--;
    }
}

