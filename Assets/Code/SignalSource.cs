using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SignalSource : MonoBehaviour
{
    public LayerMask WorldMask;
    public LayerMask SignalTargetMask;
    public LayerMask MirrorMask;
    public GameObject ReflectionPrefab;

    private GameObject _satellite;
    private GameObject _mainMirror;
    private GameObject _signalPos;
    private List<GameObject> _reflections;

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
        _reflections = new List<GameObject>();
    }

    void Update()
    {
        foreach (var reflection in _reflections)
            GameObject.Destroy(reflection);
        _reflections.Clear();

        CreateReflections(_signalPos.transform.position, (_mainMirror.transform.position - _signalPos.transform.position).normalized);
    }

    private void CreateReflections(Vector3 startPos, Vector3 direction)
    {
        var dest = startPos + direction * 10.0f;
        RaycastHit hit;
        if (!Physics.Linecast(startPos, dest, out hit, WorldMask | SignalTargetMask))
        {
            if (Physics.Linecast(startPos, dest, out hit, MirrorMask))
            {
                AddReflection(startPos, hit.point);

                var mirror = hit.collider.gameObject;
                mirror.layer = LayerMask.NameToLayer("Default");
                CreateReflections(hit.point, Vector3.Reflect(direction, mirror.transform.forward));
                mirror.layer = LayerMask.NameToLayer("Mirror");
            }
            else
                AddReflection(startPos, dest);
        }
        else
        {
            AddReflection(startPos, hit.point);

            if (1 << hit.collider.gameObject.layer == SignalTargetMask && _signal.TargetGameObject == hit.collider.gameObject)
            {
                //_signal.TotalData -= Time.deltaTime;
                SignalCreator.Signals.Remove(_signal);
                GameObject.Destroy(hit.collider.gameObject);
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void AddReflection(Vector3 startPos, Vector3 endPos)
    {
        var reflection = GameObject.Instantiate(ReflectionPrefab);
        var lineRenderer = reflection.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        reflection.transform.parent = gameObject.transform;
        _reflections.Add(reflection);
    }
}