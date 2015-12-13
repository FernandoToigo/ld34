using UnityEngine;
using System.Collections;

public class AngleThing : MonoBehaviour
{
    private float angle;
    public float Angle
    {
        get
        {
            return angle % Mathf.PI;
        }

        set
        {
            angle = value;

            if (angle > 2.0f * Mathf.PI)
                angle -= 2.0f * Mathf.PI;
        }
    }
}