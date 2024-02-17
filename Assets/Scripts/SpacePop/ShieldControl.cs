using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldControl : MonoBehaviour
{
    [SerializeField] private GameObject Shield;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float duration = 0.2f;

    private float _remainingDuration = 0;

    private void OnEnable()
    {
        inputActions.FindActionMap("gameplay").Enable();
        inputActions.FindActionMap("gameplay").FindAction("beatInput").performed += OnBeatInput;
    }

    private void Update()
    {
        if (_remainingDuration <= 0) return;

        _remainingDuration -= Time.deltaTime;

        if (_remainingDuration <= 0 )
        {
            Shield.SetActive(false);
        }
    }

    private void OnBeatInput(InputAction.CallbackContext context)
    {
        Debug.Log("here");
        _remainingDuration = duration;
        Shield.SetActive(true);
    }
}
