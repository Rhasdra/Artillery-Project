using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharController : MonoBehaviour
{
    public CharSO charInfo;

    public UnityEvent OnLongJumpPress;
    public UnityEvent OnBackFlipPress;
    public UnityEvent<Vector2> OnMovementValueChanged;

    public UnityEvent OnFirePress;

    public UnityEvent StartTurn;
    public UnityEvent EndTurn;

    [SerializeField] bool debug = false;

}
