using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/Character")]
public class JobSO : ScriptableObject
{
    public string jobName;
    public int maxHP = 3000;

    public float characterWidth = 1f;
    public float movementSpeed = 5f;
    public float climbAngle = 30f;

    public float longJumpForce = 5f;
    public float backFlipJumpForceX = -2f;
    public float backFlipJumpForceY = 10f;

    public float sweetSpotAngleMax = 90f;
    public float sweetSpotAngleMin = 45f;

    public GameObject[] weapons;

    [Header("Art")]
    public Sprite sprite;
    public Vector2 artOffset = new Vector2();
}
