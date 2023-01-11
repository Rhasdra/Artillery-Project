using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Weapon/Projectile")]
public class ProjectileSO : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Projectile Settings")]
    public ProjectileComponentsDatabaseSO database;
    public float impulse = 1000f;
    public float baseDamage = 500f;
    public float forgiveness = 50f;

    [Header("Components")]
    public List<ProjectileComponentSlot> componentSlots = new List<ProjectileComponentSlot>();

    [Header("Projectile Trail")]
    [SerializeField] GameObject trailPrefab;

    [Header("Explosion")]
    public GameObject[] explosions;

    public void AddComponent(ProjectileComponent _component)
    {
        componentSlots.Add(new ProjectileComponentSlot(database.GetId[_component], _component));
    }

    public void OnBeforeSerialize()
    {
        for (int i = 0; i < componentSlots.Count; i++)
        {
            componentSlots[i].component = database.GetObject[componentSlots[i].ID];
        }
    }

    public void OnAfterDeserialize()
    {
    }
}

[System.Serializable]
public class ProjectileComponentSlot
{
    public int ID;
    public ProjectileComponent component;

    public ProjectileComponentSlot(int _id, ProjectileComponent _component)
    {
        ID = _id;
        component = _component;
    }
}
