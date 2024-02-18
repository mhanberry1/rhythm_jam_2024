using UnityEngine;
using UnityEngine.InputSystem;

namespace RhythmJam
{

public class AnimateButtonPress : MonoBehaviour
{
    [SerializeField] Sprite[] Frames;
    [SerializeField] private InputActionAsset InputActions;
    [SerializeField] float FrameTime = 0.1f;

    private int _frameIndex;
    private float _frameTimeLeft;
    private bool _animate = false;

    private void OnEnable()
    {
        InputActions.FindActionMap("gameplay").Enable();
        InputActions.FindActionMap("gameplay").FindAction("beatInput").performed += OnBeatInput;
    }

    private void Update()
    {
        if (!_animate) return;

        _frameTimeLeft -= Time.deltaTime;

        if (_frameTimeLeft > 0) return;

        _frameIndex++;
        _frameTimeLeft = FrameTime;
        _animate = _frameIndex < Frames.Length;

        if (!_animate)
        {
            _frameIndex = 0;
        }

        GetComponent<SpriteRenderer>().sprite = Frames[_frameIndex];
    }

    private void OnBeatInput(InputAction.CallbackContext context)
    {
        _frameIndex = 0;
        _frameTimeLeft = FrameTime;
        _animate = true;
    }
}

}
