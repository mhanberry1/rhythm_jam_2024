import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import "./index.scss";
import Button from "./button";
import Score from "./score";

export default function Hud(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="hud">
      <view className="flex-row padding-md">
        <Score value={globals.score.Value} />
        <view className="spacer" />
        <Button
          text="Pause"
          onClick={() => {
            gameLifecycleManager.PauseGame();
          }}
        />
      </view>
    </view>
  );
}
