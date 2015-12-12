using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SignalCreator : MonoBehaviour
{
    public GameObject SignalSourcePrefab;
    public GameObject SignalTargetPrefab;

    private List<Signal> _signals;
    private float _newSignalTimer;

    void Start()
    {
        _signals = new List<Signal>();
    }

    void Update()
    {
        _newSignalTimer -= Time.deltaTime;
        if (_newSignalTimer <= 0.0f)
        {
            _newSignalTimer += 5.0f;

            var signal = CreateRandomSignal();
            var sourcePos = new Vector3(Mathf.Cos(signal.AngleSource), Mathf.Sin(signal.AngleSource), 0.0f) * 4.0f;
            var targetPos = new Vector3(Mathf.Cos(signal.AngleTarget), Mathf.Sin(signal.AngleTarget), 0.0f) * 4.0f;
            Debug.DrawLine(new Vector3(), sourcePos, Color.red, 10.0f);
            Debug.DrawLine(new Vector3(), targetPos, Color.green, 10.0f);

            var sourcePrefab = GameObject.Instantiate(SignalSourcePrefab);
            sourcePrefab.transform.position = sourcePos;
            sourcePrefab.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, signal.AngleSource * Mathf.Rad2Deg);

            var targetPrefab = GameObject.Instantiate(SignalTargetPrefab);
            targetPrefab.transform.position = targetPos;
            targetPrefab.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, signal.AngleTarget * Mathf.Rad2Deg);

            _signals.Add(signal);
        }
    }

    private static Signal CreateRandomSignal()
    {
        var angleSource = Random.Range(0.0f, Mathf.PI * 2.0f);
        var offset = Random.Range(0.0f, Mathf.PI * 0.25f);

        var signal = new Signal
        {
            AngleSource = angleSource,
            AngleTarget = angleSource + (-Mathf.PI * 0.125f + offset)
        };

        return signal;
    }
}

public class Signal
{
    private float _angleSource;
    public float AngleSource
    {
        get { return _angleSource; }
        set { _angleSource = value; }
    }

    private float _angleTarget;
    public float AngleTarget
    {
        get { return _angleTarget; }
        set { _angleTarget = value; }
    }
}