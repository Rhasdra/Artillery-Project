using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Damage Event Channel")]
public class DamageEventChannelSO : ScriptableObject
{
    public UnityAction<Vector3, int, HealthPool> OnEventRaised;

    public void RaiseEvent(Vector3 position, int damage, HealthPool victim)
    {
        OnEventRaised.Invoke(position, damage, victim);
    }
}