import React, { Dispatch, SetStateAction } from "react";
import { ApplicationGroup, Application } from "../../auth";

export interface SidebarState {
  initialAppGroup?: ApplicationGroup;
  currentApp?: Application;
  isCollapsed: boolean;
}

export interface ISidebarContext {
  state: SidebarState;
  setSidebarState: Dispatch<SetStateAction<SidebarState>>;
}

const initialState = {
  state: { initialAppGroup: undefined, currentApp: undefined, isCollapsed: false },
  setSidebarState: () => {},
};

const SidebarContext = React.createContext<ISidebarContext>(initialState);

const SidebarContextProvider = SidebarContext.Provider;
const SidebarContextConsumer = SidebarContext.Consumer;

export { SidebarContext, SidebarContextProvider, SidebarContextConsumer, initialState };
