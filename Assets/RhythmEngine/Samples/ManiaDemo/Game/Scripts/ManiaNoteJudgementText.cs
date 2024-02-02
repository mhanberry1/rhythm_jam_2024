using UnityEngine;
using UnityEngine.UI;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// This class handles displaying the judgement of the last note hit.
    /// </summary>
    public class ManiaNoteJudgementText : MonoBehaviour
    {
        [SerializeField] private ManiaGameplayManager GameplayManager;

        [Space]
        [SerializeField] private Vector3 JudgementScale = new Vector3(1.5f, 1.5f, 1f);
        [SerializeField] private float LerpSpeed = 5f;

        private Text _text;
        private Vector3 _defaultScale;

        private void Start()
        {
            _text = GetComponent<Text>();
            _defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            GameplayManager.OnJudgement += OnJudgement;
        }

        private void OnDisable()
        {
            GameplayManager.OnJudgement -= OnJudgement;
        }

        private void OnJudgement(ManiaGameplayManager.Judgement judgement)
        {
            transform.localScale = JudgementScale;
            _text.text = $"Last Note:\n{judgement}";
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _defaultScale, Time.deltaTime * LerpSpeed);
        }
    }
}
