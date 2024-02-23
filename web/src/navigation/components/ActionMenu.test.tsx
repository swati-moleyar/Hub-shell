import React from "react";
import { screen, fireEvent, getByText } from "@testing-library/react";
import { render } from "../../utilities";
import { ActionMenu } from "./ActionMenu";
import { AuthProvider } from "../../auth";
import { Session } from "../../auth/types";

const companyName = "Goats R Us";
const firstName = "Billy";
const lastName = "Goat";

const externalState = {
  session: { companyName, firstName, lastName, userId: 23, companyId: 532, canChangePassword: true },
};

const internalState = {
  session: { companyName, firstName, lastName, userId: 23, companyId: 532, canChangePassword: false },
};

const withContext = (component: JSX.Element, state: { session: Session }) => (
  <AuthProvider
    value={{
      state,
      dispatch: () => false,
    }}
  >
    {component}
  </AuthProvider>
);

describe("Action menu for external user", () => {
  beforeEach(() => {
    render(
      withContext(
        <ActionMenu>
          {externalState.session.firstName} {externalState.session.lastName}
        </ActionMenu>,
        externalState
      )
    );
    fireEvent.click(screen.getByText(`${firstName} ${lastName}`));
  });

  test("displays change password link for an external user", () => {
    const changePasswordText = screen.getByText("Change password");
    expect(changePasswordText).toBeInTheDocument();
  });

  test("displays help link for an external user", () => {
    const helpText = screen.getByText("Help");
    expect(helpText).toBeInTheDocument();
  });

  test("displays sign out link for an external user", () => {
    const signOutText = screen.getByText("Sign out");
    expect(signOutText).toBeInTheDocument();
  });
});

describe("Action menu for internal user", () => {
  beforeEach(() => {
    render(
      withContext(
        <ActionMenu>
          {internalState.session.firstName} {internalState.session.lastName}
        </ActionMenu>,
        internalState
      )
    );
    fireEvent.click(screen.getByText(`${firstName} ${lastName}`));
  });

  test("does not displays change password link for iQ user", () => {
    const changePasswordText = screen.queryByText("Change password");
    expect(changePasswordText).not.toBeInTheDocument();
  });

  test("displays help link for iQ user", () => {
    render(withContext(<ActionMenu />, internalState));
    const helpText = screen.getByText("Help");
    expect(helpText).toBeInTheDocument();
  });

  test("displays sign out link for iQ user", () => {
    render(withContext(<ActionMenu />, internalState));
    const signOutText = screen.getByText("Sign out");
    expect(signOutText).toBeInTheDocument();
  });
});
