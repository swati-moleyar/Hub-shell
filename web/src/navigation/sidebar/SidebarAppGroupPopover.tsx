import React, { FC, useContext } from "react";
import styled from "styled-components";
import { ApplicationGroup } from "../../auth";
import { ColorBaseBlueDark, ColorBaseBlueDarker, ColorBaseBlueLight, ColorWhite } from "@iqmetrix/style-tokens";
import { SidebarContext } from "./context";
import { SidebarExpandedWidth, SidebarCollapsedWidth } from "./constants";

const Container = styled.div``;

const GroupName = styled.div`
  text-transform: uppercase;
  padding: 14px;
  background-color: ${ColorBaseBlueDarker};
  color: ${ColorWhite};
  width: calc(${SidebarExpandedWidth}px - ${SidebarCollapsedWidth}px);
`;

const GroupChildrenContainer = styled.div`
  background-color: ${ColorBaseBlueDark};
  overflow: hidden;
`;

const StyledAnchor = styled.a`
  color: ${ColorBaseBlueLight};
  user-select: none;
  display: flex;
  padding: 11px 11px 11px 14px;
  cursor: pointer;
  transition: color 100ms;
  width: calc(${SidebarExpandedWidth}px - ${SidebarCollapsedWidth}px);

  &:hover {
    color: ${ColorWhite};
  }
`;

const AppName = styled.div`
  &.selected {
    color: ${ColorWhite};
  }
`;

interface Props extends ApplicationGroup {}

const SidebarAppGroupPopover: FC<Props> = (props: Props) => {
  const context = useContext(SidebarContext);
  const { name, apps } = props;

  return (
    <Container>
      <GroupName>{name}</GroupName>
      <GroupChildrenContainer>
        {apps.map((app) => (
          <StyledAnchor
            key={app.id}
            href={app.href}
            onClick={() => {
              context.setSidebarState((prev) => {
                return { ...prev, currentApp: app };
              });
            }}
          >
            <AppName className={context?.state.currentApp?.id === app.id ? "selected" : ""}>{app.name}</AppName>
          </StyledAnchor>
        ))}
      </GroupChildrenContainer>
    </Container>
  );
};

export { SidebarAppGroupPopover };
