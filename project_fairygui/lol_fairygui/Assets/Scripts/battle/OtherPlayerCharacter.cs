using UnityEngine;
using System.Collections;

public class OtherPlayerCharacter : Character {

    private float _moveSpeed = 0.1f;
    private float gravity = 80;
    private Vector3 _moveDirection;

    protected override void OnAwake() {
        base.OnAwake();

        _moveDirection = Vector3.zero;
    }

    protected override void OnUpdate() {
        base.OnUpdate();
    }

}
