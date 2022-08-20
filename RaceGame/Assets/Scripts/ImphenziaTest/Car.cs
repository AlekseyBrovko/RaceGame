using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform centerOfMass;
    public float motorTorque = 100f;
    public float maxSteer = 20f;
    public float Steer { get; set; }
    public float Throttle { get; set; }
    public Wheel[] wheels;
    private Rigidbody _rigidbody;

    private void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        Steer = GameManager.Instance.InputController.SteerInput;
        Throttle = GameManager.Instance.InputController.ThrottleInput;
        
        foreach(var wheel in wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.Torque = Throttle * motorTorque;
        }
    }
}
