using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Class responsible for placing explosion bombs on mouse click in the looping explosions demo.
    /// </summary>
    public class ExplosionBombPlacer : MonoBehaviour
    {
        [SerializeField] private BeatSequencer BeatSequencer;
        [SerializeField] private ExplosionBomb ExplosionBombPrefab;

        // Caching the camera for performance.
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBomb();
            }
        }

        private void PlaceBomb()
        {
            // Nothing fancy here, just standard spawning objects at the mouse position.
            var mousePos = Input.mousePosition;
            mousePos.z = 10;
            var worldPos = _camera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            var bomb = Instantiate(ExplosionBombPrefab, worldPos, Quaternion.identity);

            // Since we're instantiating something to the scene, it won't have the reference to the beat sequencer, so we need to set it manually.
            bomb.Init(BeatSequencer);
        }
    }
}
