using UnityEngine;

/// <summary>
/// A meter with a minimum and maximum value, a current value, 
/// and a way to count up or down (by changing the current value) between the minimum and maximum. 
/// </summary>
[System.Serializable]
public class Meter
{
    /// <summary>
    /// The current value. It can never be lower than the minimum value, nor higher than the maximum. 
    /// </summary>
    public float currentValue
    {
        get
        {
            if (_currentValue < minValue)
            {
                _currentValue = minValue;
            }
            if (_currentValue > maxValue)
            {
                _currentValue = maxValue;
            }

            return _currentValue;
        }

        private set
        {
            _currentValue = value;
        }
    }

    /// <summary>
    /// The minimum value. 'currentValue' cannot be lower than this. 
    /// </summary>
    public float minValue { get { return _minValue; } private set { _minValue = value; } }
    /// <summary>
    /// The maximum value. 'currentValue' cannot be lower than this. 
    /// </summary>
    public float maxValue { get { return _maxValue; } private set { _maxValue = value; } }

    [SerializeField]
    private float _currentValue;

    [SerializeField]
    private float _minValue;

    [SerializeField]
    private float _maxValue;

    /// <summary>
    /// Creates a new meter, with a minimum and maximum value, 
    /// and an initial value that must fall in between these two values.
    /// If it doesn't, an error will occur. 
    /// </summary>
    /// <param name="initialValue">The starting value of the meter.</param>
    /// <param name="_minValue">The minimum value of the meter.</param>
    /// <param name="_maxValue">The maximum value of the meter.</param>
    public Meter(float initialValue, float _minValue, float _maxValue)
    {
        if (initialValue < _minValue || initialValue > _maxValue)
        {
            Debug.LogError("Please enter an initial value within the ranges you have provided.");
            return;
        }

        minValue = _minValue;
        maxValue = _maxValue;

        currentValue = initialValue;
    }

    /// <summary>
    /// Counts in either direction along the meter, by 'interval'. 
    /// Use a negative number to count backwards, and vice versa. 
    /// </summary>
    /// <param name="interval">
    /// The number to count up or down by, per frame. 
    /// Multiply by 'Time.deltaTime' to count by the interval per second instead.
    /// </param>
    public void Count(float interval)
    {
        if (currentValue > maxValue)
        {
            ResetToMax();
        }
        if (currentValue < minValue)
        {
            ResetToMin();
        }

        currentValue += interval;
    }

    /// <summary>
    /// Resets the timer to the minimum value.
    /// </summary>
    public void ResetToMin()
    {
        currentValue = minValue;
    }

    /// <summary>
    /// Resets the timer to the maximum value.
    /// </summary>
    public void ResetToMax()
    {
        currentValue = maxValue;
    }
}