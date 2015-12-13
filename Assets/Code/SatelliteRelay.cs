using UnityEngine;
using System.Collections;

public class SatelliteRelay : MonoBehaviour
{
    private float _angle = 0.0f;

    public AngleThing Source;
    public AngleThing Target;

    private AngleThing _angleThing;

    void Start()
    {
        _angleThing = GetComponent<AngleThing>();
    }
    
    void Update()
    {
        if (Source == null || Target == null)
            return;

        _angleThing.Angle += Time.deltaTime * 5.0f * Mathf.Deg2Rad;
        _angleThing.Direction = new Vector3(Mathf.Cos(_angleThing.Angle), Mathf.Sin(_angleThing.Angle), 0.0f);
        transform.position = _angleThing.Direction * 6.0f;
        //transform.position = Vector3.Normalize((Target.Direction + Source.Direction)) * 6.0f;
        //var dir = ((Source.transform.position - transform.position) + (Target.transform.position - transform.position)) * 0.5f;

        //transform.forward = dir;
        // transform.forward = -transform.position;
    }
}