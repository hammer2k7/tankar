using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GlobalInput : MonoBehaviour
{
    private Player player;
    private float currentOrthosize;
    private float currentFieldofview;
    public NavMeshSurface navSurface;


    void Start()
    {
        GlobalInfo.scrollSpeed = 25;
        GlobalInfo.rotateSpeed = 100;
        GlobalInfo.scrollWidth = 15;
        GlobalInfo.rotateSpeed = 100;
        GlobalInfo.minCameraHeight = 10;
        GlobalInfo.maxCameraHeight = 40;

        player = transform.root.GetComponent<Player>();

        currentFieldofview = Camera.main.fieldOfView;
        currentOrthosize = Camera.main.orthographicSize;

        navSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.human)
        {
            RotateCamera();
            MoveCamera();
            ZoomCamera();
        }
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //detect rotation amount if ALT is being held and the Right mouse button is down
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * GlobalInfo.rotateSpeed;
            destination.y += Input.GetAxis("Mouse X") * GlobalInfo.rotateSpeed;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * GlobalInfo.rotateSpeed);
        }
    }

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        //horizontal camera movement
        if (xpos >= 0 && xpos < GlobalInfo.scrollWidth)
        {
            movement.x -= GlobalInfo.scrollSpeed;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - GlobalInfo.scrollWidth)
        {
            movement.x += GlobalInfo.scrollSpeed;
        }

        //vertical camera movement
        if (ypos >= 0 && ypos < GlobalInfo.scrollWidth)
        {
            movement.z -= GlobalInfo.scrollSpeed;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - GlobalInfo.scrollWidth)
        {
            movement.z += GlobalInfo.scrollSpeed;
        }

        //make sure movement is in the direction the camera is pointing
        //but ignore the vertical tilt of the camera to get sensible scrolling
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        //away from ground movement
        movement.y -= GlobalInfo.scrollSpeed * Input.GetAxis("Mouse ScrollWheel");

        //calculate desired camera position based on received input
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        //limit away from ground movement to be between a minimum and maximum distance
        if (destination.y > GlobalInfo.maxCameraHeight)
        {
            destination.y = GlobalInfo.maxCameraHeight;
        }
        else if (destination.y < GlobalInfo.minCameraHeight)
        {
            destination.y = GlobalInfo.minCameraHeight;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * GlobalInfo.scrollSpeed);
        }
    }

    void ZoomCamera()
    {
        // -------------------Code for Zooming Out------------
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= currentFieldofview)
                Camera.main.fieldOfView += 2;
            if (Camera.main.orthographicSize <= currentOrthosize)
                Camera.main.orthographicSize += 0.5f;

        }
        // ---------------Code for Zooming In------------------------
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 2)
                Camera.main.fieldOfView -= 2;
            if (Camera.main.orthographicSize >= 1)
                Camera.main.orthographicSize -= 0.5f;
        }

        // -------Code to switch camera between Perspective and Orthographic--------
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (Camera.main.orthographic == true)
                Camera.main.orthographic = false;
            else
                Camera.main.orthographic = true;
        }
    }


}
