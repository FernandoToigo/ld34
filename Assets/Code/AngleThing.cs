using UnityEngine;
using System.Collections;

public class AngleThing : MonoBehaviour
{
    private Vector3 _direction = Vector3.right;
    public Vector3 Direction
    {
        get { return _direction; }
    }

    private float _angle;
    public float Angle
    {
        get
        {
            return _angle;
        }
        set
        {
            _angle = value;

            if (_angle > 2.0f * Mathf.PI)
                _angle -= 2.0f * Mathf.PI;
            else if (_angle < 0)
                _angle += 2.0f * Mathf.PI;

            _direction = new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0.0f);
        }
    }
}