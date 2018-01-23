using UnityEngine;

/// <summary>
/// Attach this class to an object to swap between multiple materials on a timer.
/// </summary>
public class TimedMaterialSwapper : MonoBehaviour
{
    /// <summary>
    /// Holds a material along with the time at which that material changes to the next one. 
    /// </summary>
    [System.Serializable]
    private class TimedMaterial
    {
        [SerializeField]
        private Material _material;

        [SerializeField]
        private float _timeStamp;

        /// <summary>
        /// One of the materials to swap between on a timer.
        /// </summary>
        public Material material { get { return _material; } private set { _material = value; } }
        /// <summary>
        /// The time after which the current material will be swapped out. 
        /// </summary>
        public float timeStamp { get { return _timeStamp; } private set { _timeStamp = value; } }
    }

    /// <summary>
    /// The list of materials and timestamps. 
    /// </summary>
    [SerializeField]
    private TimedMaterial[] materialList;

    /// <summary>
    /// The renderer on this object. 
    /// </summary>
    private Renderer _renderer;

    /// <summary>
    /// The current position in the array. 
    /// </summary>
    [SerializeField]
    private int listPosition = 0;

    /// <summary>
    /// The elapsed time since this object began projecting its current image. 
    /// </summary>
    [SerializeField]
    private float timer = 0;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = materialList[listPosition].material;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > materialList[listPosition].timeStamp)
        {
            listPosition++;
            if(listPosition > materialList.Length - 1)
            {
                listPosition = 0;
            }

            timer = 0;
            _renderer.material = materialList[listPosition].material;
        }
    }
}