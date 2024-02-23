import React, { useReducer } from "react";
import { AuthProvider, authReducer } from "./auth";
import { AppLoader, ErrorBoundary, I18nProvider, InteropHandlers, SessionHandler } from "./components";
import "@iqmetrix/antd/dist/accessibility.css";
import { InactivityTimer } from "./components";

const App: React.FunctionComponent = () => {
  const [authState, authDispatch] = useReducer(authReducer, {
    referrer: null,
    isLoggedIn: false,
  });

  return (
    <ErrorBoundary>
      <I18nProvider>
        <AuthProvider value={{ state: authState, dispatch: authDispatch }}>
          <SessionHandler>
            <InteropHandlers>
              <InactivityTimer>
                <AppLoader />
              </InactivityTimer>
            </InteropHandlers>
          </SessionHandler>
        </AuthProvider>
      </I18nProvider>
    </ErrorBoundary>
  );
};

export { App };
