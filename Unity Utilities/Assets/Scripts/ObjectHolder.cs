using UnityEngine;

/// <summary>
/// Place this onto an empty game object and drag in a number of game objects to clear up your hierarchy. 
/// When all child objects have been destroyed, this empty will be destroyed as well. 
/// </summary>
public class ObjectHolder : MonoBehaviour
{
	private void Update ()
    {
		if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
	}
}