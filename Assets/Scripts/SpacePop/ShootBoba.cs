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

    private GameObject _currentShooter;
    private bool _newMeasure = false;

    void OnEnable()
    {
        CallResponseGameplayManager.Instance.OnCallNote += OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;

        _currentShooter = Boba1;
    }

    void OnDisable()
    {
        CallResponseGameplayManager.Instance.OnCallNote -= OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote -= OnResponseNote;
    }

    void OnCallNote()
    {
        Instantiate(BobaBallPrefab, _currentShooter.transform.position, Quaternion.identity).SetActive(true);
        _newMeasure = true;
    }

    void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement)
    {
        if (!_newMeasure) return;

        _currentShooter = _currentShooter == Boba1 ? Boba2 : Boba1;
        _newMeasure = false;
    }
}

}
