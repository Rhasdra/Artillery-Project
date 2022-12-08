using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrail : MonoBehaviour
{
    [Header("References:")]
    public CharManager owner;
    public Transform projectile;
    public LineRenderer lineRenderer;

    [Header("Settings:")]
    [SerializeField] float lineWidth = 0.01f;
    public float trailLife = 1;
    public float timeOfCreation;
    int i = 0;

    private void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        timeOfCreation = Time.time;
    }

    private void OnEnable() 
    {
        lineRenderer.startWidth = lineWidth;
        RandomizeColors();
    }

    private void FixedUpdate() 
    {
        if(projectile == null)
        {
            return;
        }

        lineRenderer.positionCount ++;
        lineRenderer.SetPosition(i, projectile.position);
        i++;
    }

    void RandomizeColors()
    {
        Color start = Random.ColorHSV(0f, 1f, 0f, 1f, 0.5f, 1f, 1f, 1f);
        Color end = Random.ColorHSV(0f, 1f, 0f, 1f, 0.5f, 1f, 1f, 1f);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(start, 0f), new GradientColorKey(end, 1f)},
            new GradientAlphaKey[] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0f)}
        );

        lineRenderer.colorGradient = gradient;
    }
}
