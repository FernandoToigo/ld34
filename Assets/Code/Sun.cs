using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

    float _t = 0.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _t += Time.deltaTime * 0.1f;
        this.transform.localPosition = new Vector3(Mathf.Cos(_t) * -120, 0.0f, Mathf.Sin(_t) * 120);
        this.transform.forward = -this.transform.localPosition;
    }
}
