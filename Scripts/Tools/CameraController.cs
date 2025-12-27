using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System.Collections.Generic;



namespace STUDIO {

public class CameraController : MonoBehaviour {

    // Zoom settings (for orthographic zooming in this example).
    public float zoomSpeed = 2f;
    public float minZoom = 3f;
    public float maxZoom = 10f;

    // Pan settings.
    public float panSpeed = 1f;

    // Reference to the background image that should enable panning.
    public GameObject backgroundImage;

    private Camera cam;
    private Vector3 dragOrigin;
    private bool canDrag = false;

    // These are used for UI raycasting.
    public GraphicRaycaster raycaster;
    private EventSystem eventSystem;


    private void Start(){
        cam = GetComponent<Camera>();
        if(cam == null){
            Debug.LogError("Camera component not found!");
            return;
        }
        // Get the GraphicRaycaster from the Canvas (assumes there's one in your scene).
        //raycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    private void Update(){
        HandleZoom();
        HandlePan();
        ResetView1();
        ResetView2();
    }

    // Zoom by adjusting the camera's orthographic size.
    void HandleZoom(){
        PointerEventData pointerData = new PointerEventData(eventSystem){ position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerData, results);

        if (results.Count > 0 && results[0].gameObject == backgroundImage){
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if(Mathf.Abs(scroll) > 0.01f){
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
        }
    }

    // Pan only if the topmost UI element under the pointer is the background image.
    private void HandlePan(){
        if(Input.GetMouseButtonDown(0)){
            // Create pointer data for the current mouse position.
            PointerEventData pointerData = new PointerEventData(eventSystem){ position = Input.mousePosition };
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerData, results);

            // Check the top-most element in the UI under the mouse.
            if(results.Count > 0 && results[0].gameObject == backgroundImage){
                canDrag = true;
                // Convert mouse position to world coordinates (using near clip plane depth).
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = cam.nearClipPlane;
                dragOrigin = cam.ScreenToWorldPoint(mousePos);
            } else {
                // Block dragging if any other UI element is in front.
                canDrag = false;
            }
        }

        if(Input.GetMouseButton(0) && canDrag){
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = cam.nearClipPlane;
            Vector3 currentMouseWorldPos = cam.ScreenToWorldPoint(mousePos);
            Vector3 difference = dragOrigin - currentMouseWorldPos;
            cam.transform.position += difference * panSpeed;
        }

        if(Input.GetMouseButtonUp(0)){
            canDrag = false;
        }
    }

    void ResetView1(){
        if(! (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space)) ){ return; }

        cam.orthographicSize = 5f;
    }

    void ResetView2(){
        if(! (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space)) ){ return; }

        cam.transform.position = new Vector3(0, 0, -10f);
        cam.orthographicSize = 5f;
    }

    public void ResetView(){
        cam.transform.position = new Vector3(0, 0, -10f);
        cam.orthographicSize = 5f;
    }

}
}