import React from "react";
import { screen } from "@testing-library/react";
import { render } from "../utilities";
import { Header } from "./Header";
import { AuthProvider } from "../auth";

const companyName = "Goats R Us";
const firstName = "Billy";
const lastName = "Goat";

const withContext = (component: JSX.Element) => (
  <AuthProvider
    value={{
      state: { session: { companyName, firstName, lastName, userId: 23, companyId: 532, canChangePassword: false } },
      dispatch: () => false,
    }}
  >
    {component}
  </AuthProvider>
);

describe("Header tests", () => {
  test("renders Header", () => {
    render(<Header />);
    const linkElement = screen.getByText("Hub");
    expect(linkElement).toBeInTheDocument();
  });

  test("displays user's name", () => {
    render(withContext(<Header />));
    const name = screen.getByText(`${firstName} ${lastName}`);
    expect(name).toBeInTheDocument();
  });

  test("displays company name", () => {
    render(withContext(<Header />));
    const company = screen.getByText(companyName);
    expect(company).toBeInTheDocument();
  });
});
