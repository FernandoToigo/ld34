using UnityEngine;
using System.Collections;

public class AngleThing : MonoBehaviour
{
    private Vector3 direction;
    public Vector3 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    private float angle;
    public float Angle
    {
        get
        {
            return angle;
        }

        set
        {
            angle = value;

            if (angle > 2.0f * Mathf.PI)
                angle -= 2.0f * Mathf.PI;
            else if (angle < 0)
                angle += 2.0f * Mathf.PI;
        }
    }
}