using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject SpacePopLevel;
    public GameObject OldManRaveLevel;

    public void SwitchLevels(GameLifecycleManager.GameType gameType)
    {
        DisableLevel();

        switch (gameType)
        {
            case GameLifecycleManager.GameType.SpacePop:
                SpacePopLevel.SetActive(true);
                break;
            case GameLifecycleManager.GameType.OldManRave:
                OldManRaveLevel.SetActive(true);
                break;
        }
    }

    public void DisableLevel()
    {
        SpacePopLevel.SetActive(false);
        OldManRaveLevel.SetActive(false);
    }
}
