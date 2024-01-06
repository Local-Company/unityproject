using System;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

[RequireComponent(typeof(NetworkIdentity)), RequireComponent(typeof(AudioSource))]
public class Guns : NetworkBehaviour {
    [Header("Bullet")] [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float bulletLifeTime = 5f;
    [SerializeField] private int bulletDamage = 10;
    [SerializeField] private bool bulletBounce;
    [SerializeField] private AudioClip audioClip;


    [Header("Gun")] [SerializeField] private float fireRate = 1f;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private int magazineSize = 10;
    [SerializeField] public int currentAmmo = 100;
    [SerializeField] private bool isAutomatic;
    public int _currentMagazine;
    private bool _isReloading;
    private bool _isFiring;

    private GameObject _shootPoint;
    private AudioSource _audioSource;
    private WeaponHandler _weaponHandler;

    private void Start() {
        _shootPoint = transform.Find("ShootPoint").gameObject;
        _currentMagazine = magazineSize;
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource) _audioSource.spatialBlend = 1;
        _weaponHandler = transform.parent.gameObject.GetComponent<WeaponHandler>();
    }

    private new void OnValidate() {
        if (transform.Find("ShootPoint") == null)
            Debug.LogError($"Weapon {name} doesn't have ShootPoint child");
    }

    [Server]
    private void Reload() {
        if (_isReloading || currentAmmo <= 0 || _currentMagazine == magazineSize) return;
        _isReloading = true;
        Invoke(nameof(ResetReload), reloadTime);
        var ammoToReload = magazineSize - _currentMagazine;
        if (currentAmmo < ammoToReload) ammoToReload = currentAmmo;
        _currentMagazine += ammoToReload;
        currentAmmo -= ammoToReload;
    }

    [Server]
    public void StartShooting() {
        if (isAutomatic) InvokeRepeating(nameof(Shoot), 0f, fireRate);
        else Shoot();
    }

    [Server]
    public void StopShooting() {
        if (isAutomatic) CancelInvoke(nameof(Shoot));
    }

    [Server]
    public void Shoot() {
        bool isPaused = GameObject.FindGameObjectWithTag("Event").GetComponent<PauseMenu>().isPaused;

        if (_isReloading || _isFiring || isPaused) return;
        if (_currentMagazine <= 0) {
            Reload();
            return;
        }

        _currentMagazine--;
        _isFiring = true;
        Invoke(nameof(ResetFire), fireRate);

        var bulletGo = Instantiate(bulletPrefab, _shootPoint.transform.position, _shootPoint.transform.rotation);

        if (bulletGo.TryGetComponent<Bullet>(out var bulletBehaviour)) {
            bulletBehaviour.SetDirection(bulletGo.transform.forward * bulletSpeed);
            bulletBehaviour.OnCollision += OnBulletCollision;
        }

        NetworkServer.Spawn(bulletGo);
        Destroy(bulletGo, bulletLifeTime);
        _weaponHandler.CMD_ShootAudio();
    }

    [Client]
    public void ShootAudio(){
        if (_audioSource && audioClip)
            _audioSource.PlayOneShot(audioClip);
    }

    [Server]
    private void ResetFire() => _isFiring = false;

    [Server]
    private void ResetReload() => _isReloading = false;

    [Server]
    private void OnBulletCollision(Collision collision, Bullet bullet) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.gameObject.TryGetComponent<NetworkIdentity>(out var networkIdentity)) {
                if (networkIdentity.gameObject.TryGetComponent<NetworkPlayer>(out var networkPlayer)) {
                    networkPlayer.ReduceHealth(bulletDamage);
                    Debug.Log("je suis touché");
                    GameObject.FindGameObjectWithTag("Event").GetComponent<comboANDscore>().ResetCombo();
                    NetworkServer.Destroy(bullet.gameObject);
                }
            }
        }

        if (!bulletBounce) NetworkServer.Destroy(bullet.gameObject);
    }
}
