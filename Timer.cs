using UnityEngine;

[System.Serializable]
public class Timer
{
    /// <summary>
    /// The elapsed time since this timer started.
    /// </summary>
    public float time { get { return _time; } private set { _time = value; } }

    [SerializeField]
    private float _time;

    /// <summary>
    /// Resets the timer to zero.
    /// </summary>
    public void Reset() { time = 0; }

    /// <summary>
    /// Counts up by 'interval' once per frame.
    /// </summary>
    public void Count(float interval)
    {
        time += interval;
    }
}