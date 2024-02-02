using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Class responsible for the visuals of the explosion bomb in the looping explosions demo.
    /// </summary>
    public class ExplosionBomb : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ExplosionParticles;
        [SerializeField] private float SizeLerpSpeed = 5f;

        public void Init(BeatSequencer beatSequencer)
        {
            // This will call Explode() on the next snare hit.
            // It will call only once, so we don't have to worry about subscribing and unsubscribing to the OnInstrumentStep event.
            beatSequencer.CallOnNextInstrumentStep(0, Explode);

            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            // Lerp the size up for nice visuals.
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * SizeLerpSpeed);
        }

        private void Explode()
        {
            Instantiate(ExplosionParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
