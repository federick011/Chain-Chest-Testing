using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjectUIPrefabs", menuName = "ScriptableObjects/UIPrefabs", order = 1)]
public class ScriptableUIPrefabs : ScriptableObject
{
    public GameObject UIManagerObject;

    public GameObject UIPanelPlayPrefab;
}
