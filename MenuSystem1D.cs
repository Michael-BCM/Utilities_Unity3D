//System by Michael Vakharia. 
//Email: michael@bluecarbonmedia.co.uk 
//LinkedIn: https://uk.linkedin.com/in/michaelvakharia

#region Introduction
//This system allows for the use of a keyboard or controller to scroll between and highlight multiple options aligned horizontally or vertically, 
//whether they be options in a menu, or on a multiple choice question sheet, or otherwise. 

//It's virtually plug and play, and with the instructions below, you should be up and running within a few minutes. 
//If you do have any questions, drop me a message using the contact information above.
#endregion

#region Instructions

//Place this system onto a GameObject (ideally an empty, dedicated to 'controlling the menu') and set the size of the Options List 
//depending on how many options you have: 'Play, Options, Quit' would be 3.

//Then, drag your options (as GameObjects) into the Element slots, 
//and set Initial Selection ID depending on the option you'd like to show when the menu loads. For Element 0 (Play?), set it to 0. For Element 1 (Settings?), set it to 1.
//The menu objects can, but don't need to be child objects of this object.

//This option (Current Selection) will set itself when you enter Playmode, and will change every time you use directional input to change the selection.

//Set the orientation depending on the direction of the menu: 
//If the menu is ordered top to bottom, use Plus Y To Minus Y; for bottom to top, Minus Y To Plus Y. For left to right menus, Minus X to Plus X, and for right to left, Plus X To Minus X. 

//Set up the menu control type to use the keyboard, the controller, or both.

//Keyboard Input Type: Mandatory: Set up 4 new inputs in the Input Manager, and call them Keyboard Up, Keyboard Down, Keyboard Left, and Keyboard Right.
//Set their primary and secondary inputs to whatever you'd like to use as keyboard direction up (W, Up Arrow), keyboard direction down (S, Down Arrow) and so on.
//Errors may occur if you don't do this.
//When that's set up, set the Keyboard Input Type to Generic (for KeyCodes) or Button (for the buttons you just set up).

//Once you are set up, enter Playmode. 
//Then, depending on how you've set up your menu, use the corresponding directional inputs (arrow keys, controller thumbsticks, etc) and watch the Current Selection change.

//Use the current selection in conjunction with key or button presses by referencing the selectionID as follows: if (selectionID == 0 && Input.GetButtonDown("A Button")) { //do stuff }
//in order to perform actions (which you would code), like loading another scene or quitting the application. 
#endregion

#region Script
using UnityEngine;

#region enums
public enum Axis { PlusYToMinusY, MinusYToPlusY, MinusXToPlusX, PlusXToMinusX }     //The origin of the menu and the direction it is laid out in from there. 
                                                                                    //For example, set the orientation to 'PlusYToMinusY' for a menu that has its first option at the top, 
                                                                                    //followed by subsequent options below in descending order.

public enum Operator { Plus, Minus }                                                //Used with the 'Scroll()' method, the second to last method in the script.

public enum ControlType { Keyboard, Controller, Both }                              //The user can select the current method of control from a menu in the Inspector.
                                                                                    //Ideally, the player should be able to alter this themselves by use of an Options menu in game. 
                                                                                    
public enum KeyType { GenericKeyCode, Button}                                       //For keyboard inputs, do you want to use the generic GetKeyDown, or the non generic GetButtonDown?
                                                                                    //Bear in mind that you'll have to add Inputs to the Input Manager titled Keyboard Up, Keyboard Down, etc...
                                                                                    //(see the method 'KeyboardScroll' for details)
#endregion

public class MenuSystem1D : MonoBehaviour
{
    #region Variables and Objects   
    [SerializeField]
    protected GameObject[] _optionsList;                            //Drag each of your options (Play, Options, Quit, etc, as GameObjects) into this list.
    public GameObject[] optionsList{ get { return _optionsList; } } //Ensure that they're aptly named. 

    [SerializeField]
    protected GameObject _currentSelection;                                     //The currently selected option (at first, this might be 'New Game' 
    public GameObject currentSelection { get { return _currentSelection; } }    //(if you're a new user) or 'Continue' (if you're not)).
                                                                                //This is set on the first frame of Update (see below).
    [SerializeField]
    protected int _initialSelectionID, _selectionID;                        //The number within the square brackets of 'optionsList[selectionID]', 
    public int initialSelectionID { get { return _initialSelectionID; } }   //and its initial value, the latter of which you must set up yourself.
    public int selectionID { get { return _selectionID; } }                 //The 'Scroll()' method increments and decrements the selectionID, 
                                                                            //depending on the key pressed.            
                                                                            //The current selection then updates to reflect the new selectionID. 
    [SerializeField]
    protected Axis menuOrientation;                 
    [SerializeField]
    protected ControlType menuControlType;      
    [SerializeField]
    protected KeyType keyboardInputType;

    protected bool scrollOnce = false;
    public bool menuIsActive; //A toggle you can use to turn the menu system on and off without deactivating the gameobject.

    protected bool AxisIsInUse() { if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) { return true; } return false; }
    
    #endregion

    private void Start()
    {
        _selectionID = _initialSelectionID; //On the first frame, we set the selectionID to the value defined by you, the developer. 
    }

    private void Update()
    {
        if (!menuIsActive)
            return;

        switch (menuControlType)
        {
            case ControlType.Controller: ControllerScroll(); break;             //If 'menuControlType' is set to 'ControlType.Controller', run the 'ControllerScroll' method. 
            case ControlType.Keyboard: KeyboardScroll(); break;                 //If 'menuControlType' is set to 'ControlType.Keyboard', run the 'KeyboardScroll' method. 
            case ControlType.Both: ControllerScroll(); KeyboardScroll(); break; //See lines above, and run both methods. 
        }
        _currentSelection = _optionsList[_selectionID]; //Set the current selection to the option under 'selectionID' in the optionsList. 
    }

    private void ControllerScroll()
    {
        if (!AxisIsInUse()) //If the axis is no longer in use,
            scrollOnce = false; //The axis has stopped being in use and can be used again. 

        if (AxisIsInUse() && !scrollOnce) //If the horizontal or vertical axes are in use, 
        {
            if (menuOrientation == Axis.PlusYToMinusY || menuOrientation == Axis.PlusXToMinusX) //If the menu is oriented top-to-bottom or right-to-left, 
            {
                if (CurrentAxis() > 0) //If 'Up' or 'Right' is pressed,
                    Scroll(Operator.Minus); //Decrement 'Selection ID'

                else if (CurrentAxis() < 0) //If 'Down' or 'Left' is pressed,
                    Scroll(Operator.Plus); //Increment 'Selection ID'

                scrollOnce = true; //The axis has been used and can't be used again until the user lets go of it.
            }
            if (menuOrientation == Axis.MinusYToPlusY || menuOrientation == Axis.MinusXToPlusX) //If the menu is oriented bottom-to-top or left-to-right, 
            {
                if (CurrentAxis() > 0) //If 'Up' or 'Right' is pressed,
                    Scroll(Operator.Plus); //Increment 'Selection ID'

                else if (CurrentAxis() < 0) //If 'Down' or 'Left' is pressed,
                    Scroll(Operator.Minus); //Decrement 'Selection ID'

                scrollOnce = true; //The axis has been used and can't be used again until the user lets go of it.
            }
        }        
    }

    void KeyboardScroll()
    {
        //If the input type is 'GenericKeyCode' and the user presses a corresponding key, 
        //OR if the input type is 'Button' and the user presses the corresponding button,
        //increment or decrement Selection ID depending on the orientation of the menu.

        if ((keyboardInputType == KeyType.GenericKeyCode && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            || (keyboardInputType == KeyType.Button && Input.GetButtonDown("Keyboard Down")))
            switch (menuOrientation) { case Axis.PlusYToMinusY: Scroll(Operator.Plus); break; case Axis.MinusYToPlusY: Scroll(Operator.Minus); break; }

        if ((keyboardInputType == KeyType.GenericKeyCode && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            || (keyboardInputType == KeyType.Button && Input.GetButtonDown("Keyboard Up")))
            switch (menuOrientation) { case Axis.PlusYToMinusY: Scroll(Operator.Minus); break; case Axis.MinusYToPlusY: Scroll(Operator.Plus); break; }

        if ((keyboardInputType == KeyType.GenericKeyCode && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
            || (keyboardInputType == KeyType.Button && Input.GetButtonDown("Keyboard Right")))
            switch (menuOrientation) { case Axis.MinusXToPlusX: Scroll(Operator.Plus); break; case Axis.PlusXToMinusX: Scroll(Operator.Minus); break; }

        if ((keyboardInputType == KeyType.GenericKeyCode && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
            || (keyboardInputType == KeyType.Button && Input.GetButtonDown("Keyboard Left")))
            switch (menuOrientation) { case Axis.MinusXToPlusX: Scroll(Operator.Minus); break; case Axis.PlusXToMinusX: Scroll(Operator.Plus); break; }
    }

    void Scroll(Operator _operator)
    {
        if (_operator == Operator.Plus) //If the method's parameter is set to 'Plus', 
        {
            _selectionID++; //Increment SelectionID. 

            if (_selectionID > _optionsList.Length - 1) //If SelectionID is greater than the ID of the last element,
                _selectionID = 0; //reset it to 0. 
        }
        else if (_operator == Operator.Minus) //If the method's parameter is set to 'Minus', 
        {
            _selectionID--; //Decrement SelectionID. 

            if (_selectionID < 0) //If SelectionID is below zero,
                _selectionID = _optionsList.Length - 1; //Set it to the value of the ID of the final element. 
        }
    }

    float CurrentAxis()
    {
        switch (menuOrientation)
        {
            case Axis.PlusYToMinusY: case Axis.MinusYToPlusY: return Input.GetAxis("Vertical"); 
                //If the menu is oriented top-to-bottom or vice versa, use the vertical axis. 

            case Axis.MinusXToPlusX: case Axis.PlusXToMinusX: return Input.GetAxis("Horizontal"); 
                //If the menu is oriented left-to-right or vice versa, use the horizontal axis. 
        }
        return 0;
    }
}
#endregion