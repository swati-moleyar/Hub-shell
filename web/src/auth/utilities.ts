import { AuthEndpoint, LogoutEndpoint } from "../constants";

const redirectToLogin = (referrer?: string) => {
  let authEndpoint = `${AuthEndpoint(window.location.origin)}`;

  if (!!referrer) {
    authEndpoint += `&state=${encodeURIComponent(referrer)}`;
  }

  window.top.location.replace(authEndpoint);
};

const redirectToLogout = (token: string | null) => {
  if (token) {
    window.top.location.replace(LogoutEndpoint(token, window.location.origin));
  } else {
    redirectToLogin();
  }
};

const getAuthCodeFromQueryString = () => {
  const params = new URLSearchParams(window.location.search);

  if (params.has("code")) {
    return params.get("code");
  }
};

const getReferrerFromQueryString = () => {
  const params = new URLSearchParams(window.location.search);

  if (params.has("state")) {
    return decodeURIComponent(params.get("state") as string);
  } else {
    return null;
  }
};

export { redirectToLogin, redirectToLogout, getAuthCodeFromQueryString, getReferrerFromQueryString };
