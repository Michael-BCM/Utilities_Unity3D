public class Utilities
{
    ///A function for finding a gameobject, placing it in a reference (created by you, the user),
    ///and then retrieving any one of its components and placing that inside a reference as well, also created by you, the user.
    public static void FindAndGetComponent<ComponentType>(ref UnityEngine.GameObject gameObject, 
        string objectName, ref ComponentType component) where ComponentType : UnityEngine.Component
    {
        gameObject = UnityEngine.GameObject.Find(objectName);
        component = gameObject.GetComponent<ComponentType>();
    }
    ///Helpful at runtime for finding and using prefabs/instantiated objects, and for retrieving their components for use.
    ///Use this method sparingly, ideally only on objects that are inaccessible during edit mode. 
}