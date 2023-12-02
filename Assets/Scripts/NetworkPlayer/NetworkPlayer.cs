using Mirror;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public partial class NetworkPlayer : NetworkBehaviour {
    private readonly Logger.ILogger _logger = Logger.Logger.createLogger();

    private CharacterController _characterController;

    private GameObject _rotationYObject;
    private GameObject _weaponHandlerObject;
    private WeaponHandler _weaponHandler;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        OnValidate();
    }

    private new void OnValidate() {
        Renderer playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null) throw new MissingComponentException("No renderer found on NetworkPlayer");

        TMP_Text nameText = GetComponentInChildren<TMP_Text>();
        if (nameText == null) throw new MissingComponentException("No TMP_Text found on NetworkPlayer");

        _characterController = GetComponent<CharacterController>();
        if (_characterController == null)
            throw new MissingComponentException("No CharacterController found on NetworkPlayer");

        _rotationYObject = transform.Find("RotationY").gameObject;
        if (_rotationYObject == null) throw new MissingComponentException("No RotationY found on NetworkPlayer");

        _weaponHandlerObject = _rotationYObject.transform.Find("WeaponHandler").gameObject;
        if (_weaponHandlerObject == null) throw new MissingComponentException("No WeaponHandlerPrefab found on NetworkPlayer");

        _weaponHandler = _weaponHandlerObject.GetComponent<WeaponHandler>();
        if (_weaponHandler == null) throw new MissingComponentException("No WeaponHandler found on NetworkPlayer");
    }

    private void Update() {
        if (isServer) MovePlayer();

        if (isClient) { }

        if (isOwned) { }
    }
}
