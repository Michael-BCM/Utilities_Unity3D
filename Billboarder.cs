using UnityEngine;

/// <summary>
/// Place this MonoBehaviour onto an object to enable billboarding around the selected axis.
/// </summary>
public class Billboarder : MonoBehaviour
{
    /// <summary>
    /// If true, this object will rotate around the 'x' axis. 
    /// </summary>
    [Header("Axes to rotate around.")]
    [SerializeField]
    private bool X;

    /// <summary>
    /// If true, this object will rotate around the 'y' axis. 
    /// </summary>
    [SerializeField]
    private bool Y;

    /// <summary>
    /// If true, this object will rotate around the 'z' axis. 
    /// </summary>
    [SerializeField]
    private bool Z;
        
	private void Update ()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 thisPos = transform.position;
        
        transform.LookAt(new Vector3(
            billboardFloat(X, camPos.x, thisPos.x), 
            billboardFloat(Y, camPos.y, thisPos.y), 
            billboardFloat(Z, camPos.z, thisPos.z)));
	}

    /// <summary>
    /// Returns one of two float values based on the value of 'coOrdinate'.
    /// </summary>
    /// <param name="coOrdinate">The boolean value to check against.</param>
    /// <param name="trueValue">The value to return if 'coOrdinate' is true.</param>
    /// <param name="falseValue">The value to return if 'coOrdinate' is false.</param>
    /// <returns></returns>
    private float billboardFloat (bool coOrdinate, float trueValue, float falseValue)
    {
        if(!coOrdinate)
            return trueValue;

        return falseValue;
    }
}