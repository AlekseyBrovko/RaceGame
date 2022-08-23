using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    [SerializeField] private Text LeftFrontRPMText;
    [SerializeField] private Text RightFrontRPMText;
    [SerializeField] private Text LeftBackRPMText;
    [SerializeField] private Text RightBackRPMText;

    [SerializeField] private Text CarSpeed;

    private CarController carController;

    private void Start()
    {
        carController = CarController.Instance;
    }

    private void Update()
    {
        GetStatsValues();
    }

    public void GetStatsValues()
    {
        LeftFrontRPMText.text = "LeftFrontRPM  " + ((float)System.Math.Round(carController.wheels[0].wheelCollider.rpm)).ToString();
        RightFrontRPMText.text = "RightFrontRPM  " + ((float)System.Math.Round(carController.wheels[1].wheelCollider.rpm)).ToString();
        LeftBackRPMText.text = "LeftBackRPM  " + ((float)System.Math.Round(carController.wheels[2].wheelCollider.rpm)).ToString();
        RightBackRPMText.text ="RightBackRPM  " + ((float)System.Math.Round(carController.wheels[3].wheelCollider.rpm)).ToString();
        CarSpeed.text = "CarSpeed  " + carController.carSpeed.ToString();
    }
}
