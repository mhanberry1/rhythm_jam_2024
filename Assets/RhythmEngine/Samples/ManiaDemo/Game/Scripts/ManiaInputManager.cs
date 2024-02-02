using System;
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// This class is responsible for handling input for the Mania demo.
    /// For a real game you should use a custom input handling system or the new unity input system (I really like the latter).
    ///
    /// It also handles the visual feedback for the keys.
    /// </summary>
    public class ManiaInputManager : MonoBehaviour
    {
        [SerializeField] private Transform[] Keys = new Transform[4];

        [Space]
        [SerializeField] private Vector3 KeyPressScale = new Vector3(1.2f, 1.2f, 1f);
        [SerializeField] private float KeyDepressLerpSpeed = 5f;

        private Vector3 _keyDefaultScale;

        // This event is used to send input to the gameplay manager (or any other systems that require key input)
        public event Action<int> OnKeyPressed;

        private void Start()
        {
            _keyDefaultScale = Keys[0].localScale;
        }

        private void Update()
        {
            // This is really simple for the demo, ideally you'd want to use an input system's event system and subscribe to the specific key events.
            // Handling input in Update means that input time can only be accurate to the frame rate, which is not ideal.

            if (Input.GetKeyDown(KeyCode.D))
            {
                PressKey(0);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                PressKey(1);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                PressKey(2);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                PressKey(3);
            }

            foreach (var key in Keys)
            {
                key.transform.localScale = Vector3.Lerp(key.transform.localScale, _keyDefaultScale, Time.deltaTime * KeyDepressLerpSpeed);
            }
        }

        private void PressKey(int key)
        {
            Keys[key].localScale = KeyPressScale; // Visual press feedback
            OnKeyPressed?.Invoke(key);
        }
    }
}
