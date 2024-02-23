import en from "./en";
import fr from "./fr";
import es from "./es";

export type Messages = typeof en;

export const messages: Record<string, Messages> = {
  en,
  fr,
  es,
};
