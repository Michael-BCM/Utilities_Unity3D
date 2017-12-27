using UnityEngine;

[System.Serializable]
public class LimitedAbility : Ability
{
    [Header("How many times has this ability been used?")]
    [SerializeField]
    protected int _numberOfUses;
    /// <summary>
    /// The number of times that the ability has been used. When the ability reaches its usage limit, it may no longer be used. 
    /// </summary>
    public int numberOfUses { get { return _numberOfUses; } protected set { _numberOfUses = value; } }

    [Header("How many times may this ability be used?")]
    [SerializeField]
    protected int _usageLimit;
    /// <summary>
    /// The number of times that the ability may be used.
    /// </summary>
    public int usageLimit { get { return _usageLimit; } protected set { _usageLimit = value; } }

    /// <summary>
    /// Enables the ability for use, if the number of uses so far hasn't exceeded the usage limit. 
    /// </summary>
    public override void EnableAbility()
    {
        if(numberOfUses >= usageLimit)
        {
            DisableAbility();
            return;
        }

        base.EnableAbility();
    }

    /// <summary>
    /// Ends execution of the ability, ups the number of uses, and disables the ability if the number of uses exceeds the usage limit. 
    /// </summary>
    public override void EndAbility()
    {
        base.EndAbility();

        numberOfUses++;

        if(numberOfUses >= usageLimit)
        {
            DisableAbility();
        }
    }

    /// <summary>
    /// Resets the ability's usage to zero. Call when you want to re-enable an ability for use.
    /// </summary>
    public void ResetUses ()
    {
        numberOfUses = 0;
    }

    /// <summary>
    /// Returns true if the ability can still be used at least once. 
    /// </summary>
    public bool IsUnderUsageLimit ()
    {
        return numberOfUses < usageLimit;
    }
}