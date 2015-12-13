using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SignalCreator : MonoBehaviour
{
    public GameObject SignalSourcePrefab;
    public GameObject SignalTargetPrefab;

    private static List<Signal> _signals;

    private float _newSignalTimer;

    private int _signalsRemovedCount = 0;
    private int _maxConcurrentSignals = 1;
    private float _maxAngleOffset = 45.0f * Mathf.Deg2Rad;
    private float _minAngleOffset = 10.0f * Mathf.Deg2Rad;

    void Start()
    {
        _signals = new List<Signal>();
    }

    void Update()
    {
        _newSignalTimer -= Time.deltaTime;

        if (_signals.Count < _maxConcurrentSignals)
        {
            if (_newSignalTimer <= 0.0f)
            {
                _newSignalTimer += 10.0f;

                var signal = CreateRandomSignal();
                _signals.Add(signal);
            }
        }

        var toRemove = new List<Signal>();
        foreach (var signal in _signals)
        {
            if (signal.TotalData <= 0.0f)
            {
                toRemove.Add(signal);
                GameObject.Destroy(signal.SourceGameObject);
                GameObject.Destroy(signal.TargetGameObject);
                _signalsRemovedCount++;

                if (_signalsRemovedCount == 1)
                {
                    _maxAngleOffset = 180.0f * Mathf.Deg2Rad;
                    _minAngleOffset = 90.0f * Mathf.Deg2Rad;
                }
                else if (_signalsRemovedCount == 3)
                {
                    //_maxConcurrentSignals = 2;
                    _maxAngleOffset = 90.0f * Mathf.Deg2Rad;
                    _minAngleOffset = 45.0f * Mathf.Deg2Rad;
                }

                Debug.Log(_signalsRemovedCount);
            }
        }

        foreach (var signal in toRemove)
            _signals.Remove(signal);
    }

    private Signal CreateRandomSignal()
    {
        var angleSource = Random.Range(0.0f, Mathf.PI * 2.0f);
        var offset = Random.Range(_minAngleOffset, _maxAngleOffset);
        offset *= Mathf.Sign(Random.Range(-1.0f, 1.0f));
        //Debug.Log("Offset " + offset * Mathf.Rad2Deg);

        var signal = new Signal
        {
            AngleSource = angleSource,
            AngleTarget = angleSource + offset,
            TotalData = Signal.MAX_DATA,
            Color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f))
        };

        var sourcePos = new Vector3(Mathf.Cos(signal.AngleSource), Mathf.Sin(signal.AngleSource), 0.0f) * 3.8f;
        var targetPos = new Vector3(Mathf.Cos(signal.AngleTarget), Mathf.Sin(signal.AngleTarget), 0.0f) * 3.8f;

        var sourcePrefab = GameObject.Instantiate(SignalSourcePrefab);
        sourcePrefab.GetComponent<SignalSource>().Signal = signal;
        sourcePrefab.transform.FindChild("sender").transform.FindChild("default").GetComponent<MeshRenderer>().material.color = signal.Color;
        sourcePrefab.transform.position = sourcePos;
        sourcePrefab.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, signal.AngleSource * Mathf.Rad2Deg);
        signal.SourceGameObject = sourcePrefab;

        var targetPrefab = GameObject.Instantiate(SignalTargetPrefab);
        targetPrefab.GetComponent<AngleThing>().Angle = signal.AngleTarget;
        targetPrefab.transform.FindChild("receiver").transform.FindChild("default").GetComponent<MeshRenderer>().material.color = signal.Color;
        targetPrefab.transform.position = targetPos;
        targetPrefab.transform.localRotation =
            Quaternion.Euler(0.0f, 0.0f, signal.AngleTarget * Mathf.Rad2Deg) *
            Quaternion.Euler(45.0f, 0.0f, 0.0f);
        signal.TargetGameObject = targetPrefab;

        if (_signalsRemovedCount >= 0)
        {
            var firstRelay = GameObject.Find("FirstSatelliteRelay").GetComponent<SatelliteRelay>();
            firstRelay.Target = signal.TargetGameObject.GetComponent<AngleThing>();
        }

        return signal;
    }
}

public class Signal
{
    public const float MAX_DATA = 3.0f;

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

    private GameObject _sourceGameObject;
    public GameObject SourceGameObject
    {
        get { return _sourceGameObject; }
        set { _sourceGameObject = value; }
    }

    private GameObject _targetGameObject;
    public GameObject TargetGameObject
    {
        get { return _targetGameObject; }
        set { _targetGameObject = value; }
    }

    private float _totalData;
    public float TotalData
    {
        get { return _totalData; }
        set { _totalData = value; }
    }

    private Color color;
    public Color Color
    {
        get { return color; }
        set { color = value; }
    }
}