interface Token {
  value: string;
  refreshToken: string;
  expiresUtc: string;
}

interface Session {
  companyId: number;
  companyName: string;
  userId: number;
  firstName: string;
  lastName: string;
  canChangePassword: boolean;
}

interface SessionResponse extends Session {
  brandingLogo?: string;
  parentEntityRole?: string;
  applicationGroups?: ApplicationGroup[];
}

interface ApplicationGroup {
  apps: Application[];
  defaultApp?: Application;
  icon: string;
  name: string;
}

interface Application {
  description?: string;
  href: string;
  id: string;
  name: string;
  version: number;
}

interface AuthState {
  referrer?: string | null;
  isLoggedIn?: boolean;
  session?: Session;
  brandingLogo?: string;
  applicationGroups?: ApplicationGroup[];
  parentEntity?: ParentEntity;
}

interface ParentEntity {
  id: number;
  name: string;
  role: string;
}

type AuthAction =
  | { type: "set_referrer"; referrer: string | null }
  | { type: "login" }
  | { type: "logout" }
  | { type: "set_session"; session: SessionResponse };

export type { Token, Session, AuthAction, AuthState, SessionResponse, ParentEntity, Application, ApplicationGroup };
