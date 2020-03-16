using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    //Author: Patrik Ahlgren

    private static DontDestroyOnLoad _instance;

    public static DontDestroyOnLoad Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<DontDestroyOnLoad>();
#if UNITY_EDITOR
                if (FindObjectsOfType<DontDestroyOnLoad>().Length > 1) {
                    Debug.LogError("Found more than one DDOL");
                }
#endif
            }
            return _instance;
        }
    }

    void Start(){
        if (_instance == null) {
            _instance = this;
        }
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
        }
        DontDestroyOnLoad(gameObject); 
    }
}
