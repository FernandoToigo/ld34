using UnityEngine;
using System.Collections;

public class Satelite : MonoBehaviour {

    float RotationSpeedIncrement = 0.5f;
    float TranslationSpeedIncrement = 0.005f;

    float _rotationSpeed = 0.0f;
    float _translationSpeed = 0.01f;
    float _maxTraslationSpeed = 1.0f;
    float _maxRotationSpeed = 5.0f;
    
    ParticleSystem _rightTurbineThrusterAnimation;
    ParticleSystem _leftTurbineThrusterAnimation;
    AudioSource _leftTurbineThrusterSound;
    AudioSource _rightTurbineThrusterSound;
    AngleThing _angleThing;

    public bool OnMenu = true;

    // Use this for initialization
    void Start()
    {
        _angleThing = GetComponent<AngleThing>();

        //this.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

        _rightTurbineThrusterAnimation = this.transform.FindChild("RightTurbine").GetComponent<ParticleSystem>();
        _rightTurbineThrusterAnimation.Stop();

        _rightTurbineThrusterSound = this.transform.FindChild("RightTurbine").GetComponent<AudioSource>();
        _rightTurbineThrusterSound.loop = true;
        _rightTurbineThrusterSound.Stop();

        _leftTurbineThrusterAnimation = this.transform.FindChild("LeftTurbine").GetComponent<ParticleSystem>();
        _leftTurbineThrusterAnimation.Stop();

        _leftTurbineThrusterSound = this.transform.FindChild("LeftTurbine").GetComponent<AudioSource>();
        _leftTurbineThrusterSound.loop = true;
        _leftTurbineThrusterSound.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (OnMenu)
            return;

        //return;

        bool leftArrowPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightArrowPressed = Input.GetKey(KeyCode.RightArrow);

        var v0 = Vector3.Normalize(Vector3.Cross(-this.transform.position, new Vector3(0.0f, 0.0f, 1.0f)));

        var percentTranslation = Mathf.Abs(_translationSpeed) / _maxTraslationSpeed;
        var trueRotationSpeed = RotationSpeedIncrement + percentTranslation;

        if (leftArrowPressed && rightArrowPressed)
        {
            //RotationSpeedIncrement = 0.00f;
            TranslationSpeedIncrement += 0.005f * Time.deltaTime;
            _rightTurbineThrusterAnimation.Play();

            if (!_rightTurbineThrusterSound.isPlaying)
                _rightTurbineThrusterSound.Play();

            _leftTurbineThrusterAnimation.Play();

            if (!_leftTurbineThrusterSound.isPlaying)
                _leftTurbineThrusterSound.Play();
        }
        else if (leftArrowPressed)
        {
            TranslationSpeedIncrement = 0.0f;
            _rotationSpeed = Mathf.Clamp(_rotationSpeed + trueRotationSpeed * Time.deltaTime, -_maxRotationSpeed, _maxRotationSpeed);
            //RotationSpeedIncrement += 0.015f * Time.deltaTime;
            _leftTurbineThrusterAnimation.Play();

            if (!_leftTurbineThrusterSound.isPlaying)
                _leftTurbineThrusterSound.Play();

            _rightTurbineThrusterAnimation.Stop();
            _rightTurbineThrusterSound.Stop();
        }
        else if (rightArrowPressed)
        {
            TranslationSpeedIncrement = 0.0f;
            //RotationSpeedIncrement -= 0.015f * Time.deltaTime;
            _rotationSpeed = Mathf.Clamp(_rotationSpeed - trueRotationSpeed * Time.deltaTime, -_maxRotationSpeed, _maxRotationSpeed);

            _rightTurbineThrusterAnimation.Play();

            if (!_rightTurbineThrusterSound.isPlaying)
                _rightTurbineThrusterSound.Play();

            _leftTurbineThrusterAnimation.Stop();
            _leftTurbineThrusterSound.Stop();
        }
        else
        {
            _rightTurbineThrusterAnimation.Stop();
            _rightTurbineThrusterSound.Stop();
            _leftTurbineThrusterAnimation.Stop();
            _leftTurbineThrusterSound.Stop();
            TranslationSpeedIncrement = 0.0f;
        }

        //RotationSpeedIncrement = Mathf.Clamp(RotationSpeedIncrement, -0.1f, 0.1f);
        TranslationSpeedIncrement = Mathf.Clamp(TranslationSpeedIncrement, 0.0f, 0.01f);

        _translationSpeed += TranslationSpeedIncrement * Vector3.Dot(-Vector3.Normalize(this.transform.up), Vector3.Normalize(v0));
        //_translationSpeed += TranslationSpeedIncrement * Vector3.Dot(-this.transform.up, v0);

        _translationSpeed = Mathf.Clamp(_translationSpeed, -_maxTraslationSpeed, _maxTraslationSpeed);

        //Debug.Log("r:  " + _rotationSpeed + "; t%:  " + _translationSpeed / _maxTraslationSpeed);

        var eulerZ = this.transform.localRotation.eulerAngles.z;
        this.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, eulerZ + _rotationSpeed);

        _angleThing.Angle += Time.deltaTime * _translationSpeed;
        this.transform.position = _angleThing.Direction * 6.0f;

        //Debug.DrawLine(this.transform.position, this.transform.position - this.transform.up * 1.0f, Color.cyan);
        //Debug.DrawLine(this.transform.position, new Vector3(), Color.magenta);
        //Debug.DrawLine(this.transform.position, this.transform.position - v0, Color.green);
    }
}
