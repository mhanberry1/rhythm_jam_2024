import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import "./index.scss";
import Button from "./button";

export default function Hud(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="hud">
      <view className="flex-row padding-md">
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
