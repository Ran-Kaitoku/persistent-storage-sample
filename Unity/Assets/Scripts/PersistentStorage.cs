using System;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class PersistentStorage
{
    #if UNITY_ANDROID && !UNITY_EDITOR
    private static readonly string _className = "jp.smartshare.shared_preferences.SharedPreferencesManager";
    private static readonly string _unityClassName = "com.unity3d.player.UnityPlayer";
    private static readonly string _androidActivityName = "currentActivity";
    #elif UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool saveToKeychainWithKey(string key, string data);

    [DllImport("__Internal")]
    private static extern string getFromKeychainWithKey(string key);
    
    [DllImport("__Internal")]
    private static extern bool deleteFromKeychainWithKey(string key);
    #endif
    public static void SaveString(string key, string value, bool needsEncrypted = false)
    {
        #if UNITY_EDITOR
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
        #elif UNITY_ANDROID
        using var javaClass = new AndroidJavaClass(_className);
        using var context =
            new AndroidJavaObject(_unityClassName).GetStatic<AndroidJavaObject>(_androidActivityName);
        javaClass.CallStatic("saveString", context, key, value, needsEncrypted ? 1 : 0);
        #elif UNITY_IOS
        _ = saveToKeychainWithKey(key, value);
        #endif
    }

    public static string GetString(string key, string defaultValue = "", bool isEncrypted = false)
    {
        #if UNITY_EDITOR
        return PlayerPrefs.GetString(key, defaultValue);
        #elif UNITY_ANDROID
        using var javaClass = new AndroidJavaClass(_className);
        using var context =
            new AndroidJavaObject(_unityClassName).GetStatic<AndroidJavaObject>(_androidActivityName);
        return javaClass.CallStatic<string>("getString", context, key, defaultValue, isEncrypted ? 1 : 0);
        #elif UNITY_IOS
        return getFromKeychainWithKey(key) ?? defaultValue;
        #endif
    }

    public static bool Remove(string key, bool isEncrypted = false)
    {
        #if UNITY_EDITOR
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
        return true;
        #elif UNITY_ANDROID
        using var javaClass = new AndroidJavaClass(_className);
        using var context =
            new AndroidJavaObject(_unityClassName).GetStatic<AndroidJavaObject>(_androidActivityName);
        return javaClass.CallStatic<bool>("remove", context, key, isEncrypted ? 1 : 0);
        #elif UNITY_IOS
        return deleteFromKeychainWithKey(key);
        #endif
    }
}