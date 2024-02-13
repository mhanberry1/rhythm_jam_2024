import { useGlobals, useReactiveValue } from "@reactunity/renderer";

import "./index.scss";

export default function Debug(): React.ReactNode {
  const globals = useGlobals();
  const route = useReactiveValue(globals.route);
  const gameState = useReactiveValue(globals.debugGameState);

  return (
    <view className="debug-ui">
      <view className="spacer" />
      <view className="footer">
        <view className="flex-column">
          <view className="text">{`UI Route: ${route}`}</view>
          <view className="text">{`Game State: ${gameState}`}</view>
        </view>
      </view>
    </view>
  );
}
