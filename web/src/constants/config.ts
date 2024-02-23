export type Environment = "dev" | "int" | "rc" | "prod";

interface IConfig {
  environment: Environment;
  googleAnalyticsId: string;
  googleAnalyticsSidebarDimension: string;
  authUrl: string;
  hubShellBffUrl: string;
  sentryDsn: string;
}

const defaultConfig: IConfig = {
  environment: "dev",
  googleAnalyticsId: "UA-53263868-10",
  googleAnalyticsSidebarDimension: "dimension1",
  authUrl: "https://accountsint.iqmetrix.net",
  hubShellBffUrl: "https://hubshellbff-dev.azure.development.k8s.iqmetrix.net",
  sentryDsn: "https://31b30bcb656c4802a7d85cdadd6629b5@o163465.ingest.sentry.io/5601913",
};

const Config: IConfig = {
  ...defaultConfig,
  ...(window as any).HUBSHELL_CONFIG,
};

export { Config };
