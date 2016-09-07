using UnityEngine;
using System.Collections;

public class ActorManager : MonoBehaviour {

    private Animator ActorAnimator_;

    void Awake() {
        this.ActorAnimator_ = this.GetComponent<Animator>();
        Invoke("MoveForward", 2f);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //向前移动
    public void MoveForward() {
        this.ActorAnimator_.SetBool("Run", true);
    }




}
