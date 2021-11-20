using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Reliques
{
    public string reliqueName;
    public int powerAttribute;
    public int weight;
    public int inventarySpace;
    public Color reliqueColor;

    public enum PowerUpType {JumpingPower, Whip};
    public PowerUpType powerUpType;

    public string GetPowerUpType()
    {
        return powerUpType.ToString();
    }
}
