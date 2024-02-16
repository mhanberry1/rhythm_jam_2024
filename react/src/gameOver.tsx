import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import Button from "./button";
import Score from "./score";
import "./index.scss";

export default function GameOver(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="gameover">
      <view className="title">{globals.status.Value}</view>
      <view className="content">
        <Button
          text="Main Menu"
          onClick={() => {
            gameLifecycleManager.ReturnToMainMenu();
          }}
        />
        <view className="flex-row padding-md">
          <Score value={globals.score.Value} />
        </view>
      </view>
    </view>
  );
}
