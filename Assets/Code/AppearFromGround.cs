using UnityEngine;
using System.Collections;

public class AppearFromGround : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _direction;
    private float _distance;
    private float _currentDistance;
    private ParticleSystem _appearingParticlesSystem;

    private bool _appearing;
    public bool Appearing
    {
        get { return _appearing; }
    }

    void Awake()
    {
        _appearingParticlesSystem = transform.FindChild("AppearingParticles").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_appearing)
        {
            _currentDistance = Mathf.Min(_currentDistance + Time.deltaTime * 0.5f, _distance);
            transform.position = _startPos + _direction * _currentDistance;

            if (_currentDistance >= _distance)
            {
                _appearing = false;
                _appearingParticlesSystem.Stop();
            }
        }
    }

    public void Appear(Vector3 direction, float distance)
    {
        _direction = direction;
        _currentDistance = 0;
        _distance = distance;
        _startPos = transform.position;
        _appearing = true;
        _appearingParticlesSystem.Play();
    }
}