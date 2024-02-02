using UnityEngine;

namespace RhythmEngine.Examples
{
    public class SampleAudioVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform LinesParent;
        [SerializeField] private AudioBandListener AudioBandListener;
        [SerializeField] private float LineHeight = 2;

        private RectTransform[] _lines;

        private void Start()
        {
            _lines = new RectTransform[LinesParent.childCount];
            for (int i = 0; i < LinesParent.childCount; i++)
            {
                _lines[i] = LinesParent.GetChild(i).GetChild(0).GetComponent<RectTransform>();
                _lines[i].localScale = new Vector3(1, 0, 1);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                var yScale = AudioBandListener.Band(i, true) * LineHeight;
                _lines[i].localScale = new Vector3(1, yScale, 1);
            }
        }
    }
}
