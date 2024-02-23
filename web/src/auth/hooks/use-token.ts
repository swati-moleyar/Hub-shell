import { useContext, useCallback } from "react";
import { TokenByRefreshTokenEndpoint } from "../../constants";
import { AuthContext } from "../auth-context";
import { Token } from "../types";
import axios from "axios";

const TOKEN = "token";
const OAUTH_TOKEN = "oauthToken";

const storeToken = (token: Token) => {
  localStorage.setItem(TOKEN, JSON.stringify(token));

  // Backwards compatibility for old Hub apps
  localStorage.setItem(OAUTH_TOKEN, token.value);
  sessionStorage.setItem(OAUTH_TOKEN, token.value);
};

const useToken = () => {
  const { dispatch } = useContext(AuthContext);

  const setToken = useCallback(
    (token: Token) => {
      storeToken(token);

      dispatch({ type: "login" });
    },
    [dispatch]
  );

  const clearToken = useCallback(() => {
    localStorage.removeItem(TOKEN);

    // Backwards compatibility for old Hub apps
    localStorage.removeItem(OAUTH_TOKEN);
    sessionStorage.removeItem(OAUTH_TOKEN);

    dispatch({ type: "logout" });
  }, [dispatch]);

  const getToken = useCallback(async (): Promise<Token> => {
    // Try to grab a token from localStorage
    const serializedToken = localStorage.getItem(TOKEN);
    if (!serializedToken) {
      return Promise.reject(new Error("No token found in localStorage"));
    }

    // Try to deserialize the token from localStorage
    let token = null;
    try {
      token = JSON.parse(serializedToken) as Token;
    } catch {
      return Promise.reject(new Error("Unable to parse serialized token from localStorage"));
    }

    // If the token hasn't expired, return it
    if (new Date(token.expiresUtc).getTime() > Date.now()) {
      // re-store the token - will put it back into session storage for backwards compatability
      storeToken(token);
      return Promise.resolve(token);
    }

    // If the token has expired, try to refresh it
    try {
      const response = await axios.post(TokenByRefreshTokenEndpoint(), { refreshToken: token.refreshToken });
      const refreshedToken = response.data as Token;
      setToken(refreshedToken);
      return Promise.resolve(refreshedToken);
    } catch {
      clearToken();
      return Promise.reject(new Error("Unable to get token from refresh token"));
    }
  }, [setToken, clearToken]);

  return {
    getToken,
    setToken,
    clearToken,
  };
};

export { useToken };
