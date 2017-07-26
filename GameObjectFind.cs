public class Utilities
{
    ///A function for finding a GameObject, placing it in an existing reference, 
    ///and then retrieving any one of its components and placing that inside another existing reference.
    public static void FindAndGetComponent<ComponentType>(ref UnityEngine.GameObject gameObject, 
        string objectName, ref ComponentType component) where ComponentType : UnityEngine.Component
    {
        gameObject = UnityEngine.GameObject.Find(objectName);
        component = gameObject.GetComponent<ComponentType>();
    }
    ///May be useful at runtime for finding and using prefabs/instantiated objects, and for retrieving their components for use. 
    ///Use this method sparingly, ideally only on objects that are inaccessible during edit mode. 
}
