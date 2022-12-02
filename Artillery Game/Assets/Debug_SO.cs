using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Debug_SO : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    private void OnEnable() {
        inputReader.MoveEvent += DebugInput;
        inputReader.ShootEvent += DebugInput;
        inputReader.LongJumpEvent += DebugInput;
    }

    void DebugInput(Vector2 v)
    {
        Debug.Log(v);
    }

    void DebugInput()
    {
        Debug.Log("Event called");
    }
}
