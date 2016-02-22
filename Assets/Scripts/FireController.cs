using UnityEngine;
using System.Collections;



[RequireComponent(typeof(Rigidbody))]
public class FireController : MonoBehaviour {
    public GameObject BulletPrefab;
    public float FirePointDistance;
    Vector3 PreviousPosition;
    bool directionSet = false;
	
    Vector3 lastKnownDirection = Vector3.zero;


    void Update () {        
        Vector3 currentPosition = transform.position;

        if (directionSet){
            if (currentPosition != PreviousPosition){
                lastKnownDirection = (currentPosition - PreviousPosition).normalized;
            }
        }
        #if DEBUG_RAYS

        Debug.DrawRay(transform.position, lastKnownDirection, Color.black);
        #endif
        if (lastKnownDirection != Vector3.zero && Input.GetButton("Fire") && GameData.ActiveBulletsNumber == 0 ){
            GameObject bullet = GameObject.Instantiate(BulletPrefab);
            Vector3 bulletPosition = transform.position + (lastKnownDirection * FirePointDistance);
            bullet.transform.position = bulletPosition;
            bullet.transform.LookAt(bulletPosition + lastKnownDirection);
        }
        PreviousPosition = currentPosition;
        directionSet = true;
	}        
}
