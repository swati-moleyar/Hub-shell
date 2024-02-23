import React, { FC, useCallback, useEffect, useRef } from "react";
import styled from "styled-components";

interface AppFrameProps {
  title?: string;
  appUrl: string;
  appRoute: string;
  onRouteChanged(newRoute: string): void;
}

const StyledIFrame = styled.iframe`
  flex-grow: 1;
  border: 0;
`;

const AppFrame: FC<AppFrameProps> = ({ title = "Loading", appUrl, appRoute, onRouteChanged }) => {
  const frameRef = useRef<HTMLIFrameElement>(null);

  const onHashChange = useCallback(
    (ev: HashChangeEvent) => {
      onRouteChanged(ev.newURL.split("#")[1]);
    },
    [onRouteChanged]
  );

  const onLoad = () => {
    frameRef.current?.contentWindow?.addEventListener("hashchange", onHashChange);
  };

  useEffect(() => {
    return frameRef.current?.contentWindow?.removeEventListener("hashchange", onHashChange);
  }, [onHashChange]);

  const src = `${appUrl}/#${appRoute}`;
  return <StyledIFrame ref={frameRef} title={title} src={src} onLoad={onLoad} />;
};

export { AppFrame };
export type { AppFrameProps };
