//Creator:                                  Michael Vakharia
//Email (for queries/freelance work/other): michael@bluecarbonmedia.co.uk
//Email 2:                                  michael.bcm@outlook.com

//This script pans a (Main) Camera across an image plane using the right mouse button.

#region Instructions
//1. Place this script on the Camera you'd like to use for scrolling across your map. 

//2a. Place your image (the one you'd like to scroll across) onto a Plane. If you're not using an image, skip to step 3. 

//2b. Rotate the plane so that the image is aligned correctly with World space, if it isn't already.

//2c. Create a new Empty GameObject, and call it 'Map'. 
//Ensure it is the correct way up, and drag the Plane onto it, making the Plane its child object. 

//3. In the Camera's Inspector, you'll see this script. 

//4. Place your Map (if you followed steps 2a-c) or your image Plane into the 'Game Object' slot,
//under the dropdown 'Map' at the top of the Inspector Script.

//5. Place your Plane into the 'Renderer' slot under the same 'Map' dropdown. 

//6. Now, place your Main Camera into the 'Game Object' slot under the second dropdown, 'Camera'.

//7. Enter 2D space and ensure that the image plane is facing you, the user, in the Scene view. 

//8. In the Game view, ensure that you can see something similar, even if zoomed in or out. 
//If you can't, reposition the camera or the plane, and ensure that the clipping planes are set up properly. 

//8a. Ideally, the Map empty's rotation should be -90 on the X, and 0 on the Y and Z.

//9. Hit Play to test out the map scroll. Right click and drag to move the map in any direction you like, but not beyond the confines of the Y axis. 

//9a. You'll notice that you can still drag the map past the limits of the left and right sides of the camera lens. 
//To remedy this, increase 'Mod X Times' until the map can no longer go beyond the left and right sides. 

//Optional: 'Object For Spawning' is a slot for an object you may want to spawn onto your map by left clicking. Drag any object you like into this slot.

//'Map Zoom' represents the minimum and maximum OrthographicSize of the camera, or in other words, the min/max zoom levels.
//The 'current' level cannot be edited (it updates every frame), but the minimum, maximum, and interval can be set yourself.
//The 'interval' is the amount that the camera zooms in or out (the amount the OrthographicSize changes) on each click of the Mouse Wheel. 
//Again, these figures will require your adjustment, dependending entirely on your project.

//If you have any questions or wish to talk about other projects, my details are at the top of the script.
#endregion

using UnityEngine;
using BCM_MapUtilities;

public class MapMove : MonoBehaviour
{
    #region Variables and Objects
    [SerializeField]
    protected Map map;
    [SerializeField]
    protected _Cam _camera;
    [SerializeField]
    protected GameObject ObjectForSpawning;
    [SerializeField]
    protected float modXTimes; //Adjust modXTimes until the edge of the camera's view stays at the edge of the map at all times. 
    [SerializeField]
    protected Meter mapZoom = new Meter(2, 5, 0, 0.1F); //A meter to control the Camera's 'Size' variable. 

    private float modX;
    #endregion
    
    void Update()
    {
        if (MapUtilities.ClickCheck()) //If the mouse is positioned over a GameObject, 
        {
            if (Input.GetMouseButtonDown(0))    //If we left click, 
                SpawnObjAtPos();                //Spawn 'ObjectForSpawning' at the mouse's current position. 

            if (Input.GetMouseButton(1))        //If we right click,
                transform.Translate(Input.GetAxis("Mouse X") * -0.4F, Input.GetAxis("Mouse Y") * -0.4F, 0); //Move the camera. 
        }
        
        Camera.main.orthographicSize += MapZoom(); //Sets the field of view of the camera. See the method 'MapZoom' for more info.

        mapZoom.current = Camera.main.orthographicSize;

        #region Set Camera and Map Bounds
        _camera.Center = Camera.main.transform.position;
        _camera.Extents = Camera.main.OrthographicBounds().extents;
        _camera._Sides.top.length = _camera.Center.y + _camera.Extents.y;
        _camera._Sides.bottom.length = _camera.Center.y - _camera.Extents.y;
        _camera._Sides.right.length = _camera.Center.x + _camera.Extents.x;
        _camera._Sides.left.length = _camera.Center.x - _camera.Extents.x;

        map._Sides.top.length = map._Renderer.bounds.max.y;
        map._Sides.bottom.length = map._Renderer.bounds.min.y;
        map._Sides.right.length = map._Renderer.bounds.max.x;
        map._Sides.left.length = map._Renderer.bounds.min.x;
        #endregion 

        transform.position = CameraPosition(); //Set the position of this GameObject to that of the Vector3 '_position' put out by the method 'CameraPosition()'.

        //The bounds MUST be set before the camera position is set - the above two lines MUST be executed in the order they're currently in. 
        //DO NOT swap them around, or you will end up being able to drag the camera outside of the map's bounds, resulting in really odd behaviour concerning the camera and the map. 
        //Do so at your peril. 

        modX = modXTimes * Camera.main.orthographicSize; //modX increases or reduces the amount that the camera can scroll horizontally at all zoom levels. Change 'modXTimes' to alter this.
    }

    Vector3 CameraPosition()
    {
        Vector3 _position = transform.position; //Declares a reference to the position of this script's GameObject (the camera). 

        if (_camera._Sides.top.length > map._Renderer.bounds.max.y)             //If the top edge of the camera lens goes over the top edge of the map,
            _position = new Vector3(_position.x, map._Sides.top.length - _camera.Extents.y, _position.z);           //Set the reference's y value to what it was on the frame before it went over.

        if (_camera._Sides.bottom.length < map._Renderer.bounds.min.y)          //If the bottom edge of the camera lens goes over the bottom edge of the map,
            _position = new Vector3(_position.x, map._Sides.bottom.length + _camera.Extents.y, _position.z);        //Set the reference's y value to what it was on the frame before it went over. 

        if (_camera._Sides.right.length > map._Renderer.bounds.max.x - modX)    //If the right edge of the camera lens goes over the right edge of the map, taking into account the modifier,
            _position = new Vector3(map._Sides.right.length - _camera.Extents.x - modX, _position.y, _position.z);  //Set the reference's x value to what it was on the frame before it went over. 

        if (_camera._Sides.left.length < map._Renderer.bounds.min.x + modX)     //If the left edge of the camera lens goes over the left edge of the map, taking into account the modifier,
            _position = new Vector3(map._Sides.left.length + _camera.Extents.x + modX, _position.y, _position.z);   //Set the reference's x value to what it was on the frame before it went over. 

        return _position; //Output the reference with whatever value it may have, regardless of whether or not it was affected by any of the 4 'if' statements above. 
    }
    
    void SpawnObjAtPos()
    {
        if(ObjectForSpawning == null)
            return;

        Vector3 mousePos = Input.mousePosition; //Declare a reference to Input.mousePosition that isn't read only and can be modified (Input.mousePosition IS read-only, and can't). 
        mousePos.z = map._GameObject.transform.position.z - transform.position.z; //The 'z' property of this reference becomes that of the map minus that of the object that this script is on.
        Instantiate(ObjectForSpawning, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity); //Instantiates the object, at the position 'mousePos' transformed into world space.                                                                                               
        //If we don't use ScreenToWorldPoint, 'ObjectForSpawning' will be instantiated somewhere else.
    }

    float MapZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > mapZoom.minimum) //If the scroll wheel is clicked up once while the camera is not fully zoomed in,
            return -mapZoom.interval; //Zoom in some more.
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < mapZoom.maximum) //If the scroll wheel is clicked down once while the camera is not fully zoomed out,
            return mapZoom.interval; //Zoom out some more.
        return 0;
    }
}

namespace BCM_MapUtilities
{
    public class MapUtilities
    {
        public static bool ClickCheck() //Checks whether or not the mouse is positioned over a GameObject. 
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition))) //If the ray travelling from the camera goes through something, 
                return true; //ClickCheck() evaluates to true.
            return false; //Otherwise, ClickCheck() evaluates to false. 
        }
    }

    public static class CameraExtensions
    {
        public static Bounds OrthographicBounds(this Camera cam) //Declares a new Bounding Box object that extends Unity's existing Camera class. 
        {
            float screenAspectRatio = Screen.width / Screen.height; //Aspect ratio of the screen as a float (not in x:y format)
            float cameraHeight = cam.orthographicSize * 2; //The distance from the centre f the screen to the top or bottom, multiplied by two. 
            return new Bounds(cam.transform.position, new Vector3(cameraHeight * screenAspectRatio, cameraHeight, 0));
        }
    }

    [System.Serializable]
    public class Meter //A counter, with minimum and maximum levels, a current level, and an interval (an amount it increases by on every tick/frame/second). 
    {
        public float minimum, maximum, current, interval;
        public Meter() { }
        public Meter(float min, float max, float cur, float intv) { minimum = min; maximum = max; current = cur; interval = intv; } //Sets the four float values when declaring a new Meter object. 
    }

    public class Rectangle //Any four sided GameObject and all of its properties. 
    {
        public GameObject _GameObject;              //The object itself.

        [System.Serializable]
        public class Sides
        {
            public Side top, bottom, left, right;
            //Holds all 4 sides.
        }

        [System.Serializable]
        public class Side
        {
            public float length;                    //One side, with a property for length. 
        }

        public Sides _Sides;                        //Declaration of sides.
    }
    [System.Serializable]
    public class Map : Rectangle
    {
        public Renderer _Renderer;                  //The object's renderer. 
    }

    [System.Serializable]
    public class _Cam : Rectangle
    {
        public Vector3 Center, Extents;             //The position of the object's center and the distance from the center to the edges.
    }
}