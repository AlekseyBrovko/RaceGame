using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct WheelTest
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
        public GameObject wheelEffectObj;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<WheelTest> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRB;

    private void Start()
    {
        carRB = GetComponent<Rigidbody>();
        carRB.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        GetInputs();
        AnimationWheels();
        WheelEffects();
    }

    private void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    public void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    public void Move()
    {
        foreach (var wheel in wheels)
        {
            //wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime * (wheel.invertSteer ? -1 : 1);
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    public void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    public void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    public void AnimationWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.rotation = rot;
            wheel.wheelModel.transform.position = pos;
        }
    }

    public void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }
}
