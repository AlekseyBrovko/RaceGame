using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;

    public Vector3 offset;
    public Vector3 eulerRotation;
    public float dumper;

    private void Update()
    {
        if (Target == null) return;
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, dumper * Time.deltaTime);
    }
}
