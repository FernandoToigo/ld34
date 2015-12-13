using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    float _t = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.localRotation = Quaternion.Euler(30.0f, _t, 0.0f);

        _t += Time.deltaTime * 0.05f;

        var q0 = Quaternion.AngleAxis(-30.0f, Vector3.Normalize(new Vector3(0.0f, 0.7f, 0.3f)));
        this.transform.position = (q0 * new Vector3(Mathf.Cos(_t) * -60, 0.5f, Mathf.Sin(_t) * 120));
    }
}
