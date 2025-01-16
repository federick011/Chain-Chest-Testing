using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Action<Vector3> OnClickScreen = delegate { };

    void Update()
    {
        MouseInputs();
    }

    private void MouseInputs() 
    {
        if (!Input.GetMouseButtonDown(0)) return;

        OnClickScreen?.Invoke(ScreenPointToWorld());
    }

    private Vector3 ScreenPointToWorld() 
    {
        Vector3 _pos = AppManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return _pos;
    }
}
