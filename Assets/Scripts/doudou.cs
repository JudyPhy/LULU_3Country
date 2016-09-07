using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sphere : MonoBehaviour {
    public float Index_X;
    public int Index_Y;
    private Color Color;

    public void SetIndex(float x, int y) {
        this.Index_X = x;
        this.Index_Y = y;
        this.name = this.Index_X + "_" + this.Index_Y;
    }

    public void SetPos(float x, float y) {
        this.transform.localPosition = new Vector3(x, y, 0);
    }
}

public class doudou : MonoBehaviour {

    public static doudou Instance;

    private GameObject ClickBoxCollider;

    private List<Sphere> SphereList = new List<Sphere>();
    private int Radio = 5;      //只能是奇数
    private float SphereDia = 0.45f;

    [HideInInspector]
    public GameObject SphereSelf;
    private float MoveK;
    private bool IsPlayingBall = false;
    private bool MoveStart = false;
    private bool IsUp = false;
    private float SphereSpeedX = 0.2f;      //小球发射后的运动速度
    private float LIMITE_HALFWIDTH = 5.3f;
    private float LIMITE_HEIGHT = 4.6f;
    
    private float Index_Drop_X = -99;
    private int Index_Drop_Y = -99;
    private float ToTargetPosDistanceLimite = 1f;

    void Awake() {
        Instance = this;
        this.SphereSelf = GameObject.Find("SelfSphere").gameObject;
        this.ClickBoxCollider = GameObject.Find("UI Root").transform.FindChild("Panel/BG").gameObject;
        UIEventListener.Get(this.ClickBoxCollider).onClick = OnClick;
    }

    void Start() {
        InitSelfSphere();
        InitSphere();        
        this.IsPlayingBall = false;
    }

    //初始摆放小球
    private void InitSphere() {
        float index_x = 0;
        int index_y = 0;
        int lineCount = Radio * 2 - 1;      //最初中央行内球个数
        while (index_y < Radio) {
            //Debug.LogError("index_y:" + index_y);
            int sphere_index = 0;
            if (lineCount % 2 != 0) {
                //奇数行
                //center sphere              
                CreateInitSphere(0, index_y);
                sphere_index++;
                index_x = 1;
            } else {
                index_x = 0.5f;
            }
            //other sphere
            while (sphere_index < lineCount) {
                //right
                CreateInitSphere(index_x, index_y);
                sphere_index++;
                //left
                CreateInitSphere(-index_x, index_y);
                sphere_index++;
                index_x++;
            }
            lineCount--;
            index_y++;
        }
    }

    private Sphere GetSphere() {
        GameObject obj = Instantiate(Resources.Load("Sphere")) as GameObject;
        obj.transform.parent = this.transform;
        obj.transform.localScale = Vector3.one * SphereDia;
        Sphere script = obj.AddComponent<Sphere>();
        SphereList.Add(script);
        return script;
    }

    private void CreateInitSphere(float index_x, int index_y) {
        //Debug.LogError("CreateSphere:" + index_x + "  " + index_y);
        Sphere script = GetSphere();
        script.SetIndex(index_x, index_y);
        float posX = index_x * SphereDia;
        float posY = index_y * SphereDia * Mathf.Sqrt(3f) / 2f;
        script.SetPos(posX, posY);
        if (Mathf.Abs(index_y) > 0) {         
            script = GetSphere();
            script.SetIndex(index_x, -index_y);
            script.SetPos(posX, -posY);
        } else {
            script.SetPos(posX, posY);
        }
    }

    private Sphere QuerySphere(float index_x, int index_y) {
        for (int i = 0; i < this.SphereList.Count; i++) {
            if (this.SphereList[i].Index_X == index_x && this.SphereList[i].Index_Y == index_y) {
                return this.SphereList[i];
            }
        }
        return null;
    }

    private void OnClick(GameObject go) {
        if (!this.IsPlayingBall) {
            this.IsPlayingBall = true;
            Vector3 tempOrigPos = this.SphereSelf.transform.position;
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clickPos.y >= tempOrigPos.y) {
                return;
            }
            this.MoveK = clickPos.x == tempOrigPos.x ? 1 : (clickPos.y - tempOrigPos.y) / (clickPos.x - tempOrigPos.x);
            this.IsUp = false;
            this.MoveStart = true;
        }
    }

    //初始化玩家小球
    private void InitSelfSphere() {
        this.SphereSelf.transform.localScale = Vector3.one * SphereDia;
        this.SphereSelf.transform.localPosition = new Vector3(0, this.LIMITE_HEIGHT, 0);
        this.SphereSelf.gameObject.SetActive(true);
        this.Index_Drop_X = -99;
        this.Index_Drop_Y = -99;
        this.IsPlayingBall = false;
    }

    //碰撞生成小球
    private void CreateCollosionSphere(float index_x, int index_y) {
        Debug.LogError("CreateCollosionSphere index_x:" + index_x + "    index_y:" + index_y);
        Sphere script = GetSphere();
        script.SetIndex(index_x, index_y);
        float posX = index_x * SphereDia;
        float posY = index_y * SphereDia * Mathf.Sqrt(3f) / 2f;
        script.SetPos(posX, posY);
        this.SphereList.Add(script);
    }

    void Update() {        
        if (this.MoveStart) {
            Vector3 selfPos = this.SphereSelf.transform.position;
            float tempX = 0;
            if (this.IsUp) {
                float addValueX = this.MoveK > 0 ? this.SphereSpeedX : -this.SphereSpeedX;
                tempX = selfPos.x + addValueX;
            } else {
                float addValueX = this.MoveK > 0 ? -this.SphereSpeedX : this.SphereSpeedX;
                tempX = selfPos.x + addValueX;
            }
            //Debug.LogError("tempX:" + tempX);
            if (Mathf.Abs(tempX) > LIMITE_HALFWIDTH) {
                //hit the left or right wall
                //Debug.LogError("hit the left or right wall");
                tempX = tempX > 0 ? LIMITE_HALFWIDTH : -LIMITE_HALFWIDTH;
                this.MoveK = -this.MoveK;
            }
            float tempY = (tempX - selfPos.x) * this.MoveK + selfPos.y;
            if (Mathf.Abs(tempY) > LIMITE_HEIGHT) {
                //hit the bottom or up wall
                //Debug.LogError("hit the bottom or up wall");
                tempY = tempY < 0 ? -LIMITE_HEIGHT : LIMITE_HEIGHT;
                this.MoveK = -this.MoveK;
                this.IsUp = tempY < 0;
            }
            //Debug.LogError("tempY:" + tempY);
            this.SphereSelf.transform.position = new Vector3(tempX, tempY, 0);
            if (IsCollison()) {
                this.MoveStart = false;               
                CheckCollision();
                CreateCollosionSphere(this.Index_Drop_X, this.Index_Drop_Y);
                this.SphereSelf.gameObject.SetActive(false);
                InitSelfSphere();
            }
        }
    }

    //是否有碰撞
    private bool IsCollison() {
        for (int i = 0; i < this.SphereList.Count; i++) {
            Vector3 targetPos = this.SphereList[i].transform.position;
            float distance = Vector2.Distance(targetPos, this.SphereSelf.transform.position);
            if (distance <= this.SphereDia) {
                return true;
            }
        }
        return false;
    }

    private List<Sphere> GetCollisionList() {
        List<Sphere> result = new List<Sphere>();
        for (int i = 0; i < this.SphereList.Count; i++) {
            Vector3 targetPos = this.SphereList[i].transform.position;
            float distance = Vector2.Distance(targetPos, this.SphereSelf.transform.position);
            if (distance <= this.SphereDia && !RangeIsFull(this.SphereList[i])) {
                result.Add(this.SphereList[i]);
            }
        }
        return result;
    }

    //球周围是否全部占满
    private bool RangeIsFull(Sphere sphere) {
        List<Coordination> list = GetRangeSphereIndexList(sphere.Index_X, sphere.Index_Y);
        list.Add(new Coordination(sphere.Index_X + 0.5f, sphere.Index_Y + 1));
        for (int i = 0; i < list.Count; i++) {
            Sphere target = QuerySphere(list[i].X, list[i].Y);
            if (target == null) {
                return false;
            }
        }
        return true;
    }

    //周围球的索引号列表
    private List<Coordination> GetRangeSphereIndexList(float centerX, int centerY) {
        List<Coordination> list = new List<Coordination>();
        list.Add(new Coordination(centerX + 1, centerY));
        list.Add(new Coordination(centerX + 0.5f, centerY - 1));
        list.Add(new Coordination(centerX - 0.5f, centerY - 1));
        list.Add(new Coordination(centerX - 1, centerY));
        list.Add(new Coordination(centerX - 0.5f, centerY + 1));
        list.Add(new Coordination(centerX + 0.5f, centerY + 1));
        return list;
    }

    //检测碰撞
    private void CheckCollision() {
        List<Sphere> collisionList = GetCollisionList();
        collisionList.Sort(delegate(Sphere sphere1, Sphere sphere2) {
            float distance1 = Vector2.Distance(doudou.Instance.SphereSelf.transform.position, sphere1.transform.position);
            float distance2 = Vector2.Distance(doudou.Instance.SphereSelf.transform.position, sphere2.transform.position);
            return distance1.CompareTo(distance2);
        }); //按距离排序
        Debug.LogError("碰撞球的个数：" + collisionList.Count);
        if (collisionList.Count == 1) {
            //=================================================== 碰撞到1个球 ===============================================================

        } else if (collisionList.Count == 2) {
            //=================================================== 碰撞到2个球 ===============================================================
            List<Coordination> rangeIndexList1 = GetRangeSphereIndexList(collisionList[0].Index_X, collisionList[0].Index_Y);
            List<Coordination> rangeIndexList2 = GetRangeSphereIndexList(collisionList[1].Index_X, collisionList[1].Index_Y);
            List<Coordination> sameIndexList = new List<Coordination>();
            for (int i = 0; i < rangeIndexList1.Count; i++) {
                for (int j = 0; j < rangeIndexList2.Count; j++) {
                    if (rangeIndexList2[j].X == rangeIndexList1[i].X && rangeIndexList2[j].Y == rangeIndexList1[i].Y) {
                        sameIndexList.Add(new Coordination(rangeIndexList1[i].X, rangeIndexList1[i].Y));
                    }
                }
            }
            Debug.LogError("两个球共同区域个数：" + sameIndexList.Count);
            if (sameIndexList.Count == 1) {
                //两个球不连接，且相隔一个球的位置
                if (QuerySphere(sameIndexList[0].X, sameIndexList[0].Y) == null) {
                    this.Index_Drop_X = sameIndexList[0].X;
                    this.Index_Drop_Y = sameIndexList[0].Y;
                    return;
                }
            } else if (sameIndexList.Count == 2) {
                //两个球相邻
                Sphere sphere1 = QuerySphere(sameIndexList[0].X, sameIndexList[0].Y);
                Sphere sphere2 = QuerySphere(sameIndexList[1].X, sameIndexList[1].Y);
                if (sphere1 == null && sphere2 != null) {
                    this.Index_Drop_X = sameIndexList[0].X;
                    this.Index_Drop_Y = sameIndexList[0].Y;
                    return;
                } else if (sphere2 == null && sphere1 != null) {
                    this.Index_Drop_X = sameIndexList[1].X;
                    this.Index_Drop_Y = sameIndexList[1].Y;
                    return;
                } else if (sphere2 == null && sphere1 == null) {

                    return;
                }
            }
        } else if (collisionList.Count == 3) {
            //=================================================== 碰撞到3个球 ===============================================================
            List<Coordination> rangeIndexList1 = GetRangeSphereIndexList(collisionList[0].Index_X, collisionList[0].Index_Y);
            List<Coordination> rangeIndexList2 = GetRangeSphereIndexList(collisionList[1].Index_X, collisionList[1].Index_Y);
            List<Coordination> rangeIndexList3 = GetRangeSphereIndexList(collisionList[2].Index_X, collisionList[2].Index_Y);
            List<Coordination> tempSameIndexList = new List<Coordination>();
            for (int i = 0; i < rangeIndexList1.Count; i++) {
                for (int j = 0; j < rangeIndexList2.Count; j++) {
                    if (rangeIndexList2[j].X == rangeIndexList1[i].X && rangeIndexList2[j].Y == rangeIndexList1[i].Y) {
                        tempSameIndexList.Add(new Coordination(rangeIndexList1[i].X, rangeIndexList1[i].Y));
                    }
                }
            }
            List<Coordination> sameIndexList = new List<Coordination>();
            for (int i = 0; i < tempSameIndexList.Count; i++) {
                for (int j = 0; j < rangeIndexList3.Count; j++) {
                    if (rangeIndexList3[j].X == tempSameIndexList[i].X && rangeIndexList3[j].Y == tempSameIndexList[i].Y) {
                        sameIndexList.Add(new Coordination(tempSameIndexList[i].X, tempSameIndexList[i].Y));
                    }
                }
            }
            Debug.LogError("三个球共同区域个数：" + sameIndexList.Count);
            if (sameIndexList.Count == 1) {
                Sphere sphere1 = QuerySphere(sameIndexList[0].X, sameIndexList[0].Y);
                Sphere sphere2 = QuerySphere(sameIndexList[1].X, sameIndexList[1].Y);
                if (sphere1 == null && sphere2 != null) {
                    this.Index_Drop_X = sameIndexList[0].X;
                    this.Index_Drop_Y = sameIndexList[0].Y;
                    return;
                } else if (sphere2 == null && sphere1 != null) {
                    this.Index_Drop_X = sameIndexList[1].X;
                    this.Index_Drop_Y = sameIndexList[1].Y;
                    return;
                } else if (sphere2 == null && sphere1 == null) {

                    return;
                }
            }
        }
        bool isSuccess = false;
        for (int i = 0; i < collisionList.Count; i++) {
            Debug.LogError("i:" + i + "   index:" + collisionList[i].Index_X + "  " + collisionList[i].Index_Y);
            if (CalDropPosSuccess(collisionList[i])) {
                isSuccess = true;
                break;
            }
        }
        if (!isSuccess) {
            //根据Y轴来判断落点
            collisionList.Sort(delegate(Sphere sphere1, Sphere sphere2) {
                float y1 = sphere1.transform.position.y;
                float y2 = sphere2.transform.position.y;
                if (this.IsUp) {
                    return y1.CompareTo(y2);
                } else {
                    return y2.CompareTo(y1);
                }
            });
        }
    }

    private Coordination GetDropPodCoordination(Sphere beHitSphere) {
        float x0 = this.SphereSelf.transform.position.x;
        float y0 = this.SphereSelf.transform.position.y;
        float x1 = beHitSphere.transform.position.x;
        float y1 = beHitSphere.transform.position.y;
        float a = this.MoveK * this.MoveK + 1;
        float b = 2 * (y0 * MoveK - MoveK * MoveK * x0 - y1 * MoveK - x1);
        float c = (y0 - y1 - this.MoveK * x0) * (y0 - y1 - this.MoveK * x0) + x1 * x1 - this.SphereDia * this.SphereDia;
        float x_1 = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / 2 / a;
        float y_1 = y0 - this.MoveK * (x0 - x_1);
        Debug.LogError("x_1:" + x_1);
        float x_2 = (-b - Mathf.Sqrt(b * b - 4 * a * c)) / 2 / a;
        Debug.LogError("x_2:" + x_2);
        float result_x = 0;
        if (this.MoveK > 0) {
            result_x = this.IsUp ? Mathf.Min(x_1, x_2) : Mathf.Max(x_1, x_2);
        } else {
            result_x = this.IsUp ? Mathf.Max(x_1, x_2) : Mathf.Min(x_1, x_2);
        }
        float result_y = y0 - this.MoveK * (x0 - result_x);
        Debug.LogError("result_x:" + result_x + "   result_y:" + result_y);
        int pos_Y = 0;
        for (int i = 0; i < 100; i++) {
            float tempY = i * SphereDia * Mathf.Sqrt(3f) / 2f;
            float tempY_next = (i + 1) * SphereDia * Mathf.Sqrt(3f) / 2f;
            if (result_y > 0) {
                if (result_y >= tempY && result_y <= tempY_next) {
                    pos_Y = i;
                    break;
                }
            } else if (result_y < 0) {
                if (result_y <= -tempY && result_y >= -tempY_next) {
                    pos_Y = -i;
                    break;
                }
            } else {
                break;
            }
        }
        Debug.LogError("pos_Y:" + pos_Y);
        float pos_X = 0;
        if (pos_Y % 2 == 0) {
            //奇数行
            for (int i = 0; i < 100; i++) {
                float tempX = i * SphereDia;
                float tempX_next = (i + 1) * SphereDia;
                if (result_x > 0) {
                    if (result_x >= tempX && result_x <= tempX_next) {
                        pos_X = i;
                        break;
                    }
                } else if (result_x < 0) {
                    if (result_x <= -tempX && result_x >= -tempX_next) {
                        pos_X = -i;
                        break;
                    }
                } else {
                    break;
                }
            }
        } else {
            //偶数行
            for (float i = 0.5f; i < 100; i++) {
                float tempX = i * SphereDia;
                float tempX_next = (i + 1) * SphereDia;
                if (result_x > 0) {
                    if (result_x >= tempX && result_x <= tempX_next) {
                        pos_X = i;
                        break;
                    }
                } else if (result_x < 0) {
                    if (result_x <= -tempX && result_x >= -tempX_next) {
                        pos_X = -i;
                        break;
                    }
                } else {
                    break;
                }
            }            
        }
        Debug.LogError("pos_X:" + pos_X);
        return new Coordination(pos_X, pos_Y);
    }

    //计算落点坐标

    private bool CalDropPosSuccess(Sphere beHitSphere) {
        //Debug.LogError("===========>>>hit ball:" + index_x_behit + "   " + index_y_behit);  
        Coordination dropPos = GetDropPodCoordination(beHitSphere);
        if (QuerySphere(dropPos.X, dropPos.Y) != null) {
            Debug.LogError("位置:" + dropPos.X + "   " + dropPos.Y + "已被占据，错误!!!!!!!");
            return false;
        } else {
            Debug.LogError("落点是：" + dropPos.X + "   " + dropPos.Y);
            this.Index_Drop_X = dropPos.X;
            this.Index_Drop_Y = dropPos.Y;
            return true;
        }
    }


}

public class Coordination {
    public float X;
    public int Y;

    public Coordination(float x, int y) {
        X = x;
        Y = y;
    }
}
