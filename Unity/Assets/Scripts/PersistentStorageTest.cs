using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentStorageTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string testValue = PersistentStorage.GetString("SmartShare");
        if (string.IsNullOrEmpty(testValue))
        {
            Debug.Log("Key not found. Set test data");
            PersistentStorage.SaveString("SmartShare", "ABCDEFGHIJKLMN");
        }
        else
        {
            Debug.Log($"Key found. Value is: {testValue}");
        }
    }
}
