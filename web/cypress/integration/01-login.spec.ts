/// <reference path="../support/index.d.ts" />

describe("user can login", () => {
  let token: string;;
  before(() => {
    cy.stubApp("Home", "home.html")
      .then(() => cy.loginWithKeyVaultCredentials({
        envSuffix: "int",
        user: "demo",
        vault: "hubtests"
      }))
      .then((x: { token: string }) => {
        token = x.token;
      });
  });

  beforeEach(() => {
    window.localStorage.setItem("token", JSON.stringify({ value: token, refreshToken: "sdfsd", expiresUtc: "2100-01-01T00:00:00Z" }));
  });

  it("shows user's name", () => {
    cy.intercept("GET", "/api/session").as("session");
    cy.visit("/");
    cy.wait("@session", { timeout: 15000 });

    cy.findAllByText("Hub Test").should("exist");
  });
});
