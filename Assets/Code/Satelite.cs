﻿using UnityEngine;
using System.Collections;

public class Satelite : MonoBehaviour {

    float RotationSpeedIncrement = 0.1f;
    float TranslationSpeedIncrement = 0.005f;

    float _rotationSpeed = 0.0f;
    float _translationSpeed = 0.01f;
    float _maxTraslationSpeed = 1.0f;
    float _maxRotationSpeed = 10.0f;
    
    ParticleSystem _rightTurbine;
    ParticleSystem _leftTurbine;
    AngleThing _angleThing;

    // Use this for initialization
    void Start ()
    {
        _angleThing = GetComponent<AngleThing>();

        this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

        _rightTurbine = this.transform.FindChild("RightTurbine").GetComponent<ParticleSystem>();
        _rightTurbine.Stop();

        _leftTurbine = this.transform.FindChild("LeftTurbine").GetComponent<ParticleSystem>();
        _leftTurbine.Stop();
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
            _rightTurbine.Play();
            _leftTurbine.Play();
        }
        else if (leftArrowPressed)
        {
            _rotationSpeed += RotationSpeedIncrement * Mathf.Abs((_translationSpeed / _maxTraslationSpeed));
            _leftTurbine.Play();
            _rightTurbine.Stop();
        }
        else if (rightArrowPressed)
        {
            _rotationSpeed -= RotationSpeedIncrement * Mathf.Abs((_translationSpeed / _maxTraslationSpeed));
            _rightTurbine.Play();
            _leftTurbine.Stop();
        }
        else
        {
            _rightTurbine.Stop();
            _leftTurbine.Stop();
        }

        _translationSpeed = Mathf.Clamp(_translationSpeed, -_maxTraslationSpeed, _maxTraslationSpeed);
        _rotationSpeed = Mathf.Clamp(_rotationSpeed, -_maxRotationSpeed, _maxRotationSpeed);
        
        //Debug.Log("r:  " + _rotationSpeed + "; t%:  " + _translationSpeed / _maxTraslationSpeed);

        var eulerZ = this.transform.localRotation.eulerAngles.z;
        this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, eulerZ + _rotationSpeed);

        _angleThing.Angle += Time.deltaTime * _translationSpeed;

        var x = Mathf.Cos(_angleThing.Angle) * 8.0f;
        var y = Mathf.Sin(_angleThing.Angle) * 8.0f;

        this.transform.position = new Vector3(x, y, 0.0f);
        

        Debug.DrawLine(this.transform.position, this.transform.position - this.transform.up * 1.0f, Color.cyan);
        Debug.DrawLine(this.transform.position, new Vector3(), Color.magenta);
        Debug.DrawLine(this.transform.position, this.transform.position - v0, Color.green);

    }
}
