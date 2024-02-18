using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldControl : MonoBehaviour
{
    [SerializeField] private GameObject Shield;
    [SerializeField] private InputActionAsset InputActions;
    [SerializeField] private float Duration = 0.2f;

    private float _remainingDuration = 0;

    private void OnEnable()
    {
        InputActions.FindActionMap("gameplay").Enable();
        InputActions.FindActionMap("gameplay").FindAction("beatInput").performed += OnBeatInput;
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
        _remainingDuration = Duration;
        Shield.SetActive(true);
    }
}
