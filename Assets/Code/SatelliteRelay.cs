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
        //_angle += 2.5f * Mathf.Deg2Rad * Time.deltaTime;

        if (Source == null || Target == null)
            return;
        Debug.Log("source:" + Source.Angle);
        var angle = Source.Angle + (Target.Angle - Source.Angle) * 0.5f;
        var x = Mathf.Cos(angle) * 8.0f;
        var y = Mathf.Sin(angle) * 8.0f;

        transform.position = new Vector3(x, y, 0.0f);

        var dir = ((Source.transform.position - transform.position) + (Target.transform.position - transform.position)) * 0.5f;

        transform.forward = dir;
        //transform.forward = -transform.position
    }
}