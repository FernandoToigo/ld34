using UnityEngine;
using System.Collections;
using System;

public class Menu : MonoBehaviour {

    public Material SelectedMaterial;
    public Material Material;

    GameObject playButton;
    GameObject creditsButton;
    GameObject exitButton;

    GameObject[] _buttons;

    Action[] _actions;

    // Use this for initialization
    void Start () {
        playButton = this.transform.FindChild("PlayButton").gameObject;
        creditsButton = this.transform.FindChild("CreditsButton").gameObject;
        exitButton = this.transform.FindChild("ExitButton").gameObject;

        _actions = new Action[]
            {
                ()=> {
                    //GameObject.Find("Satellite").GetComponent<Animator>().SetBool("SatelliteGo", true);
                    var animation = GameObject.Find("Satellite").GetComponent<Animation>();

                    animation["Intro0"].wrapMode = WrapMode.Once;
                    animation.Play("Intro0");
                },
                ()=> { },
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

    float _lastPressed;
    float _delay = 0.05f;
	// Update is called once per frame
	void Update ()
    {
        bool leftArrowPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightArrowPressed = Input.GetKey(KeyCode.RightArrow);

        if (leftArrowPressed && rightArrowPressed)
        {
            _actions[_selectedIndex]();
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
