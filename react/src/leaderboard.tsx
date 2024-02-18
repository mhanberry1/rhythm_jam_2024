import { useGlobals, useReactiveValue } from "@reactunity/renderer";
import "./index.scss";

import Button from "./button";

interface LeaderboardScores {
  LatestScore: number;
  LatestRanking: number;
  BestScore: number;
  BestRanking: number;
  GlobalBestScore: number;
}

export default function Leaderboard() {
  const globals = useGlobals();
  const gameLifecycleManager = globals.gameLifecycleManager;
  const leaderboardScores = useReactiveValue(
    globals.leaderboardScores
  ) as LeaderboardScores;
  const {
    LatestScore,
    LatestRanking,
    BestScore,
    BestRanking,
    GlobalBestScore,
  } = leaderboardScores;

  return (
    <view className="leaderboard">
      <view className="title">Leaderboard</view>
      <view className="content">
        <view className="score">{`My Best Score:  ${BestScore}`}</view>
        <view className="score">{`My Best Ranking:  #${BestRanking}`}</view>
        <view className="score">{`World Top Score:  ${GlobalBestScore}`}</view>
      </view>
      <Button
        text="Main Menu"
        onClick={() => {
          gameLifecycleManager.ReturnToMainMenu();
        }}
      />
    </view>
  );
}
