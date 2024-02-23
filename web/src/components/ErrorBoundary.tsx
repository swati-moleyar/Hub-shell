import { Result } from "@iqmetrix/antd";
import React from "react";
import * as Sentry from "@sentry/react";

const genericErrorMessage = (
  <Result
    title="There's a problem loading this page"
    subTitle={
      <>
        There’s a technical problem with the app that has prevented this page from loading. Unsaved information may be
        lost. Try reloading this page or going to another page in the app. If that doesn’t work, contact
        <a href="mailto:support@iqmetrix.com">support@iqmetrix.com</a> for updates and try again later.
      </>
    }
  />
);

export const ErrorBoundary: React.FC = (props) => {
  return <Sentry.ErrorBoundary fallback={genericErrorMessage}>{props.children}</Sentry.ErrorBoundary>;
};
