using UnityEngine;
using System.Collections;

public class SatelliteRelay : MonoBehaviour
{
    public AngleThing Source;
    public AngleThing Target;

    private AngleThing _angleThing;
    private float _targetAngle;

    void Start()
    {
        _angleThing = GetComponent<AngleThing>();
    }
    
    void Update()
    {
        if (Source == null || Target == null)
            return;

        _angleThing.Angle += Time.deltaTime * 5.0f * Mathf.Deg2Rad;
        transform.position = _angleThing.Direction * 6.0f;

        var angleStep = Mathf.Min(20.0f * Mathf.Deg2Rad * Time.deltaTime, _targetAngle);
        _targetAngle -= angleStep * Mathf.Sign(_targetAngle);
        transform.rotation *= Quaternion.Euler(0.0f, _targetAngle, 0.0f);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler);
    }

    public void RotateToTarget(Vector3 direction)
    {
        var mirror = transform.FindChild("Mirror");
        var mirrorToTarget = (Target.transform.FindChild("SignalPos").position - mirror.transform.position).normalized;
        var mirrorToReflection = (-direction).normalized;

        //_targetForward = (mirrorToTarget + mirrorToReflection).normalized;
        //transform.forward = (mirrorToTarget + mirrorToReflection).normalized;
    }
}