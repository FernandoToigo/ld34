using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SignalSource : MonoBehaviour
{
    public LayerMask WorldMask;
    public LayerMask MirrorMask;
    public GameObject ReflectionPrefab;

    private GameObject _satellite;
    private GameObject _signalPos;
    private List<GameObject> _reflections;

    void Start()
    {
        _satellite = GameObject.Find("Satellite");
        _signalPos = transform.FindChild("SignalPos").gameObject;
        _reflections = new List<GameObject>();
    }
    
    void Update()
    {
        //foreach (var reflection in _reflections)
        //    GameObject.Destroy(reflection);
        //_reflections.Clear();

        //CreateReflections(_signalPos.transform.position, (_satellite.transform.position - _signalPos.transform.position).normalized);
    }

    private void CreateReflections(Vector3 startPos, Vector3 direction)
    {
        if (!Physics.Linecast(startPos, direction * 10.0f, WorldMask))
        {
            RaycastHit hit;
            if (Physics.Linecast(startPos, startPos, out hit, MirrorMask))
            {
                var reflection = GameObject.Instantiate(ReflectionPrefab);
                var lineRenderer = reflection.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, _signalPos.transform.position);
                lineRenderer.SetPosition(1, _satellite.transform.position);

                var mirror = hit.collider.gameObject;
                CreateReflections(hit.point, Vector3.Reflect(direction, mirror.transform.forward));

                _reflections.Add(reflection);
            }
        }
    }
}