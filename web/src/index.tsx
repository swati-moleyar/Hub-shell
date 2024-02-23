import React from "react";
import ReactDOM from "react-dom";
import { App } from "./App";
import reportWebVitals from "./reportWebVitals";
import "./resources/styles/index.scss";
import * as Sentry from "@sentry/react";
import packageJson from "../package.json";
import { Config } from "./constants";
import ReactGA from "react-ga";

ReactGA.initialize(Config.googleAnalyticsId);
ReactGA.set({ env: Config.environment });

Sentry.init({
  environment: Config.environment,
  release: `${packageJson.name}@${packageJson.version}`,
  dsn: Config.sentryDsn,
  denyUrls: ["localhost"],
});

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
