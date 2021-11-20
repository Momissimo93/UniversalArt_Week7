using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Guns
{
    public string gunName;
    public int additionalDamages;
    public int weight;
    public int inventarySpace;
    public Color projectileColor;
    public int projectileSize;
    private enum GunType {Beretta, Glock}
    private GunType gunType;

    public void setAttributes(string s)
    {
        setGunType(s);

        switch(gunType)
        {
            case GunType.Beretta:
                SetBerettaAttributes();
                break;

            case GunType.Glock:
                SetGlockAttributes();
                break;

            default: break;
        }
    }
    private void setGunType(string s)
    {
        if(s == "Beretta")
        {
            gunType = GunType.Beretta;
        }
        else if (s == "Glock")
        {
            gunType = GunType.Glock;
        }
    }

    private void SetBerettaAttributes()
    {
        gunName = "Beretta";
        additionalDamages = 2;
        weight = 1;
        inventarySpace = 1;
    }

    private void SetGlockAttributes()
    {
        gunName = "Glock";
        additionalDamages = 3;
        weight = 2;
        inventarySpace = 1;
    }
}


