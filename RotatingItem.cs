using UnityEngine;

/// <summary>
/// The base class for all objects that rotate when idle. 
/// </summary>
public class RotatingItem : MonoBehaviour
{
    /// <summary>
    /// Speed at which to rotate.
    /// </summary>
    [Header("Rotation Settings")]
    [SerializeField]
    protected float rotationSpeed;

    /// <summary>
    /// Axis to rotate around. 
    /// </summary>
    [SerializeField]
    protected Vector3 upwardsDirection;

    /// <summary>
    /// Is this object shrinking into nothing?
    /// </summary>
    public bool shrink { get; protected set; }

    /// <summary>
    /// The rate at which the rotation speed increases when this object is shrinking into nothing. 
    /// </summary>
    [SerializeField]
    protected float rotationAccelerationRate;

    /// <summary>
    /// The speed at which this object reduces in size when shrinking into nothing. 
    /// </summary>
    [SerializeField]
    protected float shrinkSpeed;

    protected virtual void FixedUpdate()
    {
        transform.Rotate(upwardsDirection * Time.deltaTime * rotationSpeed);
    }

    protected virtual void Update()
    {
        if(!shrink)
        {
            return;
        }

        ShrinkAndDestroy();
    }

    /// <summary>
    /// Changes the rotation speed by the parameter 'amount' per frame. 
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeRotationSpeed(float amount)
    {
        rotationSpeed += amount;
    }

    /// <summary>
    /// Triggers shrinking and subsequent destruction of this object. 
    /// </summary>
    public void BeginShrink()
    {
        shrink = true;
    }

    /// <summary>
    /// Execute in Update. Shrinks the object into nothing and removes it from the scene.
    /// </summary>
    public virtual void ShrinkAndDestroy()
    {
        if (transform.localScale.x <= 0 && transform.localScale.y <= 0 && transform.localScale.z <= 0)
        {
            Destroy(gameObject);
        }
        
        transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;
        
        ChangeRotationSpeed(rotationAccelerationRate * Time.deltaTime);
    }
}
