import React, { useState, useRef, useEffect } from "react";
import styled from "styled-components";
import {
  ColorBaseCharcoalBase,
  ColorBaseCloudDark,
  ColorBaseCloudLight,
  PaddingBaseSmall,
  PaddingBaseXsmall,
  ColorBaseWhiteBase,
  FontSizeBaseBody,
  HeightButtonLarge,
  SpacingBaseSmall,
  MarginBaseXsmall,
  BorderRadiusBaseBase,
  BorderWidthBaseBase,
} from "@iqmetrix/style-tokens";
import { useSession, useLogout } from "../../auth";
import { useFormatMessage } from "../../utilities";
import { ChangePasswordUrl, HelpUrl } from "../../constants/endpoints";

const DROPDOWN_MIN_WIDTH = "180px";

// Need to attach the passed className prop to the DOM element
const FontAwesomeIcon = (props: { icon: string; className?: string }) => (
  <i className={`${props.className} fa fa-${props.icon}`} aria-hidden="true"></i>
);

const SubmenuIcon = styled(FontAwesomeIcon)`
  margin-right: ${MarginBaseXsmall};
  color: ${ColorBaseCharcoalBase};
  font-size: ${FontSizeBaseBody};
`;

const DownCaret = styled(FontAwesomeIcon)`
  margin-left: ${SpacingBaseSmall};
`;

const Menu = styled.div`
  align-items: center;
  margin: 0 ${MarginBaseXsmall};
`;

const Dropdown = styled.div`
  position: absolute;
  top: ${HeightButtonLarge};
  right: 0;
  border: ${BorderWidthBaseBase} solid ${ColorBaseCloudDark};
  border-radius: 0 0 ${BorderRadiusBaseBase} ${BorderRadiusBaseBase};
  background: ${ColorBaseWhiteBase};
  padding: 0;
`;

const DropdownButton = styled.button`
  background: transparent;
  border: none;
  cursor: pointer;
  opacity: 0.7;
  color: ${ColorBaseWhiteBase};
  display: flex;
  align-items: center;

  &:hover {
    opacity: 1;
  }

  &:focus {
    outline: none;
  }
`;

const List = styled.ul`
  width: 100%;
  min-width: ${DROPDOWN_MIN_WIDTH};
  list-style: none;
  margin-bottom: 0;
  padding-left: 0;
`;

const ListItem = styled.li`
  cursor: pointer;
  padding: 0;
  position: relative;
  &:hover {
    background-color: ${ColorBaseCloudLight};
    text-decoration: underline;
  }

  :last-child {
    border-top: ${BorderWidthBaseBase} solid ${ColorBaseCloudDark};
  }
`;

const ListAnchor = styled.a`
  color: ${ColorBaseCharcoalBase};
  display: block;
  padding: ${PaddingBaseXsmall} ${PaddingBaseSmall};
`;

export const ActionMenu: React.FC = (props) => {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const { session } = useSession();
  const logout = useLogout();
  const dropdownRef = useRef<any>(null);
  const canChangePassword = session?.user.canChangePassword;
  const changePasswordText = useFormatMessage("Menu.change.password");
  const helpText = useFormatMessage("Menu.help");
  const signOutText = useFormatMessage("Menu.sign.out");
  const ariaLabel = useFormatMessage("Menu.account.options");

  useEffect(() => {
    const pageClickEvent = (event: MouseEvent) => {
      // If the active element exists and is clicked outside of
      if (dropdownRef.current !== null && !dropdownRef.current.contains(event.target)) {
        setIsOpen(!isOpen);
      }
    };
    // If the item is open then listen for clicks
    if (isOpen) {
      window.addEventListener("click", pageClickEvent);
    }
    return () => {
      window.removeEventListener("click", pageClickEvent);
    };
  }, [isOpen]);

  return (
    <Menu>
      <DropdownButton type="button" data-toggle="dropdown" id="menu-button" onClick={() => setIsOpen(!isOpen)}>
        {props.children}
        <DownCaret icon="caret-down" />
      </DropdownButton>
      {isOpen && (
        <Dropdown id="dropdown-menu-content" ref={dropdownRef}>
          <List className="dropdown-menu" aria-label={ariaLabel}>
            {canChangePassword && (
              <ListItem>
                <ListAnchor href={ChangePasswordUrl()}>
                  <SubmenuIcon icon="key" />
                  {changePasswordText}
                </ListAnchor>
              </ListItem>
            )}
            <ListItem>
              <ListAnchor href={HelpUrl()}>
                <SubmenuIcon icon="question" />
                {helpText}
              </ListAnchor>
            </ListItem>
            <ListItem onClick={logout}>
              <ListAnchor>
                <SubmenuIcon icon="sign-out" />
                {signOutText}
              </ListAnchor>
            </ListItem>
          </List>
        </Dropdown>
      )}
    </Menu>
  );
};
