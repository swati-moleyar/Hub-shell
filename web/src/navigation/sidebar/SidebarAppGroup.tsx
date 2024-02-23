import React, { FC, useContext, useState } from "react";
import styled from "styled-components";
import { ApplicationGroup } from "../../auth";
import { SidebarApp } from "./SidebarApp";
import { SidebarAppGroupPopover } from "./SidebarAppGroupPopover";
import {
  ColorBaseBlueDark,
  ColorBaseBlueDarker,
  ColorBaseBlueBase,
  ColorWhite,
  WidthScreenMaxXsmall,
} from "@iqmetrix/style-tokens";
import { SidebarContext } from "./context";
import { SidebarExpandedWidth, SidebarCollapsedWidth } from "./constants";

const AppGroupContainer = styled.div`
  display: block;
  position: relative;
  width: ${SidebarExpandedWidth}px;
  transition: background-color 50ms linear, width 200ms ease;
  background: ${ColorBaseBlueBase};

  &.selected,
  &:hover,
  &:active {
    background-color: ${ColorBaseBlueDarker};
  }

  &.collapsed {
    width: ${SidebarCollapsedWidth}px;
    &:hover #popover {
      width: calc(${SidebarExpandedWidth}px - ${SidebarCollapsedWidth}px);
    }
  }

  @media (max-width: ${WidthScreenMaxXsmall}) {
    width: 100%;
  }
`;

const GroupContainer = styled.a`
  pointer-events: all;
  user-select: none;
  color: ${ColorWhite};
  padding: 14px 0px;
  display: flex;
  cursor: pointer;
  width: 100%;
  overflow: hidden;

  &:hover,
  &:active {
    color: ${ColorWhite};
  }
`;

const GroupIcon = styled.i`
  align-self: center;
  flex: 0 0 ${SidebarCollapsedWidth}px;
  text-align: center;
  font-size: 1.2rem;
`;

const GroupNameContainer = styled.div`
  flex: 1 0 calc(${SidebarExpandedWidth}px - ${SidebarCollapsedWidth}px);
  display: flex;
`;

const GroupName = styled.div`
  text-transform: uppercase;
  width: 100%;
`;

const ExpandIcon = styled.i`
  margin-left: auto;
  margin-right: 14px;
  align-self: center;
  transition: transform 100ms linear;

  &.open {
    transform: rotate(90deg);
  }
`;

const GroupPopover = styled.div`
  pointer-events: all;
  overflow: hidden;
  position: absolute;
  left: ${SidebarCollapsedWidth}px;
  top: 0px;
  width: 0px;
  transition: width 200ms ease;
`;

const GroupChildrenContainer = styled.div`
  pointer-events: all;
  position: relative;
  overflow: hidden;
  background-color: ${ColorBaseBlueDark};
  transition: max-height 200ms ease;
  max-height: 0;

  &.open {
    max-height: 1000px;
  }

  &::before {
    display: block;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 21px;
    background-image: linear-gradient(to bottom, rgba(255, 255, 255, 0.5) 40%, transparent 20%);
    background-repeat: repeat-y;
    background-position: 0;
    background-size: 1px 4px;
    width: 1px;
    content: " ";
  }

  > *:last-child::before {
    display: block;
    position: absolute;
    top: 1.5rem;
    bottom: 0;
    left: 18px;
    background-color: ${ColorBaseBlueDark};
    pointer-events: none;
    width: 7px;

    content: " ";
  }
`;

interface Props extends ApplicationGroup {}

const getIconClass = (icon: string, name: string) => {
  return icon ? (/(icon|fa)-/gi.test(icon) ? icon : "icon-" + icon) : "icon-" + name.toLowerCase();
};

const SidebarAppGroup: FC<Props> = (props: Props) => {
  const context = useContext(SidebarContext);
  const { currentApp, isCollapsed } = context.state;
  const { name, icon, apps, defaultApp } = props;
  const [groupOpen, setGroupOpen] = useState(context.state.initialAppGroup?.name === name);
  const isGroupExpandable = !(defaultApp || name === "Settings");
  const isCurrentAppDefaultApp = currentApp && defaultApp && currentApp.name === defaultApp.name;
  const isCurrentAppInGroup = currentApp && apps.filter((app) => app.id === currentApp.id).length > 0;
  const isGroupSelected = (groupOpen && !isCollapsed) || isCurrentAppDefaultApp || isCurrentAppInGroup;

  return (
    <AppGroupContainer className={`${isCollapsed ? "collapsed" : ""}  ${isGroupSelected ? "selected" : ""}`}>
      <GroupContainer
        onClick={() => {
          if (!isGroupExpandable && defaultApp) {
            window.location.href = defaultApp.href;
            context.setSidebarState((prev) => {
              return { ...prev, currentApp: defaultApp };
            });
          } else if (!isCollapsed) {
            setGroupOpen(!groupOpen);
          }
        }}
      >
        <GroupIcon className={`fa ${getIconClass(icon, name)} `} />
        <GroupNameContainer>
          <GroupName>{name}</GroupName>
          {isGroupExpandable && <ExpandIcon className={`fa fa-angle-right ${groupOpen ? "open" : ""}`} />}
        </GroupNameContainer>
      </GroupContainer>
      {isGroupExpandable && !isCollapsed && (
        <GroupChildrenContainer className={groupOpen ? "open" : ""}>
          {apps.map((app) => (
            <SidebarApp key={app.id} {...app} />
          ))}
        </GroupChildrenContainer>
      )}
      {isCollapsed && (
        <GroupPopover id="popover">
          <SidebarAppGroupPopover {...props} />
        </GroupPopover>
      )}
    </AppGroupContainer>
  );
};

export { SidebarAppGroup };
