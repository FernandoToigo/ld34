﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Material SelectedMaterial;
    public Material Material;

    GameObject playButton;
    GameObject creditsButton;
    GameObject exitButton;

    GameObject[] _buttons;

    Action[] _actions;
    FloatAnimator _animator;


    // Use this for initialization
    void Start()
    {
        playButton = this.transform.FindChild("PlayButton").gameObject;
        creditsButton = this.transform.FindChild("CreditsButton").gameObject;
        exitButton = this.transform.FindChild("ExitButton").gameObject;

        _animator = new FloatAnimator();

        _actions = new Action[]
            {
                ()=> {
                    GameObject.Find("Satellite").GetComponent<Animator>().SetBool("SatelliteGo", true);

                    var leftTurbineParticle = GameObject.Find("Satellite").transform.FindChild("LeftTurbine").GetComponent<ParticleSystem>();
                    var rightTurbineParticle = GameObject.Find("Satellite").transform.FindChild("RightTurbine").GetComponent<ParticleSystem>();
                    var leftTurbineSound = GameObject.Find("Satellite").transform.FindChild("LeftTurbine").GetComponent<AudioSource>();
                    var rightTurbineSound = GameObject.Find("Satellite").transform.FindChild("RightTurbine").GetComponent<AudioSource>();

                    rightTurbineParticle.Play();

                    if (!rightTurbineSound.isPlaying)
                        rightTurbineSound.Play();

                    leftTurbineParticle.Play();

                    if (!leftTurbineSound.isPlaying)
                        leftTurbineSound.Play();

                    _animator.Animate(
                        new FloatAnimation(0, 50, 6000, a => {

                            this.transform.localPosition = new Vector3(0.0f, 0.0f, -a);
                        }, () =>
                        {
                            var sat = GameObject.Find("Satellite");
                            sat.GetComponent<Animator>().SetBool("SatelliteGo", false);
                            sat.GetComponent<Animator>().enabled = false;
                            sat.transform.localPosition = new Vector3(6.0f, 0.0f, 0.0f);
                            sat.transform.localRotation = Quaternion.Euler(0, -540, 180);
                            sat.GetComponent<Satelite>().OnMenu = false;
                            onMenu = false;

                            rightTurbineParticle.Stop();
                            rightTurbineSound.Stop();
                            leftTurbineParticle.Stop();
                            leftTurbineSound.Stop();

                            var fadePanel = GameObject.Find("Canvas").transform.FindChild("Fade").GetComponent<Image>();

                            _animator.Animate(new FloatAnimation(
                            0.0f,
                            0.74f,
                            3000,
                            alpha =>
                            {
                                fadePanel.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                            },
                            () =>
                            {
                                var levelControl = GameObject.Find("LevelControl").GetComponent<LevelControl>();

                                DataLog.LogStatic("system initialized...", DataLog.SUCCESS_COLOR);
                                levelControl.StartLevel(levelControl.Levels[0]);

                            },
                            FloatAnimation.EaseOutQuint));
                        }));
                },
                ()=> {

                        var fadePanel = GameObject.Find("Canvas").transform.FindChild("Fade").GetComponent<Image>();

                        _animator.Animate(new FloatAnimation(
                        0.0f,
                        0.87f,
                        3000,
                        alpha =>
                        {
                            fadePanel.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                        },
                        () =>
                        {
                            var levelControl = GameObject.Find("LevelControl").GetComponent<LevelControl>();
                            levelControl.ShowCredits(true);
                        },
                        FloatAnimation.EaseOutQuint));


                },
                ()=> { Application.Quit(); },
            };

        _buttons = new GameObject[]
            {
                playButton,
                creditsButton,
                exitButton
            };

        playButton.transform.FindChild("Model").transform.FindChild("default").GetComponent<MeshRenderer>().material = Material;
        creditsButton.transform.FindChild("Model").transform.FindChild("default").GetComponent<MeshRenderer>().material = Material;
        exitButton.transform.FindChild("Model").transform.FindChild("default").GetComponent<MeshRenderer>().material = Material;
    }

    int _selectedIndex = 0;
    bool onMenu = true;
    float _lastPressed;
    float _delay = 0.05f;
    public bool Executed = false;
    // Update is called once per frame
    void Update()
    {
        _animator.Update();

        if (!onMenu)
            return;

        if (Executed)
            return;

        bool leftArrowPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightArrowPressed = Input.GetKey(KeyCode.RightArrow);

        if (leftArrowPressed && rightArrowPressed)
        {
            _actions[_selectedIndex]();
            Executed = true;
        }
        else if (leftArrowPressed)
        {
            if (_lastPressed < 0.0f)
            {
                _selectedIndex--;
                if (_selectedIndex < 0)
                    _selectedIndex = 2;

                _lastPressed = _delay;
            }
            else
                _lastPressed -= Time.deltaTime;
        }
        else if (rightArrowPressed)
        {
            if (_lastPressed < 0.0f)
            {
                _selectedIndex++;
                if (_selectedIndex > 2)
                    _selectedIndex = 0;

                _lastPressed = _delay;
            }
            else
            {
                _lastPressed -= Time.deltaTime;
            }
        }
        else
        {
            _lastPressed = _delay;
        }

        playButton.transform.FindChild("Model").transform.FindChild("default").GetComponent<MeshRenderer>().material = Material;
        creditsButton.transform.FindChild("Model").transform.FindChild("default").GetComponent<MeshRenderer>().material = Material;
        exitButton.transform.FindChild("Model").transform.FindChild("default").GetComponent<MeshRenderer>().material = Material;

        var selected = _buttons[_selectedIndex];
        selected.transform.FindChild("Model").transform.FindChild("default").GetComponent<MeshRenderer>().material = SelectedMaterial;
    }
}
