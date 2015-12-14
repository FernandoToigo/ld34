using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SignalCreator : MonoBehaviour
{
    private const float TIME_BETWEEN_SIGNALS = 20.0f;

    public GameObject SignalSourcePrefab;
    public GameObject SignalTargetPrefab;
    public AudioClip CallingClip;
    public AudioClip EndClip;

    private static List<Signal> _signals;
    public static List<Signal> Signals
    {
        get { return _signals; }
    }

    private static bool _createSignals = false;
    public static bool CreateSignals
    {
        get { return _createSignals; }
        set { _createSignals = value; }
    }

    private float _newSignalTimer;

    private int _signalsRemovedCount = 0;
    private static List<Signal> _toRemove = new List<Signal>();

    private bool _gameEnded;
    private float _gameEndedTimer;

    void Start()
    {
        _signals = new List<Signal>();
        _toRemove = new List<Signal>();
        _gameEndedTimer = 10.0f;
    }

    void Update()
    {
        if (_gameEnded)
        {
            if (_gameEndedTimer > 8.0f && _gameEndedTimer - Time.deltaTime <= 8.0f)
                DataLog.LogStatic("too many time outs", DataLog.SUCCESS_COLOR);

            if (_gameEndedTimer > 5.0f && _gameEndedTimer - Time.deltaTime <= 5.0f)
                DataLog.LogStatic("'congratulations!'", DataLog.SUCCESS_COLOR);

            _gameEndedTimer -= Time.deltaTime;

            if (_gameEndedTimer <= 0.0f)
                Application.LoadLevel("Game");

            return;
        }

        if (_createSignals && _signals.Count < LevelControl.CurrentLevel.MaxConcurrentSignals)
        {
            _newSignalTimer -= Time.deltaTime;

            if (_newSignalTimer <= 0.0f)
            {
                _newSignalTimer += TIME_BETWEEN_SIGNALS;

                var signal = CreateRandomSignal();
                _signals.Add(signal);
            }
        }

        foreach (var signal in _signals)
        {
            signal.LifeTime -= Time.deltaTime;

            var remove = false;
            if (signal.TotalData <= 0.0f)
            {
                var audioSource = GetComponent<AudioSource>();
                audioSource.volume = 0.15f;
                audioSource.clip = EndClip;
                audioSource.Play();
                DataLog.LogStatic("transmission complete: " + signal.DataSize.ToString(".0") + " gigabytes transmitted", DataLog.SUCCESS_COLOR);
                LevelControl.DataTransmitted += signal.DataSize;
                remove = true;
            }

            if (signal.LifeTime <= 0.0f)
            {
                LevelControl.Lifes--;
                DataLog.LogStatic("transmission timed out", DataLog.FAIL_COLOR);
                remove = true;

                if (LevelControl.Lifes <= 0)
                    _gameEnded = true;
            }

            if (remove)
            {
                _toRemove.Add(signal);
                signal.SourceGameObject.GetComponent<AppearFromGround>().Appear(-signal.SourceGameObject.transform.position.normalized, 1.0f);
                signal.TargetGameObject.GetComponent<AppearFromGround>().Appear(-signal.TargetGameObject.transform.position.normalized, 1.0f);

                if (_signals.Count == 1)
                    _newSignalTimer = 3.0f;
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
            }
        }
    }

    private Signal CreateRandomSignal()
    {
        var angleSource = Random.Range(0.0f, Mathf.PI * 2.0f);
        var offset = Random.Range(LevelControl.CurrentLevel.MinAngleDistance, LevelControl.CurrentLevel.MaxAngleDistance);
        offset *= Mathf.Sign(Random.Range(-1.0f, 1.0f));

        var signal = new Signal
        {
            AngleSource = angleSource,
            AngleTarget = angleSource + offset,
            TotalData = Signal.MAX_DATA,
            Color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)),
            LifeTime = Mathf.Max(60.0f, Mathf.Abs(offset * Mathf.Rad2Deg)) * LevelControl.CurrentLevel.MaxConcurrentSignals,
            DataSize = Random.Range(6.0f, 10.0f) * LevelControl.CurrentLevel.Number
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
        targetPrefab.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, signal.AngleTarget * Mathf.Rad2Deg);
        //Quaternion.Euler(45.0f, 0.0f, 0.0f);
        targetPrefab.GetComponent<AppearFromGround>().Appear(targetPos.normalized, 1.0f);
        signal.TargetGameObject = targetPrefab;

        var audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
        audioSource.clip = CallingClip;
        audioSource.Play();

        DataLog.LogStatic("new transmission requested");

        return signal;
    }

    public static void CancelAllSignals()
    {
        foreach (var signal in _signals)
        {
            _toRemove.Add(signal);
            signal.SourceGameObject.GetComponent<AppearFromGround>().Appear(-signal.SourceGameObject.transform.position.normalized, 1.0f);
            signal.TargetGameObject.GetComponent<AppearFromGround>().Appear(-signal.TargetGameObject.transform.position.normalized, 1.0f);
        }
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

    private float _lifeTime;
    public float LifeTime
    {
        get { return _lifeTime; }
        set { _lifeTime = value; }
    }

    private float _dataSize;
    public float DataSize
    {
        get { return _dataSize; }
        set { _dataSize = value; }
    }
}