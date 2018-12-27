using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Vector3 offset = new Vector3(0, 5, -4);
    private Transform target;
    private Vector3 pos;

    public float speed = 2;


    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        pos = target.position + offset;
        this.transform.position = Vector3.Lerp(this.transform.position, pos, speed * Time.deltaTime);
        Quaternion angel = Quaternion.LookRotation(target.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angel, speed * Time.deltaTime);
	}
}
