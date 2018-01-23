using UnityEngine;

///A FizzBuzz solution, created in C# for Unity.

/// 1. Place this script onto a GameObject. 
/// 2. Navigate to the Inspector and set the highest number you'd like to count to.
/// 3. Set the number of multiples you'd like to use by setting the size of the 'Fizz Buzz Pairs' array. 
/// 4. For each element in the array, set a multiple you'd like to swap out for a word, 
///    and the word you'd like to substitute for said multiple.
/// 5. Hit 'Play', and see the Console for results. 

public class FizzBuzz : MonoBehaviour
{
    /// <summary>
    /// The highest number to count to. 
    /// </summary>
    [SerializeField][Header("Number to count to:")]
    private int highestNumber;

    /// <summary>
    /// A list of multiples. Each is paired with the word that will take its place.
    /// </summary>
    [SerializeField][Header("Multiples and substitutions")]
    private FizzBuzzPair[] fizzBuzzPairs;

    /// <summary>
    /// For iterating through the loop, and for printing out numbers that have no substitutions. 
    /// </summary>
    private static int currentNumber;

    private void Start()
    {
        int actualHighestNumber = highestNumber + 1;

        for (currentNumber = 1; currentNumber < actualHighestNumber; currentNumber++)
            Debug.Log(FizzBuzzString());
    }

    private string FizzBuzzString ()
    {
        string output = "";

        foreach (FizzBuzzPair pair in fizzBuzzPairs)
        {
            pair.SetNum();

            if (pair.num)
                output += pair.name + " ";
        }

        if (output != "")
            return output;

        return currentNumber.ToString();
    }

    [System.Serializable]
    public class FizzBuzzPair
    {
        [Header("A multiple of this number...")]
        public int multiple;
        public bool num { get; private set; }
        [Header("will be substituted with this word:")]
        public string name;

        public FizzBuzzPair (int _multiple, string _output)
        {
            multiple = _multiple;
            name = _output;
        }
        public void SetNum ()
        {
            num = currentNumber % multiple == 0;
        }
    }
}