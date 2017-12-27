using UnityEngine;

/// <summary>
/// An ability that the character may use when requirements (button combinations, character states) have been fulfilled by the player.
/// </summary>
[System.Serializable]
public class Ability
{    
    [Header("Can this ability be used?")]
    [SerializeField]
    protected bool _canUse;
    /// <summary>
    /// When 'canUse' is 'true', the user is able to fulfil the requirements necessary to activate the ability (press the button, etc)
    /// </summary>
    public bool canUse { get { return _canUse; } protected set { _canUse = value; } }

    [Header("Is the ability in use?")]
    [SerializeField]
    protected bool _isUsing;
    /// <summary>
    /// When 'isUsing' is true, the user is executing the ability through one of the 'Update' functions. 
    /// </summary>
    public bool isUsing { get { return _isUsing; } protected set { _isUsing = value; } }
        
    /// <summary>
    /// Enables the ability for use, setting 'canUse' to true. 
    /// Execute this method when the user is able to press the button or combination of buttons to activate the ability. 
    /// </summary>
    public virtual void EnableAbility()
    {
        canUse = true;
    }

    /// <summary>
    /// Disables the ability, setting 'canUse' to false. 
    /// Execute this method when the user should be unable to press the button or combination of buttons to activate the ability,
    /// owing to circumstances or conditions within the game. 
    /// </summary>
    public void DisableAbility ()
    {
        canUse = false;
    }

    /// <summary>
    /// Begins execution of the ability, setting 'isUsing' to true. 
    /// Execute this method when the user fulfils the condition required to execute the ability (the user pushes a button(s), etc). 
    /// Ensure that the ability method itself is in one of the 'Update()' methods, ready to go when 'isUsing' is set to 'true'. 
    /// </summary>
    public void StartAbility ()
    {
        isUsing = true;
    }

    /// <summary>
    /// Ends execution of the ability, setting 'isUsing' to false. 
    /// Execute this method on the frame that the ability should conclude. 
    /// </summary>
    public virtual void EndAbility ()
    {
        isUsing = false;
    }
}