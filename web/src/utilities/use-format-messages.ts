import { useIntl } from "react-intl";
import { Messages } from "./i18n";

type MessageFormatPrimitiveValue = string | number | boolean | null | undefined;

export function useFormatMessage(id: keyof Messages, values?: Record<string, MessageFormatPrimitiveValue>) {
  const intl = useIntl();
  return intl.formatMessage({ id }, values);
}
