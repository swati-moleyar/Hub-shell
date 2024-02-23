import { Config } from "./config";

export const getHubAppUrl = (hubVersion: 1 | 2, appName: string) => {
  return hubVersion === 1 ? `/${appName}` : `/apps/hub.app.${appName}`;
};

const homePageUrl = window.location.href.split("/").slice(0, 3).join("/");

export const AuthEndpoint = (redirectUri: string) =>
  `${Config.authUrl}/v1/oauth2/auth?response_type=code&client_id=iqmetrixhub&redirect_uri=${redirectUri}`;

export const LogoutEndpoint = (token: string, returnUrl: string) =>
  `${Config.authUrl}/logout?accessToken=${token}&returnUrl=${returnUrl}`;

export const TokenByCodeEndpoint = () => `${Config.hubShellBffUrl}/api/authorization/code`;

export const TokenByRefreshTokenEndpoint = () => `${Config.hubShellBffUrl}/api/authorization/refresh`;

export const SessionEndpoint = () => `${Config.hubShellBffUrl}/api/session`;

export const ChangePasswordUrl = () => `${Config.authUrl}/changepassword?returnUrl=${encodeURIComponent(homePageUrl)}`;

export const HelpUrl = () => `https://support.iqmetrix.com`;
