using Mirror;
using TMPro;
using UnityEngine;

public partial class NetworkPlayer {
    private PlayerControl _playerControl;
    private PlayerControl.GroundMovementActions _groundMovementActions;

    [Header("Player Movement")] [SerializeField]
    private float sensitivityX = 8f;
    [SerializeField] private float sensitivityY = 0.1f;

    private float _cameraRotationY;

    [Client]
    public override void OnStartAuthority() {
        OnValidate();
        Cursor.lockState = CursorLockMode.Locked;

        Camera.main.transform.SetParent(_rotationYObject.transform);
        Camera.main.transform.SetLocalPositionAndRotation(new Vector3(0, 0.7f, 0), Quaternion.identity);

        _playerControl = new PlayerControl();
        _playerControl.Enable();
        _groundMovementActions = _playerControl.GroundMovement;

        _groundMovementActions.HorizontalMovement.performed += ctx => Cmd_SetDirection(ctx.ReadValue<Vector2>());
        _groundMovementActions.HorizontalMovement.canceled += ctx => Cmd_SetDirection(Vector2.zero);
        _groundMovementActions.Jump.performed += (ctx) => Cmd_Jump();
        _groundMovementActions.Shoot.performed += (ctx) => _weaponHandler.CMD_StartShooting();
        _groundMovementActions.Shoot.canceled += (ctx) => _weaponHandler.CMD_StopShooting();
        _groundMovementActions.MouseX.performed += (ctx) => RotatePlayerX(ctx.ReadValue<float>(), 0f);
        _groundMovementActions.MouseY.performed += (ctx) => RotatePlayerX(0f, ctx.ReadValue<float>());
        _groundMovementActions.ScrollUp.performed += (ctx) => _weaponHandler.CMD_ChangeWeaponUp();
        _groundMovementActions.ScrollDown.performed += (ctx) => _weaponHandler.CMD_ChangeWeaponDown();
    }

    [Client]
    private void RotatePlayerX(float mouseDeltaX, float mouseDeltaY) {
            
        bool isPaused = GameObject.FindGameObjectWithTag("Event").GetComponent<PauseMenu>().isPaused;

        if (isPaused != true)
        {
            transform.Rotate(Vector3.up, mouseDeltaX * sensitivityX * Time.deltaTime);

            _cameraRotationY -= mouseDeltaY * sensitivityY;
            _cameraRotationY = Mathf.Clamp(_cameraRotationY, -85f, 85f);
            Cmd_SetRotationY(_cameraRotationY);
            _rotationYObject.transform.SetLocalPositionAndRotation(_rotationYObject.transform.localPosition,
                Quaternion.Euler(_cameraRotationY, 0f, 0f));
        }
    }

    [Client]
    private void OnColorUpdate(Color oldColor, Color newColor) => GetComponent<Renderer>().material.color = newColor;

    [Client]
    private void OnNameUpdate(string oldName, string newName) => GetComponentInChildren<TMP_Text>().text = newName;

    [Client]
    private void OnHealthUpdate(int oldHealth, int newHealth) {
        if (isOwned) Debug.Log("I have " + newHealth + " health.");
    }

    [TargetRpc]
    private void ICollionWith(NetworkConnectionToClient target, int nbr) {
        Debug.Log("We had a collision with " + nbr);
        Debug.Log("I have " + playerHealth + " health.");
    }
}
