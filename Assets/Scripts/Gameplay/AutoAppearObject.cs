using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAppearObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToAppearInStart;
    void Start()
    {
        if(objectToAppearInStart != null) objectToAppearInStart.SetActive(true);
    }
}
