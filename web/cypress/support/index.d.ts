/// <reference types="cypress" />

declare namespace Cypress {
  interface Chainable {
    /**
     * Replace loading an actual app with a fixture
     * @param appName The name of the app and the hash route it will have
     * @param fixture The name of the fixture to replace the app with
     */
    stubApp(appName: string, fixture: string): Cypress.Chainable;
  }
}
