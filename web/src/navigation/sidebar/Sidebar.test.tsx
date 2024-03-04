import React from "react";
import { screen } from "@testing-library/react";
import { render } from "../../utilities";
import { Sidebar } from "./Sidebar";
import { Application, ApplicationGroup } from "../../auth";

describe("sidebar", () => {
  test("renders app groups", () => {
    const app: Application = { name: "Pokedex", href: "/#pokedex", id: "pokedex", version: 1 };
    const appGroup: ApplicationGroup = { name: "Pokemon Apps", defaultApp: app, icon: "basketball", apps: [app] };

    const app2: Application = { name: "Goggles", href: "/#goggles", id: "goggles", version: 1 };
    const appGroup2: ApplicationGroup = { name: "Digimon Apps", defaultApp: app, icon: "basketball", apps: [app2] };

    const appGroups: ApplicationGroup[] = [appGroup, appGroup2];

    render(<Sidebar applicationGroups={appGroups}  open/>);

    expect(screen.getByText("Pokemon Apps")).toBeInTheDocument();
    expect(screen.getByText("Digimon Apps")).toBeInTheDocument();
  });
});
