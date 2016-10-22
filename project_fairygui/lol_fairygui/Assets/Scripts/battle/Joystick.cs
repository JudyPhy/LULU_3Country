using UnityEngine;
using System.Collections;
using FairyGUI;
using DG.Tweening;

public class Joystick : EventDispatcher {

    public static Joystick instance;

    private GObject _touchArea;
    private GObject _bg;    
    private GObject _thumb;
    private Tweener _tweener;

    private int touchId;
    public int radius { get; set; }
    private Vector2 _origPos;
    public Vector3 moveDirection {
        get {
            return new Vector3((_thumb.x + _thumb.width / 2) - (_bg.x + _bg.width / 2), 0, (_bg.y + _bg.height / 2) - (_thumb.y + _thumb.height / 2));
        }
    }

    public Joystick(GComponent mainView) {
        instance = this;
        _touchArea = mainView.GetChild("touchArea");
        _bg = mainView.GetChild("joystick_bg");
        _thumb = mainView.GetChild("joystick_thumb");

        _origPos = new Vector2(_bg.x, _bg.y);
        _thumb.x = _bg.x + _bg.width / 2 - _thumb.width / 2;
        _thumb.y = _bg.y + _bg.height / 2 - _thumb.height / 2;
        touchId = -1;
        radius = 60;

        _touchArea.onTouchBegin.Add(this.onTouchDown);
    }

    private void onTouchDown(EventContext context) {
        if (touchId == -1) {
            InputEvent evt = (InputEvent)context.data;
            touchId = evt.touchId;

            if (_tweener != null) {
                _tweener.Kill();
                _tweener = null;
            }

            //左上角为(0,0)
            Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x, evt.y));
            _bg.x = pt.x - _bg.width / 2;
            _bg.y = pt.y - _bg.height / 2;
            _thumb.x = pt.x - _thumb.width / 2;
            _thumb.y = pt.y - _thumb.height / 2;
            _bg.alpha = 0.5f;

            Stage.inst.onTouchMove.Add(this.OnTouchMove);
            Stage.inst.onTouchEnd.Add(this.OnTouchUp);
        }
    }

    private void OnTouchMove(EventContext context) {
        InputEvent evt = (InputEvent)context.data;
        if (touchId != -1 && touchId == evt.touchId) {
            Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x, evt.y));
            float distance = (new Vector2(_bg.x, _bg.y) - pt).magnitude;
            if (distance <= radius) {
                _thumb.x = pt.x - _thumb.width / 2;
                _thumb.y = pt.y - _thumb.height / 2;
            } else {
                float degrees = Mathf.Atan2(pt.y - _bg.y, pt.x - _bg.x);
                _thumb.x = Mathf.Cos(degrees) * radius + _bg.x + _bg.width / 2 - _thumb.width / 2;
                _thumb.y = Mathf.Sin(degrees) * radius + _bg.y + _bg.height / 2 - _thumb.height / 2;
            }
        }
    }

    private void OnTouchUp(EventContext context) {
        InputEvent evt = (InputEvent)context.data;
        if (touchId != -1 && touchId == evt.touchId) {
            touchId = -1;
        }
        _tweener = _thumb.TweenMove(new Vector2(_bg.x + _bg.width / 2 - _thumb.width / 2, _bg.y + _bg.height / 2 - _thumb.height / 2), 0.2f).OnComplete(() => {
            _tweener = null;
            _bg.alpha = 1;
            _bg.x = _origPos.x;
            _bg.y = _origPos.y;
            _thumb.x = _bg.x + _bg.width / 2 - _thumb.width / 2;
            _thumb.y = _bg.y + _bg.height / 2 - _thumb.height / 2;
        });

        Stage.inst.onTouchMove.Remove(this.OnTouchMove);
        Stage.inst.onTouchEnd.Remove(this.OnTouchUp);
    }

    private void Move() { 
    
    }

}
