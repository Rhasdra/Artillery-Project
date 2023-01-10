using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Weapon/Projectile")]
public class ProjectileSO : ScriptableObject
{
    [Header("Projectile Settings")]
    public float impulse = 1000f;
    public float baseDamage = 500f;
    public float forgiveness = 50f;

    [Header("Components")]
    public ProjectileComponent[] components = new ProjectileComponent[3];

    [Header("Projectile Trail")]
    [SerializeField] GameObject trailPrefab;

    [Header("Explosion")]
    public GameObject[] explosions;
}
