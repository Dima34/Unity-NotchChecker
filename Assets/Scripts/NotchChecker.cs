using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotchChecker : MonoBehaviour
{
    [SerializeField] private RectTransform _blockToMoveRect;
    [SerializeField] private float _marginToMove;
    
    private ScreenOrientation _previousOrientation;
    private float _initialScreenWidth;
    private float _initialBlockToMoveXPosition;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        ApplyMargin();
    }

    private void Init()
    {
        _previousOrientation = Screen.orientation;
        _initialScreenWidth = Screen.width;
        _initialBlockToMoveXPosition = _blockToMoveRect.anchoredPosition.x;
    }
    
    private void CheckScreen()
    {
        if (Screen.orientation != _previousOrientation && _initialScreenWidth != Screen.safeArea.width)
        {
            ApplyMargin();
        } 
    }

    private void ApplyMargin()
    {
#if UNITY_EDITOR
        return;
#endif
        bool isDeviceHaveNotch = Screen.width != Screen.safeArea.width; 
        if(!isDeviceHaveNotch)
            return;

        bool isAppleDevice = Application.platform == RuntimePlatform.IPhonePlayer;
        var currentOrientation = Screen.orientation;

        Debug.Log("isAppleDevice = " + isAppleDevice);
        
        if (currentOrientation == ScreenOrientation.LandscapeLeft && !isAppleDevice)
        {
            MoveBlock();
        }
        else if(currentOrientation == ScreenOrientation.LandscapeRight && isAppleDevice)
        {
            MoveBlock();
        }
        else
        {
            SetDefaultPostion();
        }

        Debug.Log($"Block x position - {_blockToMoveRect.anchoredPosition.x}");

        _previousOrientation = currentOrientation;

        void MoveBlock()
        {
            _blockToMoveRect.anchoredPosition = new Vector2(_initialBlockToMoveXPosition + _marginToMove,
                _blockToMoveRect.anchoredPosition.y);
        }

        void SetDefaultPostion()
        {
            _blockToMoveRect.anchoredPosition = new Vector2(_initialBlockToMoveXPosition,
                _blockToMoveRect.anchoredPosition.y);
        }
    }
    
    private void FixedUpdate()
    {
        CheckScreen();
    }
}
