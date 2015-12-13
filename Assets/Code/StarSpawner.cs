using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarSpawner : MonoBehaviour {

    public GameObject StarPrefab;

    public List<GameObject> _stars;

    float _t = 0.0f;
	// Use this for initialization
	void Start () {
        SpawnStars(500);
	}

    void SpawnStars(uint n)
    {
        for (int i = 0; i < n; i++)
        {
            var star = GameObject.Instantiate(StarPrefab);

            var r = Random.Range(0.3f, 7.0f);
            var g = Random.Range(0.5f, 1.0f);
            var b = Random.Range(0.5f, 1.0f);

            star.transform.FindChild("default").GetComponent<MeshRenderer>().material.color = new Color(r, g, b, 1.0f);

            star.transform.parent = this.transform;

            var radius = Random.Range(60.0f, 100.0f);
            var theta = Random.Range(0.0f, Mathf.PI * 2.0f);
            var phi = Random.Range(0.0f, Mathf.PI);

            var x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            var y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            var z = radius * Mathf.Cos(phi);

            star.transform.position = new Vector3(x, y, z);
        }
        //_stars = new List<GameObject>();

        //for (int i = 0; i < n; i++)
        //{
        //    _stars.Add();
        //}
    }

	// Update is called once per frame
	void Update ()
    {
        _t += Time.deltaTime * 1.5f;

        this.transform.localRotation = Quaternion.Euler(0.0f, _t, 0.0f);
    }
}
