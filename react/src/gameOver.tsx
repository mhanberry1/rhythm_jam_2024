import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import Button from "./button";
import Score from "./score";
import "./index.scss";

export default function GameOver(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;
  const status = useReactiveValue(globals.status);
  const canContinue = useReactiveValue(globals.canContinue);

  return (
    <view className="gameover">
      <view className="title">{status}</view>
      <view className="content">
        {true && <Button
          text="Main Menu"
          onClick={() => {
            gameLifecycleManager.ReturnToMainMenu();
          }}
        />}
        {false && <Button
          text="Continue"
          onClick={() => {
            gameLifecycleManager.ToNextLevel();
          }}
        />}
        <view className="score flex-row padding-md">
          <Score value={globals.score.Value} />
        </view>
      </view>
    </view>
  );
}
