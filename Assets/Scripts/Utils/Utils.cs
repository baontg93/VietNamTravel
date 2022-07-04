using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utils
{
    public static void ScrollToTop(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    public static void ScrollToLeft(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    public static void ScrollToRight(this ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(1, 0);
    }

    public static Sprite GetSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public static T GetValue<T>(this JObject obj, string key)
    {
        if (obj.GetValue(key) != null)
        {
            return obj.GetValue(key).ToObject<T>();
        }
        else
        {
            return default;
        }
    }

    static public void ForceOn(this Toggle toggle)
    {
        if (toggle.isOn)
        {
            toggle.SetIsOnWithoutNotify(false);
        }
        toggle.isOn = true;
    }

    static public T GetOrAddComponent<T>(this Component comp) where T : Component
    {
        return comp.gameObject.GetOrAddComponent<T>();
    }

    static public T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T result = gameObject.GetComponent<T>();
        if (result == null)
        {
            result = gameObject.AddComponent<T>();
        }
        return result;
    }

    /// <summary>
    /// Set alpha of sprite renderer
    /// </summary>
    /// <param name="spriteRenderer">Target renderer</param>
    /// <param name="value">Alpha value, from 0 to 255</param>
    public static void SetAlpha(this SpriteRenderer spriteRenderer, float value)
    {
        Color color = spriteRenderer.color;
        color.a = value / 255;
        spriteRenderer.color = color;
    }

    /// <summary>
    /// Set alpha of graphic
    /// </summary>
    /// <param name="graphic">Target renderer</param>
    /// <param name="value">Alpha value, from 0 to 255</param>
    public static void SetAlpha(this Graphic graphic, float value)
    {
        Color color = graphic.color;
        color.a = value / 255;
        graphic.color = color;
    }

    /// <summary>
    /// Register One Button's Event Trigger
    /// </summary>
    /// <param name="button"></param>
    /// <param name="eventTriggerType"></param>
    /// <param name="action"></param>
    public static void RegisterEventTrigger(this Button button, EventTriggerType eventTriggerType, System.Action action)
    {
        EventTrigger eventTrigger = button.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry entry = new();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener((baseEventData) =>
        {
            action();
        });
        eventTrigger.triggers.Add(entry);
    }

    public static TweenerCore<Color, Color, ColorOptions> FadeIn(this Graphic graphic, float duration)
    {
        return graphic.DOFade(1, duration);
    }

    public static TweenerCore<float, float, FloatOptions> FadeIn(this GameObject gameObject, float duration)
    {
        CanvasGroup canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        return canvasGroup.DOFade(1, duration);
    }

    public static TweenerCore<Color, Color, ColorOptions> FadeOut(this Graphic graphic, float duration)
    {
        return graphic.DOFade(0, duration);
    }

    public static TweenerCore<float, float, FloatOptions> FadeOut(this GameObject gameObject, float duration)
    {
        CanvasGroup canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        return canvasGroup.DOFade(0, duration);
    }

    /// <summary>
    /// Fade alpha between 0 and 255
    /// </summary>
    public static TweenerCore<Color, Color, ColorOptions> FadeTo(this Graphic graphic, float to, float duration)
    {
        return graphic.DOFade(to / 255, duration);
    }

    /// <summary>
    /// Fade alpha between 0 and 255
    /// </summary>
    public static TweenerCore<float, float, FloatOptions> FadeTo(this GameObject gameObject, float to, float duration)
    {
        CanvasGroup canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        return canvasGroup.DOFade(to / 255, duration);
    }

    /// <summary>
    /// Fade alpha between 0 and 255
    /// </summary>
    public static Tween CountDown(long unixtime, Action<double> onUpdate = null, Action onComplete = null)
    {
        float leftSeconds = (float)GetLeftTime(unixtime).TotalSeconds;
        return DOVirtual.Float(leftSeconds, 0, leftSeconds, (val) =>
        {
            onUpdate?.Invoke(obj: (double)val);
        }).SetEase(Ease.Linear).OnComplete(() => { onUpdate?.Invoke(0); onComplete?.Invoke(); });
    }

    public static DateTime UnixTimeToDateTime(long unixtime)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixtime).ToLocalTime();
        return dateTime;
    }

    public static long DateTimeToUnix(DateTime MyDateTime)
    {
        TimeSpan timeSpan = MyDateTime - new DateTime(1970, 1, 1, 0, 0, 0);

        return (long)timeSpan.TotalSeconds;
    }

    public static TimeSpan GetLeftTime(long unixtime)
    {
        DateTime endTime = UnixTimeToDateTime(unixtime);
        DateTime startTime = DateTime.Now;

        TimeSpan elapsed = endTime.Subtract(startTime);
        return elapsed;
    }

    public static TimeSpan GetLeftTime(long startTime, long endTime)
    {
        TimeSpan elapsed = UnixTimeToDateTime(endTime).Subtract(UnixTimeToDateTime(startTime));
        return elapsed;
    }

    public static string GetReadableLeftTime(long unixtime)
    {
        DateTime endTime = UnixTimeToDateTime(unixtime);
        DateTime startTime = DateTime.Now;

        TimeSpan elapsed = endTime.Subtract(startTime);
        return elapsed.ToReadableString();
    }

    public static string ToReadableString(this double seconds)
    {
        TimeSpan span = TimeSpan.FromSeconds(seconds);
        return ToReadableString(span);
    }
    public static string ToReadableString(this TimeSpan span)
    {
        string formatted = string.Format("{0}{1}{2}{3}",
            span.Duration().TotalDays >= 1 ? string.Format("{0:0}d ", span.Days) : string.Empty,
            span.Duration().TotalHours >= 1 ? string.Format("{0:0}h ", span.Duration().TotalDays < 1 || span.Hours >= 10 ? span.Hours.ToString() : "0" + span.Hours) : string.Empty,
            span.Duration().TotalMinutes >= 1 ? string.Format("{0:0}m ", span.Duration().TotalHours < 1 || span.Minutes >= 10 ? span.Minutes.ToString() : "0" + span.Minutes) : string.Empty,
            span.Duration().TotalSeconds >= 1 ? string.Format("{0:0}s", span.Duration().TotalMinutes < 1 || span.Seconds >= 10 ? span.Seconds.ToString() : "0" + span.Seconds) : "0s");

        return formatted;
    }

    public static bool IsPointerOnTarget(int targetSorting)
    {
        if (Input.touchCount == 0) return false;
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Count > 0)
        {
            return results[0].sortingOrder < targetSorting;
        }
        return true;
    }

    public static bool IsPointerOnLayer(int targetLayer)
    {
        if (Input.touchCount == 0) return false;
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Count > 0)
        {
            return results[0].sortingLayer == targetLayer;
        }
        return true;
    }
    public static void TAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }
    }
}
