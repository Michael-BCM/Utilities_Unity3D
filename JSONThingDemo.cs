/// A quick summary of a tutorial by GameGrind: https://www.youtube.com/watch?v=6yDRbnXve_0 (thanks!)
/// on how to use the JSONUtility to save and load data to PC games and applications built using Unity3D.
/// 
/// GameGrind channel: https://www.youtube.com/channel/UCTY3kks3U4RDvpMX87fvo1A, like and subscribe to them to see more of their content. 
/// 
/// Summarised by me below. 

using UnityEngine;
using System.IO;

public class JSONThingDemo : MonoBehaviour
{
    private string path;
    private string jsonString;

    private void Start ()
    {
        #region Load and print data to the console. 
        path = Application.streamingAssetsPath + "/Thing.json"; 
        //Sets up the path to read from, ending with the save file name. 
        //Ensure that there is a 'StreamingAssets' folder in your Assets folder, and that there is a file in there with the above name. 

        jsonString = File.ReadAllText(path);
        //Opens said save file, reads the file contents into a string 'jsonString', then closes the file.

        Thing Something = JsonUtility.FromJson<Thing>(jsonString);
        //Creates a new instance of the 'Thing' class, and writes the contents of the string into the class. 

        print(Something.Name + ", " + Something.Level);
        //The lines taken from the json file, once written to the class, are displayed in the console.
        #endregion

        #region Modify and save data to the JSON file. 
        Something.Level = 3146;
        //Sets the 'level' statistic to something different. 

        string newSomething = JsonUtility.ToJson(Something);
        //Takes the modified data and generates a json representation of it. 

        File.WriteAllText(path, newSomething);
        //Writes the json representation back to the original file.
        #endregion

        print(newSomething);
    }
}

[System.Serializable]
public class Thing
{
    public string Name;
    public int Level;
    public int[] Stats;
}