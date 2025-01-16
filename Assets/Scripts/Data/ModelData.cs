using System;
using UnityEngine;

[Serializable]
public class ChestSettingData 
{
    public string name;

    public int indexID;

    public GameObject chestObject;

    public Animator chestAnimator;

    public int chestCoins = 0;

    public Vector2 chestPosition = Vector2.zero;
}


