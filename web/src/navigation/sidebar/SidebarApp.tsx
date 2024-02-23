import React, { FC, useContext } from "react";
import styled from "styled-components";
import { Application } from "../../auth";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircle } from "@fortawesome/free-regular-svg-icons";
import { faCircle as faCircleSolid } from "@fortawesome/free-solid-svg-icons";
import { ColorBaseBlueDark, ColorBaseBlueLight, ColorWhite } from "@iqmetrix/style-tokens";
import { SidebarContext } from "./context";

const Container = styled.div`
  display: block;
  position: relative;
`;

const StyledAnchor = styled.a`
  color: ${ColorBaseBlueLight};
  user-select: none;
  display: flex;
  padding: 11px 11px 11px 14px;
  cursor: pointer;
  transition: color 100ms;

  &:hover {
    color: ${ColorWhite};
  }
`;

const AppIcon = styled(FontAwesomeIcon)`
  align-self: center;
  margin-left: 4px;
  font-size: 7px;
  z-index: 2;
  background-color: ${ColorBaseBlueDark};
`;

const AppName = styled.div`
  margin-left: 16px;

  &.selected {
    color: ${ColorWhite};
  }
`;

interface Props extends Application {}

const SidebarApp: FC<Props> = (props: Props) => {
  const context = useContext(SidebarContext);
  const { name, href } = props;
  const selected = context?.state.currentApp?.id === props.id;

  return (
    <Container>
      <StyledAnchor
        href={href}
        onClick={() => {
          context.setSidebarState((prev) => {
            return { ...prev, currentApp: props as Application };
          });
        }}
      >
        <AppIcon icon={selected ? faCircleSolid : faCircle} color={selected ? "white" : ""} />
        <AppName className={selected ? "selected" : ""}>{name}</AppName>
      </StyledAnchor>
    </Container>
  );
};

export { SidebarApp };
