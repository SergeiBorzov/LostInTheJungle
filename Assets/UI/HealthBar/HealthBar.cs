using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    // Update is called once per frame

    public void SetHealth(float value)
    {
        slider.value = value;
    }

}
