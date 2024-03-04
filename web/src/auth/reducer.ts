import { Reducer } from "react";
import {ApplicationGroup, AuthAction, AuthState, ParentEntity, Session} from "./types";

const initialState = { isLoggedIn: false, referrer: null };

const reducer: (prev: any, action: any) => ({ referrer: string | null; applicationGroups?: ApplicationGroup[]; session?: Session; parentEntity?: ParentEntity; isLoggedIn: boolean; brandingLogo?: string }) = (prev, action) => {
  switch (action.type) {
    case "set_referrer":
      return { ...prev, referrer: action.referrer };
    case "login":
      return { ...prev, isLoggedIn: true };
    case "set_session":
      return {
        ...prev,
        session: {
          canChangePassword: action.session.canChangePassword,
          companyId: action.session.companyId,
          companyName: action.session.companyName,
          firstName: action.session.firstName,
          lastName: action.session.lastName,
          userId: action.session.userId,
        },
        brandingLogo: action.session.brandingLogo,
        applicationGroups: action.session.applicationGroups,
        parentEntity: {
          id: action.session.companyId,
          name: action.session.companyName,
          role: action.session.parentEntityRole,
        },
      };
    case "logout":
      return initialState;
  }
};

export { reducer as authReducer, initialState };
