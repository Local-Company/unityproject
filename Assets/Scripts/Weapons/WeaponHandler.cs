using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour {
    [SerializeField] private List<GameObject> weaponsPrefab;

    private GameObject _currentWeapon;
    private Guns _currentWeaponGuns;

    private void Start() {
        _currentWeapon = Instantiate(weaponsPrefab[0], transform);
        _currentWeaponGuns = _currentWeapon.GetComponent<Guns>();
    }

    private new void OnValidate() {
        if (weaponsPrefab.Count == 0) Debug.LogError("No weapons in the list");
        foreach (var weapon in weaponsPrefab)
            if (weapon.GetComponent<Guns>() == null)
                Debug.LogError($"Weapon {weapon.name} doesn't have Guns script");
    }

    [Command]
    public void CMD_StartShooting() => _currentWeaponGuns.StartShooting();

    [Command]
    public void CMD_StopShooting() => _currentWeaponGuns.StopShooting();
}
