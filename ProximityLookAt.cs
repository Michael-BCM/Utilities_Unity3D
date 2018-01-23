using UnityEngine;

/// <summary>
/// A class for objects that rotate to look at other objects in proximity to them. 
/// </summary>
public class ProximityLookAt : MonoBehaviour
{
    /// <summary>
    /// The default rotation of this object.
    /// </summary>
    private Quaternion defaultRotation;

    /// <summary>
    /// The furthest that the object can be before this object rotates to look at it. 
    /// </summary>
    [SerializeField]
    private float minimumDistance;

    /// <summary>
    /// The speed at which this object will rotate to look at another object, when in proximity to the latter. 
    /// </summary>
    [SerializeField]
    private float lookSpeed;

    /// <summary>
    /// The speed at which this object will rotate back to its original position, when not in proximity to a specific object. 
    /// </summary>
    [SerializeField]
    private float returnSpeed;

    /// <summary>
    /// The tag of the object to rotate towards. 
    /// </summary>
    [SerializeField]
    private string targetTag;

    private void Start()
    {
        defaultRotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, IsObjectInProximity().rotation, IsObjectInProximity().speed * Time.deltaTime);
    }
    
    /// <summary>
    /// If there is an object tagged 'targetTag' within 'minimumDistance' units of this object,
    /// this method returns the direction in which to look towards the object, and the rotation speed to rotate at. 
    /// </summary>
    private LookAtInfo IsObjectInProximity ()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, minimumDistance))
        {
            if (col.tag == targetTag)
            {
                return new LookAtInfo(Quaternion.LookRotation(col.transform.position - transform.position), lookSpeed);
            }
        }
        return new LookAtInfo(defaultRotation, returnSpeed);
    }

    /// <summary>
    /// A class containing a Quaternion value and a float value.
    /// </summary>
    public class LookAtInfo
    {
        public Quaternion rotation { get; private set; }
        public float speed { get; private set; }

        public LookAtInfo(Quaternion _rotation, float _speed)
        {
            rotation = _rotation;
            speed = _speed;
        }
    }
}