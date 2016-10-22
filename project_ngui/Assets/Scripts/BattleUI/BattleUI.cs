using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleUI : WindowsBasePanel {

    private List<ActorManager> EnermyList_ = new List<ActorManager>();

    public override void OnAwake() {
        base.OnAwake();
    }

    public override void OnStart() {
        base.OnStart();
        AddPlayerToScene();
    }

    public override void OnInitWindow() {
        base.OnInitWindow();
    }

    public override void OnUpdate() {
        base.OnUpdate();
    }

    private void AddPlayerToScene() {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Models/enermy01")) as GameObject;
        ActorManager script = obj.AddComponent<ActorManager>();
        this.EnermyList_.Add(script);
    }






}
