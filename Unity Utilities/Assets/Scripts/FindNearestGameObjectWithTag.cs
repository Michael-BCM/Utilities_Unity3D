using UnityEngine;
using UnityEditor;

/// <summary>
/// Sorts through an array of objects tagged by the user, 
/// in order to find the one that is closest to the object that this script is on. 
/// </summary>

public class FindNearestGameObjectWithTag : MonoBehaviour
{
    [SerializeField]
    [Header("Tag of objects to locate:")]
    protected string objectTag;
    /// Tag all of the objects you would like to sort through to find the nearest one, then enter the tag into this string field.

    [SerializeField][Header("Closest object this frame:")]
    protected GameObject _closestObject;
    ///This field will be populated with the nearest object once it is located. Can change every frame.
    
    public GameObject closestObject { get { return _closestObject; } protected set { _closestObject = value; } }
    /// Use this to access the closest object from other scripts. 

    [SerializeField][ReadOnly][Header("Distance to nearest object:")]
    protected float NearestObjectDistance;

    public void Update ()
    {
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

                /// In the second iteration, it will check if the 'next object*' in the list is closer than the first one, 
                ///and if it is, 'currentShortestDistance' will change to the distance between this object and that 'next object*'. 

                /// This will continue indefinitely as long as there are nearer objects to be found. 
                /// Once there are no more, the conditional for the 'if' statement we're in right now will no longer be met, 
                /// and it will be passed over. 

                _closestObject = anObject;
            }
        }
        ///You'll want to run this method in the Update function if the position of this object changes constantly,
        ///or if the positions of the objects in the list do the same. 
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