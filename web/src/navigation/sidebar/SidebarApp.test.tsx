import React from "react";
import { screen } from "@testing-library/react";
import { render } from "../../utilities";

import { SidebarApp } from "./SidebarApp";
import { Application } from "../../auth";
import { SidebarContextProvider } from "./context";

describe("sidebar app", () => {
  test("renders name of app", () => {
    const app: Application = { name: "Pokedex", href: "/#pokedex", id: "pokedex", version: 1 };
    render(<SidebarApp {...app} />);
    const sidebarAppElement = screen.getByText("Pokedex");
    expect(sidebarAppElement).toBeInTheDocument();
    expect(sidebarAppElement.classList.contains("selected")).toBeFalsy();
  });

  test("currentApp should be selected", () => {
    const app: Application = { name: "Pokedex", href: "/#pokedex", id: "pokedex", version: 1 };
    render(
      <SidebarContextProvider value={{ state: { currentApp: app }, setSidebarState: () => {} }}>
        <SidebarApp {...app} />
      </SidebarContextProvider>
    );
    const sidebarAppElement = screen.getByText("Pokedex");
    expect(sidebarAppElement.classList.contains("selected")).toBeTruthy();
  });
});
