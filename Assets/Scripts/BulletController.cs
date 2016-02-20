using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour {
    public float Speed = 0.1f;

    Rigidbody rb;
    void Start(){
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        rb.MovePosition (rb.position + transform.TransformDirection( Vector3.forward) * Speed * Time.deltaTime);
	}
}
