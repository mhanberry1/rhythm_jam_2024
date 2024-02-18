import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import Button from "./button";
import "./index.scss";

export default function MainMenu(): React.ReactNode {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;

  return (
    <view className="main-menu">
      <view className="title">Rhythm Jam 2024</view>
      <view className="content">
        <Button
          text="Space Pop"
          onClick={() => {
            gameLifecycleManager.StartGame(0);
          }}
        />
        <Button
          text="Old Man Rave"
          onClick={() => {
            gameLifecycleManager.StartGame(1);
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
