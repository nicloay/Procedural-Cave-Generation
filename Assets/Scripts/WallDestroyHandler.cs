using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MapGenerator))]
public class WallDestroyHandler : MonoBehaviour {
    MapGenerator mg;
    bool removedAnything = false;
    void Start(){
        mg = GetComponent<MapGenerator>();
    }

    void OnCollisionEnter(Collision collision) {        
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            removedAnything |= HandleContact(collision.contacts[i]);
        }
    }

    void LateUpdate(){        
        if (removedAnything){
            mg.RegenrateMesh();
            removedAnything = false;
        }
    }

    bool HandleContact(ContactPoint contact){
        #if DEBUG_RAYS
        Debug.DrawRay(contact.point, contact.normal.normalized * 0.1f, Color.red);

        #endif

        Vector3 contactPointWithOffset = contact.point;// + contact.normal.normalized * 0.1f;
        return mg.RemoveNodeAtGlobalPositionIfItsNotAWall(contactPointWithOffset);
    }
}
