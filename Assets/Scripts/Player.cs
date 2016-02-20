using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Rigidbody rigidbody;
	Vector3 velocity;
	
	void Start () {
        rigidbody = GetComponentInChildren<Rigidbody> ();
	}

	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * 10;
	}

	void FixedUpdate() {
        rigidbody.AddForce (velocity, ForceMode.Acceleration);
	}
}
