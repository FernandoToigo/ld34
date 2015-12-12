﻿using UnityEngine;
using System.Collections;

public class Satelite : MonoBehaviour {

    float RotationSpeedIncrement = 0.05f;
    float TranslationSpeedIncrement = 0.005f;

    float _rotationSpeed = 0.0f;
    float _translationSpeed = 0.01f;
    float _maxTraslationSpeed = 1.0f;
    float _maxRotationSpeed = 30.0f;

    float _t = 0.0f;
    // Use this for initialization
    void Start ()
    {
        this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

    }

    // Update is called once per frame
    void Update ()
    {
        bool leftArrowPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightArrowPressed = Input.GetKey(KeyCode.RightArrow);

        var v0 = Vector3.Normalize(Vector3.Cross(-this.transform.position, new Vector3(0.0f, 0.0f, 1.0f)));

        if (leftArrowPressed && rightArrowPressed)
        {
            _translationSpeed += TranslationSpeedIncrement * Vector3.Dot(-this.transform.up, v0);
        }
        else if (leftArrowPressed)
            _rotationSpeed += RotationSpeedIncrement;
        else if (rightArrowPressed)
            _rotationSpeed -= RotationSpeedIncrement;

        _rotationSpeed = Mathf.Clamp(_rotationSpeed, -_maxRotationSpeed, _maxRotationSpeed);

        var eulerZ = this.transform.localRotation.eulerAngles.z;
        this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, eulerZ + _rotationSpeed);

        _t += Time.deltaTime * Mathf.Clamp(_translationSpeed, -_maxTraslationSpeed, _maxTraslationSpeed);

        var x = Mathf.Cos(_t ) * 6;
        var y = Mathf.Sin(_t) * 6;

        this.transform.position = new Vector3(x, y, 0.0f);
        

        Debug.DrawLine(this.transform.position, this.transform.position - this.transform.up * 1.0f, Color.cyan);
        Debug.DrawLine(this.transform.position, new Vector3(), Color.magenta);
        Debug.DrawLine(this.transform.position, this.transform.position - v0, Color.green);

    }
}