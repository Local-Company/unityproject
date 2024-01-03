using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour {
    [SerializeField] private List<GameObject> weaponsPrefab;
    
    [SyncVar(hook = nameof(OnWeaponChange))]
    private int _currentWeaponIndex;

    [CanBeNull] private GameObject _currentWeapon;
    [CanBeNull] private Guns _currentWeaponGuns;

    private new void OnValidate() {
        if (weaponsPrefab.Count == 0) Debug.LogError("No weapons in the list");
        foreach (var weapon in weaponsPrefab)
            if (weapon.GetComponent<Guns>() == null)
                Debug.LogError($"Weapon {weapon.name} doesn't have Guns script");
    }
    
    [Client]
    private void OnWeaponChange(int oldIndex, int newIndex) {
        Destroy(_currentWeapon);
        _currentWeapon = Instantiate(weaponsPrefab[newIndex], transform);
        _currentWeaponGuns = _currentWeapon!.GetComponent<Guns>();
    }

    [ClientRpc]
    public void CMD_ShootAudio() {
        if (_currentWeaponGuns)
            _currentWeaponGuns.ShootAudio();
    }
    
    [Command]
    public void CMD_ChangeWeaponUp() {
        if (_currentWeaponIndex + 1 >= weaponsPrefab.Count) _currentWeaponIndex = 0;
        else _currentWeaponIndex += 1;
    }
    
    [Command]
    public void CMD_ChangeWeaponDown() {
        if (_currentWeaponIndex - 1 < 0) _currentWeaponIndex = weaponsPrefab.Count - 1;
        else _currentWeaponIndex -= 1;
    }

    [Command]
    public void CMD_StartShooting() => _currentWeaponGuns?.StartShooting();

    [Command]
    public void CMD_StopShooting() => _currentWeaponGuns?.StopShooting();
}
