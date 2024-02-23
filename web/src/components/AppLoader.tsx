import React, { useCallback, useEffect, useState } from "react";
import { Result, Grid } from "@iqmetrix/antd";
import styled from "styled-components";
import { Header, HeaderHeight } from "../navigation";
import { AppFrame, AppFrameProps } from "./AppFrame";
import { getHubAppUrl } from "../constants";
import { useSession } from "../auth";
import { Sidebar } from "../navigation/sidebar/Sidebar";
import * as Sentry from "@sentry/react";
import ReactGA from "react-ga";
const { useBreakpoint } = Grid;

const Content = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
`;

const MainContainer = styled.div`
  display: flex;
  flex-direction: row;
  height: calc(100% - ${HeaderHeight});
`;

const FAILED_PING_MESSAGE = [
  "%c**************************************************\n" +
    " Don't worry about the failed ping\n It's expected when loading Hub v1 or React apps\n" +
    "**************************************************",
  "font-weight: bold",
];

/*
 * Gets the current app name from the hash.
 */
const getAppNameFromHash = () => {
  const appName = window.location.hash.substr(1).replace(/\/.*/, "");

  return appName === "" ? null : appName;
};

/*
 * Returns the relative route within an app:
 *  "#endlessaisle/configurations" would return "configurations"
 */
const getAppRoute = () => {
  const route = window.location.hash.substr(1);
  const index = route.indexOf("/");
  return index >= 0 ? route.substr(index + 1) : "";
};

/*
 * This is triggered by navigation within an app.
 * We need to update the Shell's hash to match the app's.
 */
const onRouteChanged = (newRoute: string) => {
  const appName = getAppNameFromHash();

  if (newRoute.charAt(0) === "/") {
    newRoute = newRoute.substr(1);
  }

  ReactGA.pageview(`/${appName}/${newRoute}`);
  window.history.replaceState(null, "", `#${appName}/${newRoute}`);
};

type AppType = AppFrameProps | "NotFound";

const AppLoader: React.FunctionComponent = () => {
  const [app, setApp] = useState<AppType>();
  const [sidebarOpen, setSidebarOpen] = useState<boolean>(true);
  const { session } = useSession();
  const { xs, md } = useBreakpoint();

  /*
   * This is triggered by navigation within the shell.
   * We need to load the corresponding app.
   */
  const onHashChanged = useCallback((ev: HashChangeEvent) => {
    loadApp();
  }, []);

  useEffect(() => {
    window.addEventListener("hashchange", onHashChanged);

    loadApp();

    return () => {
      window.removeEventListener("hashchange", onHashChanged);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (xs) setSidebarOpen(false);
    else if (md) setSidebarOpen(true);
  }, [xs, md]);

  /*
   * Loads an app by hitting both v1 and v2 ping endpoints to determine what the iframe URL should be.
   */
  const loadApp = async () => {
    const appName = getAppNameFromHash() ?? "Home";
    const v1Url = getHubAppUrl(1, appName);
    const v2Url = getHubAppUrl(2, appName);
    const appRoute = getAppRoute();
    const pingResponses = await Promise.all([fetch(`${v1Url}/ping`), fetch(`${v2Url}/ping`)]);

    let appNotFound = false;
    if (pingResponses[1].ok) {
      // v2 is successful
      setApp({ appUrl: v2Url, appRoute, onRouteChanged });
    } else if (pingResponses[0].ok) {
      // v1/react ping is successful
      setApp({ appUrl: v1Url, appRoute, onRouteChanged });
    } else {
      setApp("NotFound");
      appNotFound = true;
    }

    if (appNotFound) {
      ReactGA.event({ category: "AppNotFound", action: appName });
    } else {
      Sentry.setTag("loaded.app", appName);
      ReactGA.pageview(`/${appName}/${appRoute}`);
    }

    // if any of the pings were unsuccessful, show nice message in console.
    if (pingResponses.find((x) => !x.ok)) {
      console.log(...FAILED_PING_MESSAGE);
    }
  };

  if (!app) {
    return null;
  }

  return (
    <Content>
      <Header menuButtonActive={sidebarOpen} onMenuButtonClick={() => setSidebarOpen((prev) => !prev)} />
      <MainContainer>
        {session?.applicationGroups && <Sidebar open={sidebarOpen} applicationGroups={session.applicationGroups} />}
        {app === "NotFound" ? <Result statusCode="404" /> : <AppFrame {...app} />}
      </MainContainer>
    </Content>
  );
};

export { AppLoader };
