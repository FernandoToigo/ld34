using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour {
    float _t;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _t += 0.1f;
        this.transform.localRotation = Quaternion.Euler(0.0f, _t, 0.0f);
    }
}
