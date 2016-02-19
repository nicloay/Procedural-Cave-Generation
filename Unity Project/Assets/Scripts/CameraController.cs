using UnityEngine;
using System.Collections;


public enum CameraState{
    Passive,
    Move,
    Zoom
}


public class CameraController : MonoBehaviour {
    public GameObject Level; 

    //We don't use Level gameobject here in case if we would need to adjust plane offset and so on
    public GameObject MapPlaneLocation;
    public Vector3 MapPlaneNormal = Vector3.up;

    CameraState cameraState = CameraState.Passive;
    Plane levelPlane;

    void Start(){
        if (MapPlaneNormal == Vector3.zero){
            Debug.LogError("Wrong map normal");
            Application.Quit();
        }
        levelPlane = new Plane(MapPlaneNormal, MapPlaneLocation.transform.position);

    }


    // Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) ){            
            HandlePan() ;       
        }
	}

    Vector3 StartGlobalPosition;
    void HandlePan(){
        if (Input.GetMouseButtonDown(0)){
            StartGlobalPosition = ScreenToGlobalPosition(Input.mousePosition);            
        } else if (Input.GetMouseButton(0)){
            Vector3 newGlobalPosition = ScreenToGlobalPosition(Input.mousePosition);
            Vector3 diff = StartGlobalPosition - newGlobalPosition;
            transform.position += diff;            
        }
    }

    /// <summary>
    /// Screens to global position.
    /// It should return global postion. 
    /// We don't handle here if ray would be parallel to the plane (it won't be possible for this kind of task)
    /// </summary>
    /// <returns>The to global position.</returns>
    /// <param name="screenPosition">Screen position.</param>
    Vector3 ScreenToGlobalPosition(Vector2 screenPosition){
        Ray r = Camera.main.ScreenPointToRay(screenPosition);
        float distance;
        if (levelPlane.Raycast(r, out distance)){
            return r.GetPoint(distance);
        }

        Debug.LogError("raycast fail, level plane is perpendecular to the camera frustrum");
        return MapPlaneLocation.transform.position;
    }

}
