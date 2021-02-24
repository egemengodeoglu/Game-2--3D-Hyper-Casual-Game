using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCarScript : MonoBehaviour
{
    public static SelectedCarScript _instance;
    public static SelectedCarScript Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SelectedCarScript>();
            return _instance;
        }
    }

    public GameObject selected;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

  
}
