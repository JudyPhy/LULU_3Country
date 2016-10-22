using UnityEngine;
using System.Collections;

public class OfflineBattlePanel : WindowsBasePanel {

    public override void OnAwake() {
        base.OnAwake();

    }

    public override void OnUpdate() {
        base.OnUpdate();

        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = LayerMask.GetMask("UI");
            if (Physics.Raycast(ray, out hit, 100, mask)) {
                Debug.LogError("11");
                Debug.DrawLine(ray.origin, hit.point);
            } else {
                Debug.LogError("22");
            }
        }
    }

}
