import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import Button from "./button";
import "./index.scss";

export default function MainMenu(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="main-menu">
      <view className="title">Divergent Dreams</view>
      <view className="content">
        <Button
          text="Start Game"
          onClick={() => {
            gameLifecycleManager.StartGame(0);
          }}
        />
        <Button
          text="Leaderboard"
          onClick={() => {
            gameLifecycleManager.ToLeaderboard();
          }}
        />
      </view>
    </view>
  );
}
