import React from "react";
import { screen } from "@testing-library/react";
import { render } from "../../utilities";
import { SidebarAppGroup } from "./SidebarAppGroup";
import { Application, ApplicationGroup } from "../../auth";

describe("sidebar app group", () => {
  test("renders app group name", () => {
    const app: Application = { name: "Pokedex", href: "/#pokedex", id: "pokedex", version: 1 };
    const appGroup: ApplicationGroup = { name: "Pokemon Apps", defaultApp: app, icon: "basketball", apps: [app] };
    render(<SidebarAppGroup {...appGroup} key={appGroup.name} />);
    const sidebarAppGroupElement = screen.getByText("Pokemon Apps");
    expect(sidebarAppGroupElement).toBeInTheDocument();
  });
});
