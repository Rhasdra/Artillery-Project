using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    public const string Default = "Default";
    public const string UI = "UI";
    public const string Terrain = "Terrain";
    public const string Characters = "Characters";
    public const string Projectiles = "Projectiles";
    public const string Guides = "Guides";
    public const string Wind = "Wind";

    public enum NamesToInt
    {
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Raycast = 3,
        Water = 4,
        UI = 5,
        Terrain = 6,
        Characters = 7,
        Projectiles = 8,
        Guides = 9,
        Wind = 10
    }
}
