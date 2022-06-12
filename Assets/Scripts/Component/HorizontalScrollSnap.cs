using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
[AddComponentMenu("Layout/Extensions/Horizontal Scroll Snap")]
public class HorizontalScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Action<int, int, int> OnScrollDrag = delegate { };
    public Action<int> OnChangePage = delegate { };

    private Transform _screensContainer;

    private int _screens = 1;
    private int _startingScreen = 1;

    private bool _fastSwipeTimer = false;
    private int _fastSwipeCounter = 0;
    private int _fastSwipeTarget = 30;


    private System.Collections.Generic.List<Vector3> _positions;
    private ScrollRect _scroll_rect;
    private Vector3 _lerp_target;
    private bool _lerp;

    private int _containerSize;

    [Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
    public GameObject Pagination;

    public Boolean UseFastSwipe = true;
    public int FastSwipeThreshold = 100;

    private bool _startDrag = true;
    private Vector3 _startPosition = new Vector3();
    private int _currentScreen;

    private float distance;
    private int percent, lastPercent;
    private int target;
    private int currentBulletToggle = -1;

    // Use this for initialization
    void Start()
    {
        _scroll_rect = gameObject.GetComponent<ScrollRect>();
        _screensContainer = _scroll_rect.content;
        //DistributePages();

        _screens = _screensContainer.childCount;

        _lerp = false;

        _positions = new System.Collections.Generic.List<Vector3>();

        if (_screens > 0)
        {
            for (int i = 0; i < _screens; ++i)
            {
                _scroll_rect.horizontalNormalizedPosition = (float)i / (float)(_screens - 1);
                _positions.Add(_screensContainer.localPosition);
            }
        }

        _scroll_rect.horizontalNormalizedPosition = (float)(_startingScreen - 1) / (float)(_screens - 1);

        _containerSize = (int)_screensContainer.gameObject.GetComponent<RectTransform>().offsetMax.x;

        ChangeBulletsInfo(CurrentScreen());

        if (_positions.Count > 1)
        {
            distance = Vector3.Distance(_positions[0], _positions[1]);
        }
    }
    void Update()
    {
        if (_lerp)
        {
            _screensContainer.localPosition = Vector3.Lerp(_screensContainer.localPosition, _lerp_target, 7.5f * Time.deltaTime);
            if (Vector3.Distance(_screensContainer.localPosition, _lerp_target) < 0.001f)
            {
                _lerp = false;
                percent = percent > 50 ? 100 : 0;
                OnScrollDrag(_currentScreen, target, percent);
            }

            //change the info bullets at the bottom of the screen. Just for visual effect
            if (Vector3.Distance(_screensContainer.localPosition, _lerp_target) < 10f)
            {
                ChangeBulletsInfo(CurrentScreen());
            }
        }

        if (_fastSwipeTimer)
        {
            _fastSwipeCounter++;
        }

        if (!_startDrag || _lerp)
        {
            if (_startPosition.x != _screensContainer.localPosition.x)
            {
                percent = Mathf.RoundToInt(100 * Mathf.Abs(Vector3.Distance(_screensContainer.localPosition, _startPosition) - distance) / distance);
                if (lastPercent != percent)
                {
                    lastPercent = percent;
                    target = _currentScreen + (_startPosition.x > _screensContainer.localPosition.x ? 1 : -1);
                    target = target < 0 ? 0 : target >= _positions.Count ? _positions.Count - 1 : target;
                    OnScrollDrag(_currentScreen, target, percent);
                }
            }
        }
    }

    private bool fastSwipe = false; //to determine if a fast swipe was performed


    //Function for switching screens with buttons
    public void NextScreen()
    {
        int currScreen = CurrentScreen();
        Debug.LogWarning("currScreen " + currScreen);
        if (currScreen < _screens - 1)
        {
            _lerp = true;
            _lerp_target = _positions[currScreen + 1];

            ChangeBulletsInfo(currScreen + 1);
        }
    }

    //Function for switching screens with buttons
    public void PreviousScreen()
    {
        int currScreen = CurrentScreen();
        if (CurrentScreen() > 0)
        {
            _lerp = true;
            _lerp_target = _positions[currScreen - 1];

            ChangeBulletsInfo(currScreen - 1);
        }
    }

    //Because the CurrentScreen function is not so reliable, these are the functions used for swipes
    private void NextScreenCommand()
    {
        if (_currentScreen < _screens - 1)
        {
            _lerp = true;
            _lerp_target = _positions[_currentScreen + 1];

            ChangeBulletsInfo(_currentScreen + 1);
        }
    }

    //Because the CurrentScreen function is not so reliable, these are the functions used for swipes
    private void PrevScreenCommand()
    {
        if (_currentScreen > 0)
        {
            _lerp = true;
            _lerp_target = _positions[_currentScreen - 1];

            ChangeBulletsInfo(_currentScreen - 1);
        }
    }


    //find the closest registered point to the releasing point
    private Vector3 FindClosestFrom(Vector3 start, System.Collections.Generic.List<Vector3> positions)
    {
        Vector3 closest = Vector3.zero;
        float distance = Mathf.Infinity;

        foreach (Vector3 position in _positions)
        {
            if (Vector3.Distance(start, position) < distance)
            {
                distance = Vector3.Distance(start, position);
                closest = position;
            }
        }

        return closest;
    }


    //returns the current screen that the is seeing
    public int CurrentScreen()
    {
        float absPoz = Math.Abs(_screensContainer.gameObject.GetComponent<RectTransform>().offsetMin.x);

        absPoz = Mathf.Clamp(absPoz, 1, _containerSize - 1);

        float calc = (absPoz / _containerSize) * _screens;

        return Mathf.RoundToInt(calc);
    }

    //returns the total screen
    public int ScreenCount()
    {
        return _positions.Count;
    }

    //changes the bullets on the bottom of the page - pagination
    private void ChangeBulletsInfo(int currentScreen)
    {
        if (!Pagination) return;
        var childCount = Pagination.transform.childCount;
        currentScreen = currentScreen < 0 ? 0 : currentScreen > childCount ? childCount : currentScreen;
        if (currentBulletToggle != currentScreen)
        {
            currentBulletToggle = currentScreen;
            OnChangePage(currentBulletToggle);
        }
        for (int i = 0; i < childCount; i++)
        {
            Pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (currentScreen == i);
        }
    }

    //used for changing between screen resolutions
    private void DistributePages()
    {
        int _offset = 0;
        int _step = Screen.width;
        int _dimension = 0;

        int currentXPosition = 0;

        for (int i = 0; i < _screensContainer.transform.childCount; i++)
        {
            RectTransform child = _screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
            currentXPosition = _offset + i * _step;
            child.anchoredPosition = new Vector2(currentXPosition, 0f);
            child.sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().rect.width, gameObject.GetComponent<RectTransform>().rect.height);
        }

        _dimension = currentXPosition + _offset * -1;

        _screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(_dimension, 0f);
    }

    public float PercentScroll()
    {

        return 0;
    }

    #region Interfaces
    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = _screensContainer.localPosition;
        _fastSwipeCounter = 0;
        _fastSwipeTimer = true;
        _currentScreen = CurrentScreen();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _startDrag = true;
        if (_scroll_rect.horizontal)
        {
            ChangeBulletsInfo(CurrentScreen());
            if (UseFastSwipe)
            {
                fastSwipe = false;
                _fastSwipeTimer = false;
                if (_fastSwipeCounter <= _fastSwipeTarget)
                {
                    if (Math.Abs(_startPosition.x - _screensContainer.localPosition.x) > FastSwipeThreshold)
                    {
                        fastSwipe = true;
                    }
                }
                if (fastSwipe)
                {
                    if (_startPosition.x - _screensContainer.localPosition.x > 0)
                    {
                        NextScreenCommand();
                    }
                    else
                    {
                        PrevScreenCommand();
                    }
                }
                else
                {
                    _lerp = true;
                    _lerp_target = FindClosestFrom(_screensContainer.localPosition, _positions);
                }
            }
            else
            {
                _lerp = true;
                _lerp_target = FindClosestFrom(_screensContainer.localPosition, _positions);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _lerp = false;
        if (_startDrag)
        {
            OnBeginDrag(eventData);
            _startDrag = false;
        }
    }
    #endregion
}