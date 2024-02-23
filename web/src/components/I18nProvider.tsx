import React from "react";
import { IntlProvider } from "react-intl";
import { ConfigProvider } from "@iqmetrix/antd";

import en_US from "antd/es/locale/en_US";
import es_ES from "antd/es/locale/es_ES";
import fr_FR from "antd/es/locale/fr_FR";
import { Locale } from "antd/lib/locale-provider";

import { messages } from "../utilities/i18n";

function getLocale(): string {
  return navigator.language.split(/[-_]/)[0]; // language without region code
}
const getMessages = () => messages[getLocale()];

function getAntdLocale(): Locale {
  switch (getLocale()) {
    case "fr":
      return fr_FR;
    case "es":
      return es_ES;
    default:
      return en_US;
  }
}

export const I18nProvider: React.FC = (props) => (
  <ConfigProvider locale={getAntdLocale()}>
    <IntlProvider locale={getLocale()} messages={getMessages()}>
      {props?.children}
    </IntlProvider>
  </ConfigProvider>
);
