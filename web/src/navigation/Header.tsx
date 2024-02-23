import React from "react";
import styled from "styled-components";
import {
  ColorBaseCharcoalBase,
  ColorBaseCharcoalLight,
  ColorBaseWhiteBase,
  MarginBaseLarge,
  HeightButtonSmall,
  MarginBaseXsmall,
  FontWeightBaseSubheading,
  HeightButtonLarge,
  MarginBaseBase,
} from "@iqmetrix/style-tokens";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import { Grid } from "@iqmetrix/antd";
import { useSession } from "../auth";
import { ActionMenu } from "./components/ActionMenu";

library.add(faBars);
const { useBreakpoint } = Grid;

export const HeaderHeight = `${HeightButtonLarge}`;

const MenuButton = styled(FontAwesomeIcon)`
  height: ${HeightButtonSmall};
  margin-left: ${MarginBaseXsmall};
`;

const Container = styled.nav`
  background-color: ${ColorBaseCharcoalBase};
  height: ${HeaderHeight};
  display: flex;
  flex-direction: row;
  align-items: center;
  z-index: 99;
`;

const Faded = styled.div`
  opacity: 0.7;
  color: ${ColorBaseWhiteBase};
  display: flex;

  &:hover {
    opacity: 1;
  }
`;

const Divider = styled.div`
  margin: 0 ${MarginBaseXsmall};
  background: ${ColorBaseCharcoalLight};
  width: 1px;
  height: ${HeightButtonSmall};
`;

const Brand = styled(Faded)`
  margin-left: ${MarginBaseBase};
  font-weight: ${FontWeightBaseSubheading};
  align-items: center;
`;

const Company = styled(Faded)`
  flex: 1;
  margin: 0 ${MarginBaseLarge};
  flex-direction: row-reverse;
`;

interface Props {
  menuButtonActive: boolean;
  onMenuButtonClick: () => void;
}

export const Header: React.FC<Props> = ({ menuButtonActive, onMenuButtonClick }) => {
  const { md, xs } = useBreakpoint();
  const { session } = useSession();

  const companyName = session?.company.name;
  const usersName = `${session?.user.firstName} ${session?.user.lastName}`;

  return (
    <Container>
      {!md && (
        <MenuButton
          icon="bars"
          size="2x"
          color={menuButtonActive ? ColorBaseWhiteBase : ColorBaseCharcoalLight}
          onClick={onMenuButtonClick}
        />
      )}
      {!xs && (
        <Brand>
          <img src={session?.brandingLogo} alt="Brand Logo" />
          <Divider />
          Hub
        </Brand>
      )}
      <Company>{companyName}</Company>
      <ActionMenu> {usersName} </ActionMenu>
    </Container>
  );
};
