import React, { useEffect, useContext } from "react";
import { Spin } from "@iqmetrix/antd";
import styled from "styled-components";
import { AuthContext, useLogin, useToken } from "../auth";
import { useHttp } from "../http";
import { useAsyncError } from "../utilities";
import * as Sentry from "@sentry/react";
import ReactGA from "react-ga";

const SpinContainer = styled.div`
  display: flex;
  height: 100%;
  width: 100%;
  position: absolute;
  align-items: center;
  justify-content: center;
`;

const SessionHandler: React.FunctionComponent = (props) => {
  const { state, dispatch } = useContext(AuthContext);
  const { httpClient } = useHttp();
  const { throwErrorOnRender } = useAsyncError();
  const { getToken } = useToken();
  const { login } = useLogin();

  useEffect(() => {
    const attemptLogin = async () => {
      try {
        await getToken();
        dispatch({ type: "login" });
      } catch {
        await login();
      }
    };

    attemptLogin();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const getSession = async () => {
      try {
        const session = await httpClient.getSession();
        dispatch({ type: "set_session", session });

        Sentry.setUser({ id: session.userId.toString() });
        Sentry.setTag("entity.id", session.companyId);

        ReactGA.set({ userId: session.userId });
      } catch (e) {
        throwErrorOnRender(e);
      }
    };

    if (state.isLoggedIn && !state.session) {
      getSession();
    }
  }, [httpClient, state.session, state.isLoggedIn, dispatch, throwErrorOnRender]);

  if (!state.isLoggedIn || !state.session) {
    return (
      <SpinContainer>
        <Spin />
      </SpinContainer>
    );
  }

  return <>{props.children}</>;
};

export { SessionHandler };
