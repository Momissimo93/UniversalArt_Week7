using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns_Manager : MonoBehaviour
{
    [SerializeField] Guns gunInfo;

    void Start()
    {
        SetGunInfo(gameObject.name);
    }
    private void SetGunInfo(string s)
    {
        gunInfo.setAttributes(s);
    }
    public Guns getGun()
    {
        return gunInfo;
    }
}
