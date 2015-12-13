using UnityEngine;
using System.Collections;

public class SatelliteRelay : MonoBehaviour
{
    public AngleThing Source;
    public AngleThing Target;

    private AngleThing _angleThing;
    private Vector3 _targetForward;
    private bool _targeting;
    private float _untargetTimer;

    void Start()
    {
        _angleThing = GetComponent<AngleThing>();
        _untargetTimer = 0.0f;
    }

    void Update()
    {
        if (_untargetTimer <= 0.0f)
            _targeting = false;
        else
            _untargetTimer -= Time.deltaTime;

        _angleThing.Angle += Time.deltaTime * 5.0f * Mathf.Deg2Rad;
        transform.position = _angleThing.Direction * 6.0f;

        var target = _targeting ? _targetForward : -transform.position;

        var angle = Mathf.DeltaAngle(Mathf.Atan2(transform.forward.y, transform.forward.x) * Mathf.Rad2Deg,
                       Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg);

        var rotated = Quaternion.AngleAxis(Mathf.Sign(angle) * 45.0f * Time.deltaTime, Vector3.forward) * transform.forward;

        if (Mathf.Abs(angle) <= 45.0f * Time.deltaTime)
            transform.forward = target;
        else
            transform.forward = rotated;
    }

    public void RotateToTarget(Vector3 direction)
    {
        var mirror = transform.FindChild("Mirror");
        var mirrorToTarget = (Target.transform.FindChild("SignalPos").position - mirror.transform.position).normalized;
        var mirrorToReflection = (-direction).normalized;

        _targetForward = (mirrorToTarget + mirrorToReflection).normalized;

        _targeting = true;
        _untargetTimer = 5.0f;
    }
}