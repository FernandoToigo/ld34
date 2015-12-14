using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour {
    float _t;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _t += 0.05f;
        this.transform.localRotation = Quaternion.Euler(0.0f, _t, 0.0f);

        var sun = GameObject.Find("Sun").transform.FindChild("SunLight");
        var camera = Camera.main;
        
        var smallLights = this.transform.FindChild("SmallLights");

        for (var i = 0; i< smallLights.childCount; i++)
        {
            var smallLight = smallLights.GetChild(i);
            var dir = smallLight.transform.position;

            var d = Vector3.Dot(-dir, sun.forward);

            if (d > 0)
                smallLight.gameObject.SetActive(false);
            else
                smallLight.gameObject.SetActive(true);

        }
    }
}
