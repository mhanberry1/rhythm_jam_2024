using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class ShootBoba : MonoBehaviour
{
    [SerializeField] private GameObject Boba1;
    [SerializeField] private GameObject Boba2;
    [SerializeField] private GameObject BobaBallPrefab;

    void OnEnable()
    {
        CallResponseGameplayManager.Instance.OnCallNote += OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    void OnDisable()
    {
        CallResponseGameplayManager.Instance.OnCallNote -= OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote -= OnResponseNote;
    }

    void OnCallNote()
    {
        var currentShooter = GetCurrentShooter();
        Instantiate(BobaBallPrefab, currentShooter.transform.position, Quaternion.identity).SetActive(true);
    }

    void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement)
    {
    }

    GameObject GetCurrentShooter() {
        var time = CallResponseGameplayManager.Instance.RhythmEngine.GetCurrentAudioTime();
        var song = CallResponseGameplayManager.Instance.CurrentSong;
        var measureNum = song.TimeToBarNum(time);
        // 2 measures one boba, 2 measures another boba
        if ((measureNum / 2) % 2 == 0) {
            return Boba1; 
        } else {
            return Boba2;
        }
    }
}

}
