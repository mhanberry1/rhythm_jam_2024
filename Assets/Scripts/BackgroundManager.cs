using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    public GameObject SpacePopBackground;
    public GameObject OldManRaveBackground;
    public GameObject CutieGermsBackground;

    public void SwitchBackgrounds(GameLifecycleManager.GameType gameType)
    {
        DisableBackground();

        switch (gameType)
        {
            case GameLifecycleManager.GameType.SpacePop:
                SpacePopBackground.SetActive(true);
                break;
            case GameLifecycleManager.GameType.OldManRave:
                OldManRaveBackground.SetActive(true);
                break;
            case GameLifecycleManager.GameType.CutieGerms:
                CutieGermsBackground.SetActive(true);
                break;
        }
    }

    public void DisableBackground()
    {
        SpacePopBackground.SetActive(false);
        OldManRaveBackground.SetActive(false);
        CutieGermsBackground.SetActive(false);
    }
}
