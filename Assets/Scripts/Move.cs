using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    private Rigidbody targetObjectRigid;

    Vector3 moveDirection;

    /*defineData*/
    private float gravityAcc = 5f;

    /*move&rotate*/
    private float moveSpeed = 3f;
    private float rotateSpeed = Pi / 3f;
    public float currentSpeed = 0f;
    private float framesBeforeStop = 30f;
    private float slowDownAcc;

    /*jump*/
    Vector3 jumpDirect;
    private float jumpSpeed = 10f;
    private float currentJumpSpeed = 0f;
    private float jumpSlowDownAcc = 0.1f;

    /*dash*/
    private float dashSpeed = 50f;
    private float currentDashSpeed = 0f;
    private float dashSlowDownAcc = 1f;
    public float maxDashForce = 40f;
    public float minDashForce = 10f;
    public float currentDashForce = 0f;
    private float chargeSpeed = 1f;
    bool charging = false;
    bool grow = true;

    /*math*/
    private float sqrt3;
    const float Pi = 3.1415926f;

    // Use this for initialization
    void Start() {
        targetObjectRigid = GetComponent<Rigidbody>();
        slowDownAcc = moveSpeed / framesBeforeStop;

        sqrt3 = Mathf.Sqrt(3f);
    }

    // Update is called once per frame
    void Update() {
        moveDirection = targetObjectRigid.transform.forward;
        InputandMove();
    }

    void DoRotate(bool Right) {
        if (Right) {
            targetObjectRigid.transform.Rotate(0, rotateSpeed, 0, Space.Self);
        }
        else {
            targetObjectRigid.transform.Rotate(0, -rotateSpeed, 0, Space.Self);
        }
    }

    void DoMove() {
        if (currentSpeed > 0)
            currentSpeed *= (1 - slowDownAcc);
        if (currentSpeed < slowDownAcc) {
            currentSpeed = 0;
            return;
        }

        targetObjectRigid.transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }

    void CalJumpDirect() {
        if (currentSpeed != 0)
            jumpDirect = new Vector3(moveDirection.x, 1, moveDirection.z);
        else
            jumpDirect = new Vector3(0, 1, 0);
    }

    void DoJump() {
        if (currentJumpSpeed > 0)
            currentJumpSpeed -= jumpSlowDownAcc;
        if (currentJumpSpeed <= 0) { 
            currentJumpSpeed = 0;
            return;
        }

        targetObjectRigid.transform.position += jumpDirect * currentJumpSpeed * Time.deltaTime;
    }

    void chargeDashForce(bool grow) {
        if (grow)
            currentDashForce += chargeSpeed;
        else
            currentDashForce -= chargeSpeed;
    }

    void calDashSpeed() {
        currentDashSpeed = dashSpeed * (currentDashForce / maxDashForce);
    }

    void calDashForce() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !charging && currentDashSpeed == 0) {
            charging = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && charging) {
            calDashSpeed();
            currentDashForce = 0;
            charging = false;
        }
        if (charging) {
            chargeDashForce(grow);
            if (currentDashForce >= maxDashForce) {
                grow = false;
            }
            if (currentDashForce <= minDashForce) {
                grow = true;
            }
        }
    }

    void DoDash() {
        if (currentDashSpeed > 0)
            currentDashSpeed -= dashSlowDownAcc;
        if (currentDashSpeed <= 0) {
            currentDashSpeed = 0;
            return;
        }

        targetObjectRigid.transform.position += moveDirection * currentDashSpeed * Time.deltaTime;
    }

    void MoveByGravity() {
        Vector3 ToGround = new Vector3(0f, -1f, 0f);
        targetObjectRigid.transform.position += ToGround * gravityAcc * Time.deltaTime;
    }

    void InputandMove() {
        if (currentDashSpeed == 0 && currentJumpSpeed == 0){
            if (Input.GetKey(KeyCode.A))
            {
                DoRotate(false);
            }
            if (Input.GetKey(KeyCode.D))
            {
                DoRotate(true);
            }
            if (Input.GetKey(KeyCode.W))
            {
                currentSpeed = moveSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)&&currentJumpSpeed==0) {
            CalJumpDirect();
            currentJumpSpeed = jumpSpeed;
        }
        if (currentDashSpeed == 0) {
            calDashForce();
        }
        DoMove();
        DoDash();
        DoJump();
        MoveByGravity();
    }
}
