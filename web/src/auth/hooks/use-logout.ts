import { redirectToLogout } from "../utilities";
import { useToken } from "./use-token";

/** Clears the token from cookies and state. Also redirects the user to accounts login page. */
const useLogout = () => {
  const { getToken, clearToken } = useToken();

  const logout = async () => {
    const token = await getToken();

    clearToken();
    redirectToLogout(token.value);
  };

  return logout;
};

export { useLogout };
