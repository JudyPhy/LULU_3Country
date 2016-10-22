using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleRoleObj : MonoBehaviour {

    private Animator Animator;    
    private TweenPosition Tween_Pos;

    private BattleRoleData Data;
    private bool HasSwitchToAtkOver = false;
    private bool HasSwitchToHurtOver = false;
    private Vector3 StartPos_run;
    private Vector3 TargetPos_run;

    void Awake() {
        this.Animator = this.GetComponent<Animator>();
        this.Tween_Pos = this.GetComponent<TweenPosition>();
        this.Tween_Pos.enabled = false;
    }

    private void Start() {
    }

    public void InitRole(BattleRoleData data) {
        this.Data = data;
        this.Animator.SetInteger("Status", 0);
        this.transform.localEulerAngles = new Vector3(0, data.IsPlayer ? 90 : -90, 0);
        this.transform.localScale = Vector3.one * 0.8f;
    }

    public BattleRoleData GetData() {
        return this.Data;
    }

    public void MoveStart(BattleRoleObj hurtObj) {
        int targetPos = hurtObj.GetData().Pos;
        List<int> limitePosList = BattleManager.Instance.GetLimitePosListBySearchPath(targetPos);
        List<int> path = SearchPath.QueryPath(this.Data.Pos, targetPos, limitePosList);
        if (path.Count > 0) {
            if (path[path.Count - 1] != targetPos) {
                if (path.Count > 1) {
                    MoveToTargetPos(path[1]);
                } else {
                    Debug.LogError("与目标点不相邻，且无法移动，进入攻击环节");
                    BattleManager.Instance.CurBattleStatus = BattleStatus.BattleAtkerAtk;
                }
            } else {
                if (path.Count > 1) {
                    MoveToTargetPos(path[1]);
                } else {
                    Debug.LogError("与目标点相邻，直接攻击");
                    BattleManager.Instance.CurBattleStatus = BattleStatus.BattleAtkerAtk;
                }
            }
        } else {
            Debug.LogError("移动路径判断有误，无出发点");
            BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewAtkByCurRound;
        }
    }

    private void MoveToTargetPos(int moveTo) {
        this.Animator.SetInteger("Status", 1);
        this.Tween_Pos.from = this.transform.localPosition;
        this.Tween_Pos.to = BattleManager.Instance.QueryPosByIndex(moveTo);
        this.Tween_Pos.enabled = true;
        Quaternion rot = Quaternion.LookRotation(this.Tween_Pos.to - this.Tween_Pos.from);
        this.transform.localRotation = rot;
        //Debug.LogError("old pos:" + this.Data.QueryPos() + "  new pos:" + moveTo);
        this.Data.Pos = moveTo;
        StartCoroutine(MoveOver());
    }

    //移动tween动画播放完切换为Idle状态
    private IEnumerator MoveOver() {
        //Debug.LogError("MoveOver");
        yield return new WaitForSeconds(this.Tween_Pos.duration);
        this.transform.localEulerAngles = new Vector3(0, this.Data.IsPlayer ? 90 : -90, 0);
        this.Tween_Pos.enabled = false;
        this.Animator.SetInteger("Status", 0);
        BattleManager.Instance.CurBattleStatus = BattleStatus.BattleAtkerAtk;
    }

    //移动动画切换后等待一段时间切换为攻击状态（不能立即切，否则动画播放有误）
    public IEnumerator AtkStart(BattleRoleObj hurtObj) {
        yield return new WaitForSeconds(0.2f);
        Vector3 hurtRoleVec = BattleManager.Instance.QueryPosByIndex(hurtObj.GetData().Pos);
        Quaternion rot = Quaternion.LookRotation(hurtRoleVec - this.transform.localPosition);
        this.transform.localRotation = rot;
        this.HasSwitchToAtkOver = false;
        this.Animator.SetInteger("Status", 3);
        BattleManager.Instance.CurBattleStatus = BattleStatus.BattleHurt;
    }

    //攻击动画播放完切回idle状态
    private IEnumerator AtkOver() {
        yield return new WaitForSeconds(0.3f);
        this.transform.localEulerAngles = new Vector3(0, this.Data.IsPlayer ? 90 : -90, 0);
        this.Animator.SetInteger("Status", 0);
        yield return new WaitForSeconds(0.3f);
        BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewAtkByCurRound;
    }

    public IEnumerator BeHurt() {
        yield return new WaitForSeconds(0.2f);
        this.HasSwitchToHurtOver = false;
        int leftHp = this.Data.CurHp - this.Data.TempDecHpByHurt;
        if (leftHp <= 0) {
            //死亡
            //Debug.LogError("死亡:" + this.Data.QueryPos());
            this.Animator.SetInteger("Status", 4);
        } else {
            //受击
            //Debug.LogError("受击:" + this.Data.QueryPos());
            this.Animator.SetInteger("Status", 2);
        }
        GameObject go = Instantiate(Resources.Load("DropHpLabel")) as GameObject;
        DropHpLabelAni label = go.AddComponent<DropHpLabelAni>();
        label.transform.position = this.transform.position;
        label.Play(this.Data.TempDecHpByHurt);
        this.Data.CurHp = leftHp <= 0 ? 0 : leftHp;
        this.Data.TempDecHpByHurt = 0;
    }

    private IEnumerator HurtOver() {
        yield return new WaitForSeconds(0.5f);
        this.Animator.SetInteger("Status", 0);
    }

    private IEnumerator Death() {
        yield return new WaitForSeconds(0.5f);
        this.Animator.SetInteger("Status", 0);
        this.gameObject.SetActive(false);
    }

    void Update() {
        switch (this.Animator.GetInteger("Status")) {
            case 1: {
                    //奔跑动画                
                }
                break;
            case 2: {
                    //受击动画
                    if (!this.HasSwitchToHurtOver && this.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                        this.HasSwitchToHurtOver = true;
                        StartCoroutine(HurtOver());
                    }
                }
                break;
            case 3: {
                    //攻击动画
                    if (!this.HasSwitchToAtkOver && this.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                        //攻击动画结束
                        //Debug.LogError("攻击动画结束");
                        this.HasSwitchToAtkOver = true;
                        StartCoroutine(AtkOver());
                    }
                }
                break;
            case 4: {
                    //死亡动画
                    if (this.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                        StartCoroutine(Death());
                    }
                }
                break;
            default:
                break;
        }
    }


}


public class DropHpLabelAni : MonoBehaviour {

    private UILabel Label;
    private TweenAlpha Tween_Alpha;
    private TweenPosition Tween_Pos;
    private TweenScale Tween_Scale;

    private void Awake() {
        this.Label = this.GetComponent<UILabel>();
        this.Tween_Alpha = this.GetComponent<TweenAlpha>();
        this.Tween_Pos = this.GetComponent<TweenPosition>();
        this.Tween_Scale = this.GetComponent<TweenScale>();
        this.Tween_Pos.enabled = false;
        this.Tween_Alpha.enabled = false;
        this.Tween_Scale.enabled = false;
    }

    public void Play(int hp) {
        this.Label.text = "-" + hp.ToString();
        this.Tween_Pos.from = this.transform.localPosition;
        this.Tween_Pos.to = this.transform.localPosition + new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
        this.Tween_Pos.enabled = true;        
        this.Tween_Scale.enabled = true;
        this.Tween_Alpha.enabled = true;
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(1f);
        DestroyImmediate(this.gameObject);
    }

}
