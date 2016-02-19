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
    Plane cameraPlane;
    void Start(){
        if (MapPlaneNormal == Vector3.zero){
            Debug.LogError("Wrong map normal");
            Application.Quit();
        }
        levelPlane = new Plane(MapPlaneNormal, MapPlaneLocation.transform.position);
        cameraPlane = new Plane(MapPlaneNormal, transform.position);
    }


    // Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) ){            
            HandlePan() ;       
        }
	}

    Vector3 StartGlobalPosition;
    Vector3 NearClipPlaneStartPosition;
    float ScreenToCamRatio;
    void HandlePan(){
        float rayDistance;
        if (Input.GetMouseButtonDown(0)){
            Ray outRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            levelPlane.Raycast(outRay, out rayDistance);
            StartGlobalPosition = outRay.GetPoint(rayDistance);
            NearClipPlaneStartPosition = outRay.GetPoint(0.0f);
        } else if (Input.GetMouseButton(0)){
            Ray camToPlaneRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            levelPlane.Raycast(camToPlaneRay, out rayDistance);
            float globalToCamDistance = Vector3.Distance( camToPlaneRay.GetPoint(rayDistance), transform.position);


            Ray globalToCamRay = new Ray(StartGlobalPosition, -camToPlaneRay.direction);
            Debug.DrawRay(globalToCamRay.GetPoint(0.0f), globalToCamRay.GetPoint(globalToCamDistance));
            Vector3 newCamPosition = globalToCamRay.GetPoint(globalToCamDistance);

            transform.position = newCamPosition;

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


    float FrustumHeightAtDistance(float distance) {
        return 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }
}
