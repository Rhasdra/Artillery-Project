using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Damage Event Channel")]
public class DamageEventChannelSO : ScriptableObject
{
    public UnityAction<Vector3, int, IDamageable> OnEventRaised = delegate { };

    public void RaiseEvent(Vector3 position, int damage, HealthPool victim)
    {
        OnEventRaised.Invoke(position, damage, victim);
    }
}