using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjectChestSettings", menuName = "ScriptableObjects/ChestSettings", order = 0)]
public class ScriptableChestSettings : ScriptableObject
{
    [Header("Data")]

    public int maxRounds;

    public int minChestInScene;
    public int maxChestInScene;

    public int minCoinsInChest;
    public int maxCoinsInChest;

    [Header("Prefabs")]

    public GameObject prefabChestReference;

    public GameObject prefabChestGameplayManagerReference;

    public ChestsGameplayManager InstantiateChestManager()
    {
        GameObject _clone = Instantiate(prefabChestGameplayManagerReference);

        if (_clone == null) return null;

        ChestsGameplayManager _chestsGameplayManager = _clone.GetComponent<ChestsGameplayManager>();

        return _chestsGameplayManager;
    }

    
}
