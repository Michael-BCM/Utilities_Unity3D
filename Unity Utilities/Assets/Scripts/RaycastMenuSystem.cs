///A menu system that utilises raycasting, to be used with the left thumbstick of a controller. 
///Ideal for unusally shaped menus that require a push of the thumbstick in an odd direction in order to move the cursor between options. 
///Annotations coming soon. 

using UnityEngine;
using System.Collections.Generic;

public class RaycastMenuSystem : MonoBehaviour
{
    private bool scrollCheck = false;

    [SerializeField]
    private GameObject _currentSelection;
    public GameObject currentSelection { get { return _currentSelection; } }

    [SerializeField]
    private GameObject[] OptionsList;
    
    List<GameObject> otherOptions = new List<GameObject>();

    void Start()
    {
        _currentSelection = OptionsList[0];
    }

    bool AxisIsInUse()
    {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            return true;

        return false;
    }

    void Update()
    {
        if (AxisIsInUse())
        {
            if(!scrollCheck)
            {
                scrollCheck = true;
                Selection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0, _currentSelection, otherOptions);
            }
        }
        else
        {
            scrollCheck = false;
        }
    }

    void Selection(float directionX, float directionY, float directionZ, GameObject originObject, List<GameObject> destinations)
    {
        Vector3 direction = new Vector3(directionX, directionY, directionZ);
        RaycastHit[] hitObjects;
        Ray r = new Ray(originObject.transform.position, direction);
        hitObjects = Physics.RaycastAll(r);
        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (hitObjects[i].transform.gameObject.name != originObject.name)
            {
                destinations.Add(hitObjects[i].transform.gameObject);
                if (i == (hitObjects.Length - 1))
                {
                    GameObject[] destinationsA = destinations.ToArray();
                    GameObject closest = null;
                    float minimumDistance = Mathf.Infinity;
                    Vector3 currentPosition = originObject.transform.position;
                    foreach (GameObject o in destinationsA)
                    {
                        float dist = Vector3.Distance(o.transform.position, currentPosition);

                        if (dist < minimumDistance)
                        {
                            closest = o;
                            minimumDistance = dist;
                            _currentSelection = closest;
                        }
                    }
                    destinations.Clear();
                }
            }
        }
    }
}