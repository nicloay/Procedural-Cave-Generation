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
    public float CamMinFOW = 20;
    public float CamMaxFOW = 60;
  
    CameraState cameraState = CameraState.Passive;
    Plane levelPlane;

    void Start(){
        if (MapPlaneNormal == Vector3.zero){
            Debug.LogError("Wrong map normal");
            Application.Quit();
        }
        levelPlane = new Plane(MapPlaneNormal, MapPlaneLocation.transform.position);
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, CamMinFOW, CamMaxFOW);
    }


    // Update is called once per frame
	void Update () {
        if (Input.touchCount > 1){
            HandleZoom() ;   
        } else if (Input.GetMouseButton(0) ){            
            HandlePan() ;       
        }
	}


    float startZoomDiff;
    float camStartZoomFow;
    void HandleZoom(){
        Touch t1 = Input.touches[0];
        Touch t2 = Input.touches[1];
        if (t1.phase== TouchPhase.Began && t2.phase == TouchPhase.Began){
            startZoomDiff = Vector2.Distance(t1.position, t2.position);
            camStartZoomFow = Camera.main.fieldOfView;
        } else if (t1.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved ){
            
            float newDiff = Vector2.Distance(t1.position, t2.position);
            float ratio = startZoomDiff / newDiff;

            Camera.main.fieldOfView = Mathf.Clamp( camStartZoomFow * ratio, CamMinFOW, CamMaxFOW);

        }
    }


    Vector3 StartGlobalPosition;

    /// <summary>
    /// Handles the pan.
    /// we use little hack here. we check mouse position because by default nity simulate 3 touches by mouse[0-3] buttons
    /// no need
    /// </summary>
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
