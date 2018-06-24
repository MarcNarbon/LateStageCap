using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class inventory : MonoBehaviour {

    shooter weaponController;
    public int maxGaugeAmmo, maxRifleAmmo, maxPistolAmmo;
    public Vector3 ammo;//maxGaugeAmmo, maxRifleAmmo, maxPistolAmmo
    public shooter.weaponType weaponFirst;
    public shooter.weaponType weaponSecond;
    public bool firstWeaponSelected;

   

    // Use this for initialization
    void Start () {
       // ammo = new Vector3(0, 0, 0);//maxGaugeAmmo, maxRifleAmmo, maxPistolAmmo
        firstWeaponSelected = true;

        //weaponFirst = shooter.weaponType.empty;
        //weaponSecond = shooter.weaponType.empty;

    }

    // Update is called once per frame
    void Update ()
    {

        weaponController = GameObject.FindWithTag("controlando").GetComponentInChildren<shooter>();

        

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            firstWeaponSelected = true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            firstWeaponSelected = false;
        }

        if(firstWeaponSelected)
            weaponController.thisWeapon = weaponFirst;
        else
            weaponController.thisWeapon = weaponSecond;


        if (ammo.x > maxGaugeAmmo)
            ammo.x = maxGaugeAmmo;

        if (ammo.y > maxRifleAmmo)
            ammo.y = maxRifleAmmo;

        if (ammo.z > maxPistolAmmo)
            ammo.z = maxPistolAmmo;



    }
}
