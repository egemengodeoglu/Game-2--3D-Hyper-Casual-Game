using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemaData", menuName = "Assets/Godemen/ScriptableObjects/ThemaReferences")]
public class ThemaReferences : ScriptableObject 
{
    public List<PoolObject> road, environment, boats, buildings;
}
