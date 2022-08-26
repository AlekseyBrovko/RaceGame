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
    public enum Side
    {
        Left,
        Right
    }

    public enum DriveType
    {
        RearWheelDrive,
        FrontWheelDrive,
        AllWheelDrive
    }

    [Serializable]
    public struct WheelTest
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
        public Side side;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
    }

    public static CarController Instance;
    public DriveType driveType;

    public float motorPower = 600f;
    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensivity = 1.0f;
    public float maxSteerAngle = 30.0f;
    public float radiusOfReturn = 6f;
    private float indexedRadiusOfReturn;
    public float maxSpeed = 30f;
    private bool isMaxSpeed;
    private float steeringLimiter;

    public Vector3 _centerOfMass;

    public List<WheelTest> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRB;
    private CarLights carLights;
    public float carSpeed;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        carRB = GetComponent<Rigidbody>();
        carRB.centerOfMass = _centerOfMass;
        carLights = GetComponent<CarLights>();
    }

    private void Update()
    {
        GetInputs();
        AnimationWheels();
        WheelEffects();

        SpeedLimiter();

        carSpeed = carRB.velocity.magnitude;
    }

    private void FixedUpdate()
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
        if (!isMaxSpeed)
        {
            if (driveType == DriveType.AllWheelDrive)
            {
                foreach (var wheel in wheels)
                {
                    //wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime * (wheel.invertSteer ? -1 : 1);

                    wheel.wheelCollider.motorTorque = (moveInput * motorPower * maxAcceleration) / 4 * Time.fixedDeltaTime;
                }
            }

            if (driveType == DriveType.FrontWheelDrive)
            {
                foreach (var wheel in wheels)
                {
                    if (wheel.axel == Axel.Front)
                    {
                        wheel.wheelCollider.motorTorque = (moveInput * motorPower * maxAcceleration) / 2 * Time.fixedDeltaTime;
                    }
                }
            }

            if (driveType == DriveType.RearWheelDrive)
            {
                foreach (var wheel in wheels)
                {
                    if (wheel.axel == Axel.Rear)
                    {
                        wheel.wheelCollider.motorTorque = (moveInput * motorPower * maxAcceleration) / 2 * Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }

    public void Steer()
    {
        SteeringLimiterValue();
        foreach (var wheel in wheels)
        {

            if (wheel.axel == Axel.Front)
            {
                if (steerInput > 0)
                {
                    if (wheel.side == Side.Left)
                        wheel.wheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (indexedRadiusOfReturn + (1.5f / 2))) * steerInput;
                    if (wheel.side == Side.Right)
                        wheel.wheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (indexedRadiusOfReturn - (1.5f / 2))) * steerInput;
                }
                if (steerInput < 0)
                {
                    if (wheel.side == Side.Left)
                        wheel.wheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (indexedRadiusOfReturn - (1.5f / 2))) * steerInput;
                    if (wheel.side == Side.Right)
                        wheel.wheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (indexedRadiusOfReturn + (1.5f / 2))) * steerInput;
                }
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

            carLights.isBackLightOn = true;
            carLights.OperateBackLights();
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
            carLights.isBackLightOn = false;
            carLights.OperateBackLights();
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
                wheel.smokeParticle.Emit(1);
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    public void SpeedLimiter()
    {
        if (carRB.velocity.magnitude >= maxSpeed)
        {
            isMaxSpeed = true;
        }
        else
        {
            isMaxSpeed = false;
        }
    }

    public void SteeringLimiterValue()
    {
        if (carSpeed <= 5f)
        {
            indexedRadiusOfReturn = radiusOfReturn;
        }
        if (carSpeed >5f)
        {
            indexedRadiusOfReturn = Mathf.Lerp(radiusOfReturn/2, radiusOfReturn, (1-(carSpeed/maxSpeed)));
        }
    }
}
