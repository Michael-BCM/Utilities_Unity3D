using UnityEngine;
using UnityEditor;

/// Sorts through an array of objects tagged by the user,
/// in order to find the one that is closest to the object that this script is on. 

///<summary>
///At what time should the method run?
/// </summary>
public enum MethodRunTime { Never, OnStart, OnUpdate, OnUserCall }

public class FindNearestGameObjectWithTag : MonoBehaviour
{
    /// <summary>
    /// Tag all of the objects you would like to sort through to find the nearest one, then enter the tag into this string field.
    /// </summary>
    [SerializeField]
    [Header("Tag of objects to locate:")]
    protected string objectTag;

    /// <summary>
    /// This field will be populated with the nearest object once it is located. Can change every frame.
    /// </summary>
    [SerializeField][Header("Closest object this frame:")]
    protected GameObject _closestObject;

    /// <summary>
    /// Use this property to access the closest object from other scripts. 
    /// </summary>
    public GameObject closestObject { get { return _closestObject; } protected set { _closestObject = value; } }
    
    /// <summary>
    /// The distance to the nearest object tagged with the string in the field 'objectTag'. 
    /// </summary>
    [SerializeField][ReadOnly][Header("Distance to nearest object:")]
    protected float NearestObjectDistance;

    [SerializeField][Header("When will this method run?")]
    protected MethodRunTime methodRunTime = MethodRunTime.Never;

    /// <summary>
    /// If the method isn't set to run on the first frame, don't run it here. Otherwise, do so. 
    /// </summary>
    private void Start()
    {
        if(methodRunTime != MethodRunTime.OnStart)
        {
            return;
        }                    
        FindClosestObject();                        
    }

    /// <summary>
    /// If the method isn't set to run on every frame, don't run it here. Otherwise, do so. 
    /// </summary>
    private void Update()
    {
        if (methodRunTime != MethodRunTime.OnUpdate)
        {
            return;
        }
        FindClosestObject();                        
    }

    /// <summary>
    /// The core method. Populates the GameObject '_closestObject' with the nearest GameObject (based on distance)
    /// tagged with the string in the field 'objectTag'.
    /// </summary>
    public void FindClosestObject ()
    {
        if (methodRunTime == MethodRunTime.Never)   ///If the method is set to never run,
            return;                                 ///drop out of this method before it starts.
        
        GameObject[] possibleObjects = GameObject.FindGameObjectsWithTag(objectTag);
        /// Populates an array with all of the objects tagged with the object tag, defined above and in the Unity editor. 

        NearestObjectDistance = Mathf.Infinity;
        ///The current shortest distance is undefined, because we haven't looped through the array yet.

        closestObject = null;
        ///For the same reason, we don't know what the closest object is yet. 

        foreach(GameObject anObject in possibleObjects) ///For each object in the list of objects we'd like to sort through, 
        {
            float distanceToObject = Vector3.Distance(transform.position, anObject.transform.position);
            ///Create a new float, the value of which becomes the distance between the object that this script is on,
            ///and the object in the list that we're currently sorting through.

            if(distanceToObject < NearestObjectDistance) 
            {
                ///In the first iteration of the foreach loop, 
                ///'currentShortestDistance' will change from 'Mathf.Infinity' to the distance from this object to the first object in the list.               
                
                NearestObjectDistance = distanceToObject;

                /// In the second iteration, it will check if the next object* in the list is closer than the first one, 
                ///and if it is, 'currentShortestDistance' will change to the distance between this object and that next object*. 

                /// This will continue indefinitely as long as there are nearer objects to be found. 
                /// Once there are no more, the conditional for the 'if' statement we're in right now will no longer be met, 
                /// and it will be passed over. 

                _closestObject = anObject;
            }
        }
    }
}

public class ReadOnlyAttribute : PropertyAttribute
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}