using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelControl : MonoBehaviour
{
    public const int MAX_LIFES = 3;

    private static Level _currentLevel;
    public static Level CurrentLevel
    {
        get { return _currentLevel; }
    }

    private static int _lifes = MAX_LIFES;
    public static int Lifes
    {
        get { return _lifes; }
        set { _lifes = value; }
    }

    private static float _dataTransmitted;
    public static float DataTransmitted
    {
        get { return _dataTransmitted; }
        set { _dataTransmitted = value; }
    }

    public List<Level> Levels
    {
        get
        {
            return _levels;
        }
    }

    private Image _fadePanel;
    private Text _levelText;
    private CommandTextWriter _writer;
    private List<Level> _levels;
    private FloatAnimator _animator;

    private float _timer = 0.0f;
    private bool _starting = false;
    private bool _ending = false;
    private bool _deathing = false;
    private bool _gameEnded = false;

    void Start()
    {
        _animator = new FloatAnimator();
        _fadePanel = GameObject.Find("Canvas").transform.FindChild("Fade").GetComponent<Image>();
        _levelText = _fadePanel.transform.FindChild("LevelText").GetComponent<Text>();
        _writer = GetComponent<CommandTextWriter>();
        InitializeLevels();
        //StartLevel(_levels[0]);
    }

    private void InitializeLevels()
    {
        _levels = new List<Level>
        {
            new Level
            {
                Number = 1,
                TotalData = 20.0f,
                MaxAngleDistance = 45.0f * Mathf.Deg2Rad,
                MinAngleDistance = 10.0f * Mathf.Deg2Rad,
                MaxConcurrentSignals = 1
            },
            new Level
            {
                Number = 2,
                TotalData = 40.0f,
                MaxAngleDistance = 90.0f * Mathf.Deg2Rad,
                MinAngleDistance = 45.0f * Mathf.Deg2Rad,
                MaxConcurrentSignals = 1
            },
            new Level
            {
                Number = 3,
                TotalData = 60.0f,
                MaxAngleDistance = 180.0f * Mathf.Deg2Rad,
                MinAngleDistance = 90.0f * Mathf.Deg2Rad,
                MaxConcurrentSignals = 1
            },
            new Level
            {
                Number = 4,
                TotalData = 100.0f,
                MaxAngleDistance = 45.0f * Mathf.Deg2Rad,
                MinAngleDistance = 10.0f * Mathf.Deg2Rad,
                MaxConcurrentSignals = 2
            },
            new Level
            {
                Number = 5,
                TotalData = 150.0f,
                MaxAngleDistance = 90.0f * Mathf.Deg2Rad,
                MinAngleDistance = 45.0f * Mathf.Deg2Rad,
                MaxConcurrentSignals = 2
            },
            new Level
            {
                Number = 6,
                TotalData = 200.0f,
                MaxAngleDistance = 180.0f * Mathf.Deg2Rad,
                MinAngleDistance = 90.0f * Mathf.Deg2Rad,
                MaxConcurrentSignals = 2
            }
        };
    }

    void Update()
    {
        _animator.Update();

        if (_currentLevel == null)
            return;

        if (!_deathing && LevelControl.Lifes <= 0)
        {
            ShowDeath();
        }

        if (!_ending && _dataTransmitted >= _currentLevel.TotalData)
        {
            EndLevel();
        }

        if (_starting)
        {
            if (_timer > 0.0f)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _animator.Animate(new FloatAnimation(
                    0.74f,
                    0.0f,
                    3000,
                    alpha =>
                    {
                        _fadePanel.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                        //_levelText.color = new Color(_levelText.color.r, _levelText.color.g, _levelText.color.b, alpha);
                        //_levelText.text = string.Format(_levelText.text, ((int)(alpha * 255.0f)).ToString("X").PadLeft(2, '0'));
                    },
                    () =>
                    {
                        SignalCreator.CreateSignals = true;
                    },
                    FloatAnimation.EaseInOutQuint));
                    _writer.StopWriting();
                    _levelText.text = "";
                    _starting = false;
                }
            }
        }

        if (_ending)
        {
            if (_timer > 0.0f)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _ending = false;
                    var nextIndex = Levels.IndexOf(_currentLevel) + 1;
                    if (nextIndex >= Levels.Count)
                        EndGame();
                    else
                        StartLevel(Levels[nextIndex]);
                }
            }
        }

        if (_deathing)
        {
            if (_timer > 0.0f)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _deathing = false;
                    StartLevel(_currentLevel);
                }
            }
        }
    }

    public void StartLevel(Level level)
    {
        _dataTransmitted = 0.0f;
        _lifes = MAX_LIFES;
        _currentLevel = level;
        var text = string.Format("Level {0} - Transmit a total of {1} gigabytes", level.Number.ToString(), level.TotalData.ToString("0.#"));
        var colors = new List<CommandTextWriter.ColorIndex>
        {
            new CommandTextWriter.ColorIndex("6EFF6EFF", 29 + level.Number.ToString().Length, 10 + level.TotalData.ToString("0.#").Length)
        };

        _writer.WriteText(text, _levelText, colors, new List<CommandTextWriter.SizeIndex>());

        _fadePanel.color = new Color(0.0f, 0.0f, 0.0f, 0.74f);
        _levelText.text = "";
        _starting = true;
        _timer = 8.0f;
    }

    void EndLevel()
    {
        SignalCreator.CancelAllSignals();
        SignalCreator.CreateSignals = false;
        _ending = true;

        _animator.Animate(new FloatAnimation(
            0.0f,
            0.74f,
            3000,
            alpha =>
            {
                _fadePanel.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            },
            () =>
            {
                var text = string.Format("Level {0} complete", _currentLevel.Number.ToString());
                var colors = new List<CommandTextWriter.ColorIndex>
                {
                    new CommandTextWriter.ColorIndex("6EFF6EFF", 0, text.Length)
                };
                _writer.WriteText(text, _levelText, colors, new List<CommandTextWriter.SizeIndex>());
            },
            FloatAnimation.EaseOutQuint));

        _timer = 8.0f;
        _levelText.text = "";
    }

    public void EndGame()
    {
        _dataTransmitted = 0.0f;
        _lifes = MAX_LIFES;
        var text = "All levels completed\n\nCongratulations!";
        var colors = new List<CommandTextWriter.ColorIndex>
        {
            new CommandTextWriter.ColorIndex("6EFF6EFF", 0, text.Length)
        };
        _writer.WriteText(text, _levelText, colors, new List<CommandTextWriter.SizeIndex>());

        _levelText.text = "";

        _animator.Animate(new FloatAnimation(
            0.0f,
            0.0f,
            8000,
            oi =>
            {
            },
            () =>
            {
                ShowCredits(false);
            }));
    }

    public void ShowCredits(bool fromMenu)
    {
        var text = "Credits\n\nFernando Molon Toigo - Programmer\nFilipe Scur - Programmer/Artist\n\n\"Peace of Mind\" Kevin MacLeod (incompetech.com)\nLicensed under Creative Commons: By Attribution 3.0\nhttp://creativecommons.org/licenses/by/3.0/\n\n\nThank you for playing!";
        var colors = new List<CommandTextWriter.ColorIndex>
        {
            new CommandTextWriter.ColorIndex("6EFF6EFF", 0, 7),
            new CommandTextWriter.ColorIndex("6EFF6EFF", 9, 20),
            new CommandTextWriter.ColorIndex("6EFF6EFF", 43, 11),
            new CommandTextWriter.ColorIndex("6EFF6EFF", 75, 31),
            new CommandTextWriter.ColorIndex("6EFF6EFF", text.Length - 23, 23)
        };
        _writer.WriteText(text, _levelText, colors, new List<CommandTextWriter.SizeIndex>());

        _animator.Animate(new FloatAnimation(
                    0.0f,
                    0.0f,
                    25000,
                    alpha =>
                    {
                    },
                    () =>
                    {
                        _levelText.text = "";
                        _writer.StopWriting();

                        _animator.Animate(new FloatAnimation(
                        0.87f,
                        0.0f,
                        3000,
                        alpha =>
                        {
                            _fadePanel.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                                //_levelText.color = new Color(_levelText.color.r, _levelText.color.g, _levelText.color.b, alpha);
                                //_levelText.text = string.Format(_levelText.text, ((int)(alpha * 255.0f)).ToString("X").PadLeft(2, '0'));
                            },
                        () =>
                        {
                            GameObject.Find("Menu").GetComponent<Menu>().Executed = false;

                            if (!fromMenu)
                            {
                                var menu = GameObject.Find("Menu");
                                _animator.Animate(
                                    new FloatAnimation(25, 0, 3000, a =>
                                    {
                                        menu.transform.localPosition = new Vector3(0.0f, 0.0f, -a);
                                    }, () =>
                                    {
                                        GameObject.Find("Menu").GetComponent<Menu>().onMenu = true;
                                    }
                                    , FloatAnimation.EaseInOutQuint));
                            }
                        },
                        FloatAnimation.EaseInOutQuint));
                    }));
    }

    public void ShowDeath()
    {
        SignalCreator.CancelAllSignals();
        SignalCreator.CreateSignals = false;
        _deathing = true;

        _animator.Animate(new FloatAnimation(
            0.0f,
            0.74f,
            3000,
            alpha =>
            {
                _fadePanel.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            },
            () =>
            {
                var text = "too many timeouts";
                var colors = new List<CommandTextWriter.ColorIndex>
                {
                    new CommandTextWriter.ColorIndex("FF6E6EFF", 0, text.Length)
                };
                _writer.WriteText(text, _levelText, colors, new List<CommandTextWriter.SizeIndex>());
            },
            FloatAnimation.EaseOutQuint));

        _timer = 8.0f;
        _levelText.text = "";
    }

    public class Level
    {
        public int Number;
        public float TotalData;
        public float MinAngleDistance;
        public float MaxAngleDistance;
        public float MaxConcurrentSignals;
    }
}