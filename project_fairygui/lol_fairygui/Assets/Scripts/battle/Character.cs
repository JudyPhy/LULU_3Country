using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
        
    protected Animator _ani;
    protected NavMeshAgent _agent;

    void Awake() {
        OnAwake();
    }

    void Start() {
        OnStart();        
    }

    void Update() {
        OnUpdate();

        

        //if (Input.GetMouseButton(0)) {
        //    Vector3 screenPoint = Input.mousePosition;
        //    Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 10000)) {
        //        Vector3 targetpoint = ray.GetPoint(hit.distance);
        //Vector3 targetpoint = new Vector3(200, 10, 300);
        //_agent.SetDestination(targetpoint);
        //_agent.Resume();
        //}
        //}
    }

    void LateUpdate() {
        OnLateUpdate();        
    }

    protected virtual void OnAwake() {
        _ani = this.GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void OnStart() {
        _ani.SetInteger("Status", 0);
    }

    protected virtual void OnUpdate() { 
    
    }

    protected virtual void OnLateUpdate() {

    }

}
