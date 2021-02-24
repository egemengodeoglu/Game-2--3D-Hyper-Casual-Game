using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "Players", menuName = "Assets/Godemen/ScriptableObjects/PlayersReferences")]
public class CarsReferences : ScriptableObject
{
    public List<PoolObject> cars;
}
