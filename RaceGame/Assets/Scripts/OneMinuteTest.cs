using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneMinuteTest : MonoBehaviour
{
    private Vector3 MoveForce;
    public float MoveSpeed = 50f;
    public float Drag = 0.98f;
    public float MaxSpeed = 15f;
    public float SteerAngle = 20f;
    public float Traction = 1f;

    private float vertInput;
    private float horInput;
    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //moving
        MoveForce += transform.forward * MoveSpeed * vertInput * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;

        //sterring
        transform.Rotate(Vector3.up * horInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);

        //drag and max speed limit
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        //traction
        Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }

    private void PlayerInput()
    {
        vertInput = Input.GetAxis("Vertical");
        horInput = Input.GetAxis("Horizontal");
    }
}
