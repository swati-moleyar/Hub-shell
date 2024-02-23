import React, { FC, useContext, useEffect } from "react";
import styled from "styled-components";
import { ColorBaseBlueLight, ColorBaseBlueBase, ColorWhite } from "@iqmetrix/style-tokens";
import { SidebarContext } from "./context";
import { SidebarExpandedWidth, SidebarCollapsedWidth, SidebarCollapsedKey } from "./constants";
import ReactGA from "react-ga";
import { Config } from "../../constants";

const CollapseContainer = styled.div`
  padding: 3px;
  position: relative;
  width: ${SidebarExpandedWidth}px;
  transition: width 200ms ease;
  pointer-events: all;

  &.collapsed {
    width: ${SidebarCollapsedWidth}px;
  }

  &::before {
    position: absolute;
    top: 13px;
    left: 14px;
    right 14px;
    border-top: 1px solid ${ColorBaseBlueLight};
    content: " ";
  }
`;

const Collapse = styled.a`
  user-select: none;
  transition: background-color 50ms linear;
  color: ${ColorWhite};
  cursor: pointer;
  position: relative;
  display: flex;
  margin: auto;
  width: fit-content;
  border-radius: 100%;
  border: 1px solid ${ColorBaseBlueLight};
  background: ${ColorBaseBlueBase};

  &.open,
  &:hover,
  &:active {
    background-color: ${ColorBaseBlueLight};
    color: ${ColorWhite};
  }
`;

const CollapseIcon = styled.i`
  font-size: 1.4rem;
  width: 1.6rem;
  text-align: center;
`;

const SidebarCollapse: FC = () => {
  const context = useContext(SidebarContext);
  const { isCollapsed } = context.state;

  useEffect(() => {
    ReactGA.set({ [Config.googleAnalyticsSidebarDimension]: isCollapsed ? "closed" : "open" });
  }, [isCollapsed]);

  return (
    <CollapseContainer className={`${isCollapsed ? "collapsed" : ""}`}>
      <Collapse
        onClick={() => {
          context.setSidebarState((prev) => {
            const isCollapsed = !prev.isCollapsed;
            localStorage.setItem(SidebarCollapsedKey, isCollapsed.toString());
            return { ...prev, isCollapsed: isCollapsed };
          });
        }}
      >
        <CollapseIcon className={`fa fa-angle-double-${isCollapsed ? "right" : "left"}`} />
      </Collapse>
    </CollapseContainer>
  );
};

export { SidebarCollapse };
