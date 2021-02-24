using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemaChooserScript : MonoBehaviour
{
    public Button beach, street;
    public Action<String> OnThemaEvent;

    public void ThemaChooser(string tmp)
    {
        OnThemaEvent.Invoke(tmp);
    }


}
