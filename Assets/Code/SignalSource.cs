using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SignalSource : MonoBehaviour
{
    public LayerMask WorldMask;
    public LayerMask SignalTargetMask;
    public LayerMask MirrorMask;
    public LayerMask SatelliteRelayMask;
    public GameObject ReflectionPrefab;

    private GameObject _satellite;
    private GameObject _mainMirror;
    private GameObject _signalPos;
    private AppearFromGround _appearingFromGround;
    private List<GameObject> _reflections;
    private bool _transfering;

    private Signal _signal;
    public Signal Signal
    {
        get { return _signal; }
        set { _signal = value; }
    }

    void Start()
    {
        _satellite = GameObject.Find("Satellite");
        _mainMirror = _satellite.transform.FindChild("Mirror").gameObject;
        _signalPos = transform.FindChild("SignalPos").gameObject;
        _appearingFromGround = GetComponent<AppearFromGround>();
        _reflections = new List<GameObject>();
    }

    void Update()
    {
        foreach (var reflection in _reflections)
            GameObject.Destroy(reflection);
        _reflections.Clear();

        if (_appearingFromGround.Appearing)
            return;

        _transfering = false;

        CreateReflections(_signalPos.transform.position, (_mainMirror.transform.position - _signalPos.transform.position).normalized, WorldMask | MirrorMask | SatelliteRelayMask);

        if (_transfering)
        {
            if (_signal.TotalData == Signal.MAX_DATA)
            {
                var audio = GetComponent<AudioSource>();
                if (!audio.isPlaying)
                    audio.Play();

                DataLog.LogStatic("transmiting data...");
                DataLog.LogStatic("0%...");

                GetComponent<AudioSource>().Play();
            }

            var oldData = (int)((_signal.TotalData / Signal.MAX_DATA) * 100.0f);
            _signal.TotalData -= Time.deltaTime;
            var newData = (int)((_signal.TotalData / Signal.MAX_DATA) * 100.0f);

            if (oldData / 20 != newData / 20)
                DataLog.LogStatic(100 - (newData / 20) * 20 + "%...");
        }
        else
        {
            var audio = GetComponent<AudioSource>();
            if (audio.isPlaying)
            {
                audio.Stop();
                _signal.TotalData = Signal.MAX_DATA;
                DataLog.LogStatic("signal lost", DataLog.FAIL_COLOR);
            }
        }
    }

    private void CreateReflections(Vector3 startPos, Vector3 direction, LayerMask mask)
    {
        var dest = startPos + direction * 50.0f;
        RaycastHit hit;
        if (Physics.Linecast(startPos, dest, out hit, mask))
        {
            AddReflection(startPos, hit.point);

            if (1 << hit.collider.gameObject.layer == MirrorMask)
            {
                var mirror = hit.collider.gameObject;
                var reflectDir = Vector3.Reflect(direction, mirror.transform.forward);

                if (Vector3.Dot(-direction, mirror.transform.forward) > 0.1f)
                {
                    mirror.layer = LayerMask.NameToLayer("Default");
                    CreateReflections(hit.point, reflectDir, WorldMask | SignalTargetMask | MirrorMask | SatelliteRelayMask);
                    mirror.layer = LayerMask.NameToLayer("Mirror");
                }

                var satelliteRelay = mirror.transform.parent.GetComponent<SatelliteRelay>();
                if (satelliteRelay != null)
                    satelliteRelay.RotateToTarget(direction, _signal);
            }
            else if (1 << hit.collider.gameObject.layer == SignalTargetMask && _signal.TargetGameObject == hit.collider.gameObject)
                _transfering = true;
            else if (1 << hit.collider.gameObject.layer == SatelliteRelayMask)
            {
                var satelliteRelay = hit.collider.transform.parent.GetComponent<SatelliteRelay>();
                if (satelliteRelay != null)
                    satelliteRelay.RotateToTarget(direction, _signal);
            }
        }
        else
            AddReflection(startPos, dest);
    }

    private void AddReflection(Vector3 startPos, Vector3 endPos)
    {
        var reflection = GameObject.Instantiate(ReflectionPrefab);
        var lineRenderer = reflection.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);


        var size = (_signal.TotalData / Signal.MAX_DATA) * 0.05f;
        lineRenderer.SetWidth(size, size);
        lineRenderer.material.color = _signal.Color;

        reflection.transform.parent = gameObject.transform;
        _reflections.Add(reflection);
    }
}