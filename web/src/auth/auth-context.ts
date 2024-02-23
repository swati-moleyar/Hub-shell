import React, { Dispatch } from "react";
import { initialState } from "./reducer";
import { AuthAction, AuthState } from "./types";

interface IAuthContext {
  state: AuthState;
  dispatch: Dispatch<AuthAction>;
}

const AuthContext = React.createContext<IAuthContext>({
  state: initialState,
  dispatch: () => {},
});

const AuthProvider = AuthContext.Provider;
const AuthConsumer = AuthContext.Consumer;

export { AuthContext, AuthProvider, AuthConsumer, initialState };
export type { IAuthContext };
