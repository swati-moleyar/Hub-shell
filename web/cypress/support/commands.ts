// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })
import "@testing-library/cypress/add-commands";
import "@iqmetrix/cypress";

const stubApp = (appName: string, fixture: string) => {
  return cy.intercept(`apps/hub.app.${appName}/ping`, { statusCode: 200 })
    .intercept(`${appName}/ping`, { statusCode: 404 })
    .fixture(fixture)
    .then((loadedFixture) => {
      cy.intercept(`/apps/hub.app.${appName}/`, req => req.reply(loadedFixture));
    });
}

Cypress.Commands.add("stubApp", stubApp);
