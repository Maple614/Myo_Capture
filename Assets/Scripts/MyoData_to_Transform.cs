using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyoData_to_Transform : MonoBehaviour {

    public GameObject myoObject;
    public Vector3 acceleration;
    public Vector3 velocity;

    private Vector3 offsetRotation_ = Vector3.zero;
    private ThalmicMyo myo_;

    private Vector3 before_Accel;

    private float speed = 0.1f;

    private void Awake()
    {
        myo_ = myoObject.GetComponent<ThalmicMyo>();
        before_Accel.x = myo_.accelerometer.x;
        before_Accel.y = myo_.accelerometer.y;
        before_Accel.z = myo_.accelerometer.z;
    }


    // Update is called once per frame
    void FixedUpdate () {
        //姿勢情報は ThalmicMyo がついた GameObjectを参照
        var rotation = myoObject.transform.rotation.eulerAngles;
        rotation = new Vector3(-rotation.x, rotation.y, -rotation.z);

        //位置情報の参照は加速度から計算
        var dir = Vector3.zero;
        dir = myo_.accelerometer * 0.1f + before_Accel * 0.9f;
        Debug.Log("x= " + myo_.accelerometer.x);
        Debug.Log("y= " + myo_.accelerometer.y);
        Debug.Log("z= " + myo_.accelerometer.z);
        Debug.Log("DeltaTime "+Time.deltaTime);
        var diff = dir - before_Accel;

        before_Accel = dir;

        /*
        Debug.Log("x= " + diff.x);
        Debug.Log("y= " + diff.y);
        Debug.Log("z= " + diff.z);
        */
        //正規化
        if (diff.sqrMagnitude > 1)
        {
            diff.Normalize();
        }
        
        //現在の位置を基準の角度に変更する
        if (Input.GetKeyDown(KeyCode.R))
        {
            offsetRotation_ = rotation;
        }

        //回転
        transform.rotation = Quaternion.Euler(rotation - offsetRotation_);

        //移動
        diff *= Time.fixedDeltaTime;
        transform.Translate(diff*speed);
	}
}
