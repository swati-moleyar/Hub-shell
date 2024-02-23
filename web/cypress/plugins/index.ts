const { credentialsPlugin } = require("@iqmetrix/cypress/dist/plugins");

module.exports = (on: any, config: any) => {
    return credentialsPlugin(on, config);
};
