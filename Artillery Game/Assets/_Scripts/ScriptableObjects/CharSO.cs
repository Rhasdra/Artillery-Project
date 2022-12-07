using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/Character")]
public class CharSO : ScriptableObject
{
    public string characterName;
    public int maxHP = 3000;

    public float characterWidth = 1f;
    public float movementSpeed = 5f;
    public float climbAngle = 30f;

    public float longJumpForce = 5f;
    public float backFlipJumpForceX = -2f;
    public float backFlipJumpForceY = 10f;

    public float sweetSpotAngleMax = 90f;
    public float sweetSpotAngleMin = 45f;

    public WeaponSO[] weaponsSO;
}
