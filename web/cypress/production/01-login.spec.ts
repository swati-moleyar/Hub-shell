/// <reference path="../support/index.d.ts" />

describe("user can login", () => {
  let token: string;;
  before(() => {
    cy.loginWithKeyVaultCredentials({
        envSuffix: "int",
        user: "prod",
        vault: "hubtests"
      })
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

    cy.findAllByText("Garrett Samm").should("exist");
  });
});
