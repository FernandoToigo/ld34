using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class FloatAnimator
{
    private List<FloatAnimation> _animations;
    private List<FloatAnimation> _toAddAnimations;
    private List<FloatAnimation> _toCancelAnimations;

    public FloatAnimator()
    {
        _animations = new List<FloatAnimation>();
        _toAddAnimations = new List<FloatAnimation>();
        _toCancelAnimations = new List<FloatAnimation>();
    }

    public void Update()
    {
        foreach (var animation in _toAddAnimations)
            _animations.Add(animation);
        _toAddAnimations.Clear();

        foreach (var animation in _toCancelAnimations)
            _animations.Remove(animation);
        _toCancelAnimations.Clear();

        var toDelete = new List<FloatAnimation>();

        foreach (var animation in _animations)
        {
            animation.ElapsedDelay += (int)(Time.deltaTime * 1000);
            if (animation.ElapsedDelay < animation.Delay)
                continue;

            animation.Elapsed += (int)(Time.deltaTime * 1000);

            var from = animation.From;
            var to = animation.To;
            var elapsed = (float)animation.Elapsed;
            var milliseconds = (float)animation.Milliseconds;

            var percent = Mathf.Min(Mathf.Max(elapsed / milliseconds, 0.0f), 1.0f);
            //var diff = Mathf.Sin(percent * Mathf.PI * 0.5f);
            //var diff = Mathf.Sin(percent * Mathf.PI - Mathf.PI * 0.5f) * 0.5f + 0.5f;
            //var diff = Mathf.SmoothStep(0.0f, 1.0f, percent);
            var t = percent;
            var diff = animation.EasingFunction(t);

            var value = from + (to - from) * diff;

            if (animation.ProgressCallback != null)
                animation.ProgressCallback(value);

            if (percent >= 1.0f)
            {
                toDelete.Add(animation);
                if (animation.FinishedCallback != null)
                    animation.FinishedCallback();
            }
        }

        foreach (var animation in toDelete)
            _animations.Remove(animation);
    }

    public void Animate(FloatAnimation animation)
    {
        _toAddAnimations.Add(animation);
    }

    public void CancelAllAnimations()
    {
        _toCancelAnimations.AddRange(_animations);
    }

    public void CancelAllAnimationsByTag(string tag)
    {
        _toCancelAnimations.AddRange(_animations.Where(a => a.Tag.Equals(tag)));
    }

    public void CancelAnimation(FloatAnimation animation)
    {
        _toCancelAnimations.Add(animation);
    }
}

public class FloatAnimation
{
    private float _from;
    public float From
    {
        get { return _from; }
    }

    private float _to;
    public float To
    {
        get { return _to; }
    }

    private int _milliseconds;
    public int Milliseconds
    {
        get { return _milliseconds; }
    }

    private int _elapsed;
    public int Elapsed
    {
        get { return _elapsed; }
        set { _elapsed = value; }
    }

    private int _delay;
    public int Delay
    {
        get { return _delay; }
    }

    private int _elapsedDelay;
    public int ElapsedDelay
    {
        get { return _elapsedDelay; }
        set { _elapsedDelay = value; }
    }

    private Action<float> _progressCallback;
    public Action<float> ProgressCallback
    {
        get { return _progressCallback; }
    }

    private Action _finishedCallback;
    public Action FinishedCallback
    {
        get { return _finishedCallback; }
    }

    private Func<float, float> _easingFunction;
    public Func<float, float> EasingFunction
    {
        get { return _easingFunction; }
    }

    private string _tag;
    public string Tag
    {
        get { return _tag; }
    }

    public FloatAnimation(float from, float to, int milliseconds, Action<float> progressCallback, Action finishedCallback, int delay = 0)
        : this(from, to, milliseconds, progressCallback, finishedCallback, FloatAnimation.EaseInOutQuint, delay)
    {
    }

    public FloatAnimation(float from, float to, int milliseconds, Action<float> progressCallback, Action finishedCallback, Func<float, float> easingFunction, int delay = 0, string tag = null)
    {
        _from = from;
        _to = to;
        _milliseconds = milliseconds;
        _progressCallback = progressCallback;
        _finishedCallback = finishedCallback;
        _easingFunction = easingFunction;
        _delay = delay;
        _tag = tag;
        Reset();
    }

    public void Reset()
    {
        _elapsed = 0;
        _elapsedDelay = 0;
    }

    public static float EaseInOutQuad(float t)
    {
        return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }

    public static float EaseInQuad(float t)
    {
        return t * t;
    }

    public static float EaseOutQuad(float t)
    {
        return t * (2 - t);
    }

    public static float EaseInOutQuint(float t)
    {
        return t < 0.5f ? 16.0f * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
    }

    public static float EaseInQuint(float t)
    {
        return t * t * t * t * t;
    }

    public static float EaseOutQuint(float t)
    {
        return 1 + (--t) * t * t * t * t;
    }

    public static float EaseOutElastic(float t)
    {
        if (t == 0)
            return 0.0f;
        if (t == 1)
            return 1.0f;
        float p = 0.3f;
        float a = 1.0f;
        float s = p / 4;
        return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t - s) * (2 * Mathf.PI) / p) + 1.0f);
    }

    public static float Linear(float t)
    {
        return t;
    }
}