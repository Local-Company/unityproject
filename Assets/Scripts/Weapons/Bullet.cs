using System;
using System.Collections;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : NetworkBehaviour {
    public event Action<Collision, Bullet> OnCollision;

    private new void OnValidate() {
        if (GetComponent<Rigidbody>() == null) throw new Exception("Bullet must have a rigidbody component");
    }

    [Server]
    public void SetDirection(Vector3 direction) =>
        GetComponent<Rigidbody>().AddForce(direction, ForceMode.VelocityChange);

    [ServerCallback]
    private void OnCollisionEnter(Collision other) => OnCollision?.Invoke(other, this);
}
