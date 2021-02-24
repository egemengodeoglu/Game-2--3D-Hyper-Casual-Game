using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Assets/Resources")]
public class GameDataReferences : ScriptableObject
{
    public string playerName;
    public float backgroundVolume, effectsVolume;
    public int carIndex;
    public PoolObject player;
    public int qualityIndex;

    public static GameDataReferences _instance;

    public static GameDataReferences Instance
    {
        get
        {
            if (_instance == null)
            {
                var data = Resources.Load<GameDataReferences>("GameData");
                _instance = data;
            }
            return _instance;
        }

    }


}
