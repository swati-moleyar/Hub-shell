import { useCallback } from "react";
import { useToken } from "./use-token";
import { getAuthCodeFromQueryString, getReferrerFromQueryString, redirectToLogin } from "../utilities";
import { useAsyncError } from "../../utilities";
import axios from "axios";
import { TokenByCodeEndpoint } from "../../constants";
import { Token } from "../types";

const useLogin = () => {
  const { setToken } = useToken();
  const { throwErrorOnRender } = useAsyncError();

  const login = useCallback(async () => {
    const authCode = getAuthCodeFromQueryString();
    if (!authCode) {
      redirectToLogin(`${window.location.pathname}${window.location.hash}`);
    } else {
      try {
        let referrer = getReferrerFromQueryString() ?? "/";
        window.history.replaceState({}, document.title, referrer);
        const response = await axios.post(TokenByCodeEndpoint(), { code: authCode });
        const token = response.data as Token;
        setToken(token);
      } catch {
        throwErrorOnRender(new Error("Unable to get token from code"));
      }
    }
  }, [setToken, throwErrorOnRender]);

  return { login };
};

export { useLogin };
