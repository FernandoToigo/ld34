using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudsSpawner : MonoBehaviour {

    public GameObject[] CloudsPrefabs;

    List<GameObject> _clouds;

    public float Speed;
    public uint Count;

    float _t = 0.0f;
    // Use this for initialization
    void Start()
    {
        SpawnClouds(Count);
    }

    void SpawnClouds(uint n)
    {
        var cloudPrefab = CloudsPrefabs[Random.Range(0, CloudsPrefabs.Length)];

        for (int i = 0; i < n; i++)
        {
            var cloud = GameObject.Instantiate(cloudPrefab);

            cloud.transform.parent = this.transform;

            var radius = Random.Range(4.2f, 4.4f);
            var theta = Random.Range(0.0f, Mathf.PI * 2.0f);
            var phi = Random.Range(0.0f, Mathf.PI);

            var x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            var y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            var z = radius * Mathf.Cos(phi);

            cloud.transform.localPosition = new Vector3(x, y, z);
            var up = Vector3.Normalize(new Vector3(0.0f, 1.0f, 0.0f));
            var dir = -Vector3.Normalize(cloud.transform.localPosition);
            var forward = Vector3.Cross(dir, -up);
            up = Vector3.Cross(forward, dir);
            dir = Vector3.Cross(-up, forward);

            cloud.transform.localRotation = Quaternion.LookRotation(forward, up);

            var s = Random.Range(0.3f, 0.6f);
            cloud.transform.localScale = new Vector3(s, s, s);

        }
        //_stars = new List<GameObject>();

        //for (int i = 0; i < n; i++)
        //{
        //    _stars.Add();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime * Speed;

        this.transform.localRotation = Quaternion.Euler(0.0f, _t, 0.0f);
    }
}
