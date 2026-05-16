using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private ShotgunWeapon shotgunScript;
    private Laser laserGunScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shotgunScript = this.GetComponent<ShotgunWeapon>();
        laserGunScript = this.GetComponent<Laser>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UseWeapon(ItemData weapon)
    {
        if (weapon.id == 0)
        {
            //Debug.Log("ShotGun");
            shotgunScript.TryShoot();
        }

        if (weapon.id == 1)
        {
            //Debug.Log("Laser");
            laserGunScript.TryShoot();
        }
    }
}
