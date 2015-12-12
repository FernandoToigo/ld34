﻿using UnityEngine;
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
        set { _signals = value; }
    }

    private float _newSignalTimer;

    void Start()
    {
        Signals = new List<Signal>();
    }

    void Update()
    {
        _newSignalTimer -= Time.deltaTime;
        if (_newSignalTimer <= 0.0f)
        {
            _newSignalTimer += 10.0f;

            var signal = CreateRandomSignal();
            Signals.Add(signal);
        }
    }

    private Signal CreateRandomSignal()
    {
        var angleSource = Random.Range(0.0f, Mathf.PI * 2.0f);
        var offset = Random.Range(0.0f, Mathf.PI * 0.25f);

        var signal = new Signal
        {
            AngleSource = angleSource,
            AngleTarget = angleSource + (-Mathf.PI * 0.125f + offset),
            TotalData = Signal.MAX_DATA,
            Color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f))
    };

        var sourcePos = new Vector3(Mathf.Cos(signal.AngleSource), Mathf.Sin(signal.AngleSource), 0.0f) * 4.0f;
        var targetPos = new Vector3(Mathf.Cos(signal.AngleTarget), Mathf.Sin(signal.AngleTarget), 0.0f) * 4.0f;

        var sourcePrefab = GameObject.Instantiate(SignalSourcePrefab);
        sourcePrefab.GetComponent<SignalSource>().Signal = signal;
        sourcePrefab.GetComponent<MeshRenderer>().material.color = signal.Color;
        sourcePrefab.transform.position = sourcePos;
        sourcePrefab.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, signal.AngleSource * Mathf.Rad2Deg);
        signal.SourceGameObject = sourcePrefab;

        var targetPrefab = GameObject.Instantiate(SignalTargetPrefab);
        targetPrefab.GetComponent<MeshRenderer>().material.color = signal.Color;
        targetPrefab.transform.position = targetPos;
        targetPrefab.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, signal.AngleTarget * Mathf.Rad2Deg);
        signal.TargetGameObject = targetPrefab;

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