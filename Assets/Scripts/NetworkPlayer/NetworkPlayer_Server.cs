using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class NetworkPlayer {
    [Header("Player Movement")] [SerializeField]
    private float speed = 11f;

    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private LayerMask groundMask;

    [SerializeField] [SyncVar(hook = nameof(OnHealthUpdate))]
    private int playerHealth = 100;

    [SyncVar(hook = nameof(OnNameUpdate))] private string playerName = "NO_NAME";

    [SyncVar(hook = nameof(OnColorUpdate))]
    private Color _playerColor = Color.red;

    [SyncVar] private float _rotationY;
    [SyncVar] private Vector3 _direction;

    [Command]
    private void Cmd_ChangePlayerName(string newName) => SetName(newName);

    [Server]
    public void SetRandomColor() => _playerColor = Random.ColorHSV();

    [Server]
    public void ReduceHealth(int healthToRemove) => playerHealth -= healthToRemove;

    [Server]
    public void SetName(string newName) => playerName = newName;

    [Command]
    private void Cmd_SetDirection(Vector2 horizontalDirection) =>
        _direction = new Vector3(horizontalDirection.x, _direction.y, horizontalDirection.y);

    [Command]
    private void Cmd_SetRotationY(float y) => _rotationY = y;

    [Command]
    private void Cmd_Jump() {
        if (!IsGrounded()) return;
        _direction.y = (float)Math.Sqrt(-2f * jumpHeight * gravity);
    }

    // [Server]
    // private void OnCollisionEnter(Collision other) {
    //     if (other.gameObject.CompareTag("Player")) {
    //         if (other.gameObject.TryGetComponent<NetworkIdentity>(out var networkIdentity)) {
    //             ICollionWith(networkIdentity.connectionToClient, 12);
    //             playerHealth -= 10;
    //         }
    //     }
    // }

    [Server]
    private void MovePlayer() {
        Vector3 horizontalVelocity =
            (transform.right * _direction.x + transform.forward * _direction.z) * speed;
        horizontalVelocity.y = _direction.y;
        _characterController.Move(horizontalVelocity * Time.deltaTime);

        if (IsGrounded() && _direction.y <= 0) _direction.y = 0;
        else _direction.y += gravity * Time.deltaTime;
    }

    [Server]
    private bool IsGrounded() {
        float halfHeight = _characterController.height * 0.5f;
        var bottomPoint = transform.TransformPoint(_characterController.center - Vector3.up * halfHeight);
        bool isGrounded = Physics.CheckSphere(bottomPoint, 0.1f, groundMask);
        return isGrounded;
    }
}
