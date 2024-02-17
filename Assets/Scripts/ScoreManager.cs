using System;
using UnityEngine;

namespace RhythmJam
{

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int GoodScore = 50;
    [SerializeField] private int PerfectScore = 100;
    [SerializeField] private int MissScore = 0;
    [SerializeField] private int NextStreak = 10;
    [SerializeField] private int MaxStreak = 4;
    [SerializeField] private string BadStatus = "Try harder...";
    [SerializeField] private string GoodStatus = "Not too shabby.";
    [SerializeField] private string GreatStatus = "Amazing!";

    private CallResponseGameplayManager.Judgement Good = CallResponseGameplayManager.Judgement.Good;
    private CallResponseGameplayManager.Judgement Perfect = CallResponseGameplayManager.Judgement.Perfect;
    private CallResponseGameplayManager.Judgement Miss = CallResponseGameplayManager.Judgement.Miss;

    private int _score = 0;
    private int _multiplier = 1;
    private int _currentStreak = 0;

    void OnEnable()
    {
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
        GameLifecycleManager.Instance.OnGameStateUpdated += OnGameStateUpdated;
    }

    void OnDisable()
    {
        CallResponseGameplayManager.Instance.OnResponseNote -= OnResponseNote;
        GameLifecycleManager.Instance.OnGameStateUpdated -= OnGameStateUpdated;
    }

    void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement)
    {
        _currentStreak = judgement == Miss ? 0 : _currentStreak + 1;
        _multiplier = 1 + Math.Min(_currentStreak % NextStreak, MaxStreak);

        _score += judgement == Good ? GoodScore * _multiplier
                : judgement == Perfect ? PerfectScore * _multiplier
                : MissScore * _multiplier;

        GameLifecycleManager.Instance.SetScore(_score);
    }

    // If the game is over, submit score to leaderboard
    void OnGameStateUpdated(object sender, GameLifecycleManager.GameState gameState)
    {
        if (gameState != GameLifecycleManager.GameState.GameOver) return;

        CallResponseSong currentSong = CallResponseGameplayManager.Instance.CurrentSong;
        int totalPossibleScore = currentSong.ResponseNotes.Count * PerfectScore;
        float scorePercentage = _score / totalPossibleScore;
        string status = scorePercentage > .9 ? GreatStatus
            : scorePercentage > .7 ? GoodStatus
            : BadStatus;

        Leaderboards.Instance.SubmitScoreToLeaderboard(_score);
        GameLifecycleManager.Instance.SetStatus(status);
    }
}

}
