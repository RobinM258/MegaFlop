using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private ShotgunWeapon shotgunScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shotgunScript = this.GetComponent<ShotgunWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UseWeapon(ItemData weapon)
    {
        if (weapon.id == 1)
            shotgunScript.TryShoot();
    }
}
