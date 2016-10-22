using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    private Camera mainCamera;
    private CharacterController _cc;

    private float _moveSpeed = 0.1f;
    private float gravity = 8;
    private Vector3 _moveDirection;
    private float _height = 8f;

    protected override void OnAwake() {
        base.OnAwake();

        mainCamera = Camera.main;
        _cc = this.gameObject.AddComponent<CharacterController>();
    }

    // Use this for initialization
    void Start() {

    }

    protected override void OnUpdate() {
        base.OnUpdate();

        Move();
    }

    private void Move() {
        if (_cc == null) {
            return;
        }
        if (_cc.isGrounded) {
            _moveDirection = Joystick.instance.moveDirection;
            _moveDirection *= _moveSpeed;
        }
        _moveDirection.y -= gravity * Time.deltaTime;
        _cc.Move(_moveDirection * Time.deltaTime);
        float rotate = 180 * (Mathf.Atan2(_moveDirection.x, _moveDirection.z)) / Mathf.PI;
        if (_moveDirection.x == 0 && _moveDirection.z == 0) {
            _ani.SetInteger("Status", 0);
        } else {
            _ani.SetInteger("Status", 1);
            transform.localEulerAngles = new Vector3(0, rotate, 0);
        }
    }

    protected override void OnLateUpdate() {
        base.OnLateUpdate();

        Vector3 direction = Camera.main.gameObject.transform.position - this.transform.position;
        RaycastHit hitInfo;
        int layer = LayerMask.NameToLayer("Scene");
        if (Physics.Raycast(this.transform.position, direction, out hitInfo, layer)) {
            //Debug.LogError("hitInfo:" + hitInfo.collider.gameObject.name);
            Vector3 hitPoint = hitInfo.point;
            mainCamera.transform.position = hitPoint;
        } else {
            mainCamera.transform.position = transform.position + new Vector3(0, _height, -10);
        }
        mainCamera.transform.localEulerAngles = new Vector3(30, 0, 0);
    }

}
