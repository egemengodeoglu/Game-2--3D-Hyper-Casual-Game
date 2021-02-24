using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommonUseThemaData", menuName = "Assets/Godemen/ScriptableObjects/CommonUseThemaReferences")]
public class CommonUseThemaReferences : ScriptableObject
{
    public List<PoolObject> road, environment;
    public PoolObject repeater, coinObject;
}
