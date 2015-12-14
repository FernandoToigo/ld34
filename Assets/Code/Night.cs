using UnityEngine;
using System.Collections;

public class Night : MonoBehaviour {

    // Use this for initialization
    float _t = 0.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime * 0.01f;
        this.transform.localPosition = new Vector3(Mathf.Cos(_t + Mathf.PI) * -120, 0.0f, Mathf.Sin(_t + Mathf.PI) * 120);
        this.transform.forward = -this.transform.localPosition;
    }
}
