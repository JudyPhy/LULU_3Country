//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using UnityEngine.Events;

//public class MainUI : WindowsBasePanel {

//    private GameObject Center_;
//    private GameObject MainBG_;
//    private GameObject PveEnterCollider_;

//    private GameObject Bottom_;
//    private Button BtnHome_;
//    private Button BtnHeros_;
//    private Button BtnBag_;

//    public override void OnAwake() {
//        //地图
//        this.Center_ = this.transform.FindChild("Center").gameObject;
//        this.MainBG_ = this.Center_.transform.FindChild("MainBG").gameObject;
//        this.PveEnterCollider_ = this.MainBG_.transform.FindChild("PveEnter/ClickBoxCollider").gameObject;

//        this.AddObjClickEvent(this.MainBG_, EventTriggerType.BeginDrag, new UnityAction<BaseEventData>(DragMapStart));
//        this.AddMoreEvent(this.MainBG_, EventTriggerType.Drag, new UnityAction<BaseEventData>(DragingMap));
//        this.AddMoreEvent(this.MainBG_, EventTriggerType.EndDrag, new UnityAction<BaseEventData>(DragMapOver));
//        this.AddMoreEvent(this.MainBG_, EventTriggerType.PointerDown, new UnityAction<BaseEventData>(OnClickMap));
//        this.AddObjClickEvent(this.PveEnterCollider_, EventTriggerType.PointerClick, new UnityAction<BaseEventData>(OnClickPveEnter));

//        //底部导航栏
//        this.Bottom_ = this.transform.FindChild("Bottom").gameObject;
//        this.BtnHome_ = this.Bottom_.transform.FindChild("BGGrid/ButtonHome").GetComponent<Button>();
//        this.BtnHeros_ = this.Bottom_.transform.FindChild("BGGrid/ButtonHeros").GetComponent<Button>();
//        this.BtnBag_ = this.Bottom_.transform.FindChild("BGGrid/ButtonBag").GetComponent<Button>();

//        this.AddButtonClickEvent(this.BtnHome_);
//        this.AddButtonClickEvent(this.BtnHeros_);
//        this.AddButtonClickEvent(this.BtnBag_);
//    }

//    public override void OnInitWindow() {
//        base.OnInitWindow();
//    }

//    public override void OnStart() {
//        base.OnStart();
//    }

//    public override void OnButtonClick(GameObject go) {
//        if (go == this.BtnHome_.gameObject) {
//            this.OnClickHome();
//        } else if (go == this.BtnHeros_.gameObject) {
//            this.OnClickHeros();
//        } else if (go == this.BtnBag_.gameObject) {
//            this.OnClickBag();
//        }
//    }

//    private void OnClickHome() {
//        UIManager.Instance.ShowMainWindow<MainUI>(eWindowsID.MainUI);
//    }

//    private void OnClickHeros() {
//        UIManager.Instance.ShowMainWindow<MainUI>(eWindowsID.MainUI);
//    }

//    private void OnClickBag() {

//    }

//    private void OnClickPveEnter(BaseEventData arg) {
//        UIManager.Instance.ShowMainWindow<PveUI>(eWindowsID.PveUI);
//    }

//    public override void OnUpdate() {
//        base.OnUpdate();
//    }


//    //====================================地图滑动相关===================================
//    private bool IsClickMap_ = false;
//    private bool IsOutRange_ = false;
//    private Vector3 StartPos_;
//    private MoveDirection MoveDir_;
//    private void DragMapStart(BaseEventData arg) {
//        this.StartPos_ = Input.mousePosition;
//    }

//    private void DragingMap(BaseEventData arg) {
//        this.IsClickMap_ = false;
//        float direct = Input.mousePosition.x - this.StartPos_.x;
//        this.StartPos_ = Input.mousePosition;
//        if (direct > 0) {
//            //往右
//            this.MoveDir_ = MoveDirection.Right;
//            if (this.MainBG_.transform.localPosition.x + 5 >= 370) {
//                this.IsOutRange_ = true;
//                return;
//            }
//            this.MainBG_.transform.localPosition += new Vector3(5, 0, 0);
//        } else if (direct < 0) {
//            //往左
//            if (this.MainBG_.transform.localPosition.x - 5 <= -370) {
//                this.IsOutRange_ = true;
//                return;
//            }
//            this.MoveDir_ = MoveDirection.Left;
//            this.MainBG_.transform.localPosition -= new Vector3(5, 0, 0);
//        }
//    }

//    private void DragMapOver(BaseEventData arg) {
//        if (this.IsOutRange_) {
//            this.IsOutRange_ = false;
//            return;
//        }
//        Vector3 targetPos = this.MainBG_.transform.localPosition;
//        if (this.MoveDir_ == MoveDirection.Right) {
//            targetPos += new Vector3(50, 0, 0);
//        } else if (this.MoveDir_ == MoveDirection.Left) {
//            targetPos -= new Vector3(50, 0, 0);
//        }
//        iTween.ValueTo(this.gameObject, iTween.Hash("from", this.MainBG_.transform.localPosition.x, "to", targetPos.x, "easytype", iTween.EaseType.easeInOutQuad,
//            "time", 0.4f, "onupdate", "CheckIsClickMap"));
//    }

//    private void CheckIsClickMap(float value) {
//        if (this.IsClickMap_) {
//            this.IsClickMap_ = false;
//            iTween.Stop();
//        } else {
//            if (value >= 370) {
//                value = 370;
//            } else if (value <= -370) {
//                value = -370;
//            }
//            this.MainBG_.transform.localPosition = new Vector3(value, 0, 0);
//        }
//    }

//    private void OnClickMap(BaseEventData arg) {
//        this.IsClickMap_ = true;
//    }




//}
