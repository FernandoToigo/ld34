using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour {

    float _t = 0.0f;
    float _l = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.localRotation = Quaternion.Euler(-60.0f, _t, 0.0f);

        _t += Time.deltaTime * 0.2f;
        _l += Time.deltaTime;

        //var q = Quaternion.AngleAxis(_t * Mathf.Rad2Deg, Vector3.Normalize(new Vector3(0.0f, 0.0f, 1.0f)));

        var up = Vector3.Normalize(new Vector3(0.0f, 1.0f, 0.0f));
        var dir = -Vector3.Normalize(this.transform.localPosition);
        var forward = Vector3.Cross(dir, -up);
        up = Vector3.Cross(forward, dir);
        dir = Vector3.Cross(-up, forward);

        this.transform.localRotation = Quaternion.LookRotation(forward, up);

        this.transform.localPosition = new Vector3(Mathf.Cos(_t) * + 4.2f, 0.0f, Mathf.Sin(_t) * 4.2f);


        var redLight = this.transform.FindChild("RedLight").GetComponent<Light>().intensity = Mathf.Cos(_l * 10);

        var blueLight = this.transform.FindChild("BlueLight").GetComponent<Light>().intensity = Mathf.Sin(_l * 10);
    }
}
