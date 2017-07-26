///Place this script onto any GameObject to turn it into a highly flexible, extendable button. Just make sure that you can see it through your chosen camera.
///Unlike Unity's UI button, you can press and hold this one.
///See below for help. 
using System;
using UnityEngine;

public enum ControlState { NotTouching, BeginningTouch, Touching, EndingTouch }

public class InteractiveTouchControl : MonoBehaviour
{
    public ControlState ICState { get; private set; }///The current state of the interactive control (see ControlState enum above).
    protected int touchStage { get; private set; }

    [SerializeField]
    protected int touchIndex;
    /// Set 'fingerNumber' to 0 in the Inspector for the button to interact with the first finger that touches the screen. 
    /// Set it to 1 for the button to interact with the second finger that touches the screen, and so on. 

    [SerializeField]
    protected float range;
    /// How far apart can the camera and this GameObject be before the button no longer responds to touch?

    [SerializeField]
    protected Camera currentCamera; ///Which camera are we looking through to see this object? 

    [SerializeField]
    protected UnityEngine.Events.UnityEvent OnFullPress; 
    ///An event that is triggered when the button is fully pressed and released, 
    ///without removal of the finger from the button (e.g. by sliding it off to the side, mid-press). 
    ///You can set off a method in the Inspector, under the events list marked 'On Full Press()'.
    ///Click the '+', then choose an object in the scene and a function on it to trigger. 
    
    public void Release() { ICState = ControlState.NotTouching; }
    /// Run this from somewhere else if you want to release the interactive control without taking your finger off it. 
    ///Useful for precisely locking down objects that can be dragged across the screen. 

    public void SetControlInteractivity(bool state) { gameObject.GetComponent<Collider>().enabled = state; }
    /// Sets the interactivity of the control to true or false. 

    public void SetControlVisibility(bool state) { gameObject.GetComponent<MeshRenderer>().enabled = state; }
    /// Sets the visibility of the control to true or false.
    
    protected virtual void Update ()
    {
        ICState = ControlStateF(touchIndex);

        #region Trigger for a full press of the interactive control. 
        switch (ControlStateF(touchIndex))
        {
            case ControlState.NotTouching: touchStage = 0; break;
            case ControlState.BeginningTouch: touchStage = 1; break;
            case ControlState.Touching: { if (touchStage == 1) { touchStage = 2; } break; }
            case ControlState.EndingTouch: { if (touchStage == 2) { OnFullPress.Invoke(); } touchStage = 0; break; }
        }
        #endregion
    }

    private ControlState ControlStateF (int index)
    {
        #region The TouchPhase enum, the current touchcount, and a raycast are used to return an enum that the class can use.
        if (Input.touchCount > index)
        {
            if (Input.GetTouch(index).phase == TouchPhase.Moved || Input.GetTouch(index).phase == TouchPhase.Stationary)
            {
                if (isTouchingObject(gameObject, index))
                    return ControlState.Touching;
                else
                    return ControlState.NotTouching;
            }

            if (isTouchingObject(gameObject, index))
            {
                if(Input.GetTouch(index).phase == TouchPhase.Began)
                    return ControlState.BeginningTouch;
            }
            if (Input.GetTouch(index).phase == TouchPhase.Ended)
            {
                return ControlState.EndingTouch;
            }  
        }
        return ControlState.NotTouching;
        #endregion
    }

    public bool isTouchingObject(GameObject Object, int index) ///Is the user touching an object on screen? 
    {
        Ray ray = currentCamera.ScreenPointToRay(Input.GetTouch(index).position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
            if (hit.transform.gameObject == Object)
                return true;
        return false;
    }

    private void medic()
    {
        Exception up = new Exception("I'm not feeling too good...");
        throw up; ///bleurgh. 
    }
}