using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SignalCreator : MonoBehaviour
{
    public GameObject SignalSourcePrefab;
    public GameObject SignalTargetPrefab;

    private static List<Signal> _signals;
    public static List<Signal> Signals
    {
        get { return _signals; }
    }

    private float _newSignalTimer;

    private int _signalsRemovedCount = 0;
    private int _maxConcurrentSignals = 1;
    private float _maxAngleOffset = 45.0f * Mathf.Deg2Rad;
    private float _minAngleOffset = 10.0f * Mathf.Deg2Rad;
    private static List<Signal> _toRemove = new List<Signal>();

    void Start()
    {
        _signals = new List<Signal>();
        _toRemove = new List<Signal>();
    }

    void Update()
    {
        if (_signals.Count < _maxConcurrentSignals)
        {
            _newSignalTimer -= Time.deltaTime;

            if (_newSignalTimer <= 0.0f)
            {
                _newSignalTimer += 3.0f;

                var signal = CreateRandomSignal();
                _signals.Add(signal);
            }
        }

        foreach (var signal in _signals)
        {
            if (signal.TotalData <= 0.0f)
            {
                _toRemove.Add(signal);
                signal.SourceGameObject.GetComponent<AppearFromGround>().Appear(-signal.SourceGameObject.transform.position.normalized, 1.0f);
                signal.TargetGameObject.GetComponent<AppearFromGround>().Appear(-signal.TargetGameObject.transform.position.normalized, 1.0f);
            }
        }
        
        foreach (var signal in _toRemove.ToList())
        {
            _signals.Remove(signal);

            if (!signal.SourceGameObject.GetComponent<AppearFromGround>().Appearing)
            {
                _toRemove.Remove(signal);
                GameObject.Destroy(signal.SourceGameObject);
                GameObject.Destroy(signal.TargetGameObject);

                _signalsRemovedCount++;

                if (_signalsRemovedCount == 6)
                {
                    _maxAngleOffset = 180.0f * Mathf.Deg2Rad;
                    _minAngleOffset = 90.0f * Mathf.Deg2Rad;
                    _maxConcurrentSignals = 2;
                }
                else if (_signalsRemovedCount == 3)
                {
                    _maxAngleOffset = 90.0f * Mathf.Deg2Rad;
                    _minAngleOffset = 45.0f * Mathf.Deg2Rad;
                }
            }
        }
    }
    
    private Signal CreateRandomSignal()
    {
        var angleSource = Random.Range(0.0f, Mathf.PI * 2.0f);
        var offset = Random.Range(_minAngleOffset, _maxAngleOffset);
        offset *= Mathf.Sign(Random.Range(-1.0f, 1.0f));
        Debug.Log("Offset " + offset * Mathf.Rad2Deg);

        var signal = new Signal
        {
            AngleSource = angleSource,
            AngleTarget = angleSource + offset,
            TotalData = Signal.MAX_DATA,
            Color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f))
        };

        var sourcePos = new Vector3(Mathf.Cos(signal.AngleSource), Mathf.Sin(signal.AngleSource), 0.0f) * 2.8f;
        var targetPos = new Vector3(Mathf.Cos(signal.AngleTarget), Mathf.Sin(signal.AngleTarget), 0.0f) * 2.8f;

        var sourcePrefab = GameObject.Instantiate(SignalSourcePrefab);
        sourcePrefab.GetComponent<SignalSource>().Signal = signal;
        sourcePrefab.transform.FindChild("sender").transform.FindChild("default").GetComponent<MeshRenderer>().material.color = signal.Color;
        sourcePrefab.transform.position = sourcePos;
        sourcePrefab.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, signal.AngleSource * Mathf.Rad2Deg);
        sourcePrefab.GetComponent<AppearFromGround>().Appear(sourcePos.normalized, 1.0f);
        signal.SourceGameObject = sourcePrefab;

        var targetPrefab = GameObject.Instantiate(SignalTargetPrefab);
        targetPrefab.GetComponent<AngleThing>().Angle = signal.AngleTarget;
        targetPrefab.transform.FindChild("receiver").transform.FindChild("default").GetComponent<MeshRenderer>().material.color = signal.Color;
        targetPrefab.transform.position = targetPos;
        targetPrefab.transform.localRotation =
            Quaternion.Euler(0.0f, 0.0f, signal.AngleTarget * Mathf.Rad2Deg) *
            Quaternion.Euler(45.0f, 0.0f, 0.0f);
        targetPrefab.GetComponent<AppearFromGround>().Appear(targetPos.normalized, 1.0f);
        signal.TargetGameObject = targetPrefab;

        GetComponent<AudioSource>().Play();

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