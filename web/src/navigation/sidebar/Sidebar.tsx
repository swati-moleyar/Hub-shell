import styled from "styled-components";
import { Application, ApplicationGroup } from "../../auth";
import { SidebarAppGroup } from "./SidebarAppGroup";
import { SidebarCollapse } from "./SidebarCollapse";
import SimpleBar from "simplebar-react";
import React, { FC, useState } from "react";
import { SidebarContextProvider, SidebarState } from "../sidebar/context";
import { HeaderHeight } from "navigation/Header";
import { SidebarExpandedWidth, SidebarCollapsedWidth, SidebarCollapsedKey } from "./constants";
import {
  ColorBaseBlueBase,
  WidthScreenMaxXsmall,
  WidthScreenMinMedium,
  WidthScreenMaxSmall,
} from "@iqmetrix/style-tokens";
import "simplebar/dist/simplebar.min.css";

const ScrollContainer = styled(SimpleBar)`
  width: ${SidebarExpandedWidth}px;
  height: calc(100% - ${HeaderHeight});
  position: absolute;

  .simplebar-scrollbar::before {
    background-color: lightgrey;
  }

  &.collapsed {
    pointer-events: none;
    .simplebar-scrollbar {
      visibility: hidden;
    }
  }

  @media (max-width: ${WidthScreenMaxSmall}) {
    transition: max-height 500ms cubic-bezier(0.19, 1, 0.22, 1), opacity 500ms cubic-bezier(0.19, 1, 0.22, 1);
    box-shadow: 0.1rem 0.1rem 1rem 0.2rem rgba(0, 0, 0, 0.7);
    background: ${ColorBaseBlueBase};
  }

  @media (max-width: ${WidthScreenMaxXsmall}) {
    width: 100%;
  }
`;

const SidebarBacking = styled.div`
  width: ${SidebarExpandedWidth}px;
  height: 100%;
  background: ${ColorBaseBlueBase};
  transition: width 200ms ease;

  &.collapsed {
    width: ${SidebarCollapsedWidth}px;
  }

  @media (max-width: ${WidthScreenMaxSmall}) {
    display: none;
  }
`;

const CollapseContainer = styled.div`
  @media (max-width: ${WidthScreenMaxSmall}) {
    display: none;
  }
`;

/** Get the initial state of the sidebar and set the based on the current hash.
 *  These values are used to highlight the appropriate application in the sidebar.
 */
const getInitialSidebarState = (applicationGroups: ApplicationGroup[]) => {
  const currentLocation = window.location.hash || "#home";
  let currentApp: Application | undefined;

  // find the app that corresponds to the current hash, and it's associated application group
  let initialAppGroup = applicationGroups.find((ag) => {
    currentApp = ag.apps.find((a) => a.href.toLowerCase() === `/${currentLocation.toLowerCase()}`);
    return currentApp;
  });

  let isCollapsed = localStorage.getItem(SidebarCollapsedKey) === "true";

  // if the initialAppGroup has a default app, it behaves more like an app.
  // so technically initialAppGroup is not an actual group, so set that to undefined.
  if (initialAppGroup?.defaultApp) {
    initialAppGroup = undefined;
  }

  return { initialAppGroup, currentApp, isCollapsed };
};

interface Props {
  open: boolean;
  applicationGroups: ApplicationGroup[];
}

const Sidebar: FC<Props> = ({ open, applicationGroups }) => {
  const [sidebarState, setSidebarState] = useState<SidebarState>(getInitialSidebarState(applicationGroups));
  const isCollapsed = sidebarState.isCollapsed && window.matchMedia(`(min-width: ${WidthScreenMinMedium})`).matches;

  return (
    <>
      <SidebarBacking className={isCollapsed ? "collapsed" : ""} />
      <ScrollContainer
        className={isCollapsed ? "collapsed" : ""}
        style={{ maxHeight: !open ? "0%" : "100%", opacity: !open ? "0" : "1" }}
      >
        <SidebarContextProvider
          value={{
            state: {
              ...sidebarState,
              isCollapsed: isCollapsed,
            },
            setSidebarState,
          }}
        >
          {applicationGroups.map((ag) => (
            <SidebarAppGroup {...ag} key={ag.name} />
          ))}
          <CollapseContainer>
            <SidebarCollapse />
          </CollapseContainer>
        </SidebarContextProvider>
      </ScrollContainer>
    </>
  );
};

export { Sidebar };
