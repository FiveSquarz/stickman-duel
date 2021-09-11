using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour {

    [SerializeField]
    float forwardSpeed = 0f,
        backwardSpeed = 0f,
        waitStart = 0f,
        waitRestart = 0f,
        waitReverse = 0f;
    [SerializeField]
    bool physical = false;

    enum Modes {reverseBackwards, reverseReset, restart}
    [SerializeField]
    Modes mode = Modes.reverseBackwards;

    enum Directions { In, Out, InOut }
    [SerializeField]
    Directions direction = Directions.Out;

    enum Styles { Linear, Sine, Quad, Cubic, Quart, Quint, Expo, Circ, Back, Elastic, Bounce }
    [SerializeField]
    Styles style = Styles.Linear;

    protected delegate void baseDelegate();
    protected baseDelegate move;

    float progress, startTime, returnedTime, reachedEndTime;
    bool forward = true;

    protected virtual IEnumerator Start() {
        startTime = Time.time;
        returnedTime = reachedEndTime = Mathf.NegativeInfinity;
        while (true) {
            if (Time.time >= startTime + waitStart && Time.time >= returnedTime + waitRestart && Time.time >= reachedEndTime + waitReverse) {
                progress = Mathf.Clamp01(progress + (forward ? forwardSpeed : -backwardSpeed) * (physical ? Time.fixedDeltaTime : Time.deltaTime));
                if (progress == 1f) {
                    if (mode == Modes.reverseBackwards || mode == Modes.reverseReset) {
                        reachedEndTime = Time.time;
                        forward = false;
                        if (mode == Modes.reverseReset && direction != Directions.InOut) direction = direction == Directions.In ? Directions.Out : Directions.In;
                    } else {
                        returnedTime = Time.time;
                        progress = 0f;
                    }
                } else if (progress == 0f) {
                    if (mode == Modes.reverseBackwards || mode == Modes.reverseReset) {
                        returnedTime = Time.time;
                        forward = true;
                        if (mode == Modes.reverseReset && direction != Directions.InOut) direction = direction == Directions.In ? Directions.Out : Directions.In;
                    }
                }
                move();
            }
            yield return physical ? new WaitForFixedUpdate() : null;
        }
    }

    protected float GetAmount() {
        return style == Styles.Linear ? progress : functions[direction.ToString() + style.ToString()](progress);
    }

    static readonly Dictionary<string, Func<float, float>> functions = new Dictionary<string, Func<float, float>>() {
        {"InSine", x => 1f - Mathf.Cos(x * Mathf.PI / 2f)},
        {"OutSine", x => Mathf.Sin(x * Mathf.PI / 2f)},
        {"InOutSine", x => -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f},

        {"InQuad", x => x * x},
        {"OutQuad", x => 1f - (1f - x) * (1f - x)},
        {"InOutQuad", x => x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f},

        {"InCubic", x =>  x * x * x},
        {"OutCubic", x => 1f - Mathf.Pow(1f - x, 3f)},
        {"InOutCubic", x => x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f},

        {"InQuart", x => x * x * x * x},
        {"OutQuart", x => 1f - Mathf.Pow(1f - x, 4f)},
        {"InOutQuart", x => x < 0.5f ? 8f * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f},

        {"InQuint", x => x * x * x * x * x},
        {"OutQuint", x => 1f - Mathf.Pow(1f - x, 5f)},
        {"InOutQuint", x => x < 0.5f ? 16f * x * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f},

        {"InExpo", x => x == 0f ? 0f : Mathf.Pow(2f, 10f * x - 10f)},
        {"OutExpo", x => x == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * x)},
        {"InOutExpo", x => x == 0f ? 0f : x == 1f ? 1f : x < 0.5f ? Mathf.Pow(2f, 20f * x - 10f) / 2f : (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f},

        {"InCirc", x => 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f))},
        {"OutCirc", x => Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f))},
        {"InOutCirc", x => x < 0.5f ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f : (Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f},

        {"InBack", x => 2.70158f * x * x * x - 1.70158f * x * x},
        {"OutBack", x => 1f + 2.70158f * Mathf.Pow(x - 1f, 3f) + 1.70158f * Mathf.Pow(x - 1f, 2f)},
        {"InOutBack", x => x < 0.5f ? Mathf.Pow(2f * x, 2f) * (3.5949095f * 2f * x - 2.5949095f) / 2f : (Mathf.Pow(2f * x - 2f, 2f) * (3.5949095f * (x * 2f - 2f) + 2.5949095f) + 2f) / 2f},

        {"InElastic", x => x == 0f ? 0f : x == 1f ? 1f : -Mathf.Pow(2f, 10f * x - 10f) * Mathf.Sin((x * 10f - 10.75f) * 2f * Mathf.PI / 3f)},
        {"OutElastic", x => x == 0f ? 0f : x == 1f ? 1f : Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * 2f * Mathf.PI / 3f) + 1f},
        {"InOutElastic", x => x == 0f ? 0f : x == 1f ? 1f : x < 0.5f ? -Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * 2f * Mathf.PI / 4.5f) / 2f : Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * 2f * Mathf.PI / 4.5f) / 2f + 1f},

        {"InBounce", x => 1f - functions["OutBounce"](1f - x)},
        {"OutBounce", x => x < 1f / 2.75f ? 7.5625f * x * x : x < 2f / 2.75f ? 7.5625f * (x -= 1.5f / 2.75f) * x + 0.75f : x < 2.5f / 2.75f ? 7.5625f * (x -= 2.25f / 2.75f) * x + 0.9375f : 7.5625f * (x -= 2.625f / 2.75f) * x + 0.984375f},
        {"InOutBounce", x => x < 0.5f ? (1f - functions["OutBounce"](1f - 2f * x)) / 2f : (1f + functions["OutBounce"](2f * x - 1f)) / 2f}
    };
}
