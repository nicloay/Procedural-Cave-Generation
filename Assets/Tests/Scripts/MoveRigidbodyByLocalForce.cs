using UnityEngine;
using System.Collections;


namespace Tests{    
    public class MoveRigidbodyByLocalForce : MonoBehaviour {
        public Rigidbody RigidBody;
        public Vector3 LocalForce;


        
        // Update is called once per frame
        void Update () {            
            RigidBody.AddForce( RigidBody.transform.TransformDirection( LocalForce), ForceMode.Acceleration);
        }
    }
}
