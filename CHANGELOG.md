# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [1.1.0](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/branchCompare?baseVersion=GTv1.0.0&targetVersion=GTv1.1.0) (2021-08-04)


### Features

* Add ability to load Hub V1/V2 and React apps ([d220f42](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/d220f425606b219b45d80b0a893fbcb230731f06)), closes [#196792](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196792) [#196793](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196793) [#196792](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196792) [#196793](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196793)
* Add google analytics ([9da6dae](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/9da6dae274b7505d363a9336f0b4db81eb8d5ea3))
* Add mobile friendly styles to sidebar ([419a330](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/419a330d2b71038b07bbb075864381da047a981d)), closes [#196797](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196797)
* Add sentry ([eb67400](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/eb674004c7629072a0dd8230a7a580eadf8ee149))
* Add sidebar navigation ([663db84](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/663db84cf18a5bb9a2897b31dffdb84519fcd2b1)), closes [AB#196841](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196841) [#196841](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196841)
* Added a handler for the host-interop get-authentication-token request ([1869aaf](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/1869aaf0394739f8d2dbf5cfbd06e68669052878)), closes [#196791](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196791)
* Display current user's name and company in header nav bar ([20e9bf3](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/20e9bf3d39a5b4eb864a5ad5a191e717f0bfb0eb))
* get host context ([cf51aa5](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/cf51aa55b79605b856ac4967b0407ee7075cf142))
* get hub applications for user ([717ff51](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/717ff5155fee1b9c9b15dff2128edd1c1bf1a235)), closes [#196796](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196796)
* Inactivity Timer ([e7ddbb9](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/e7ddbb9e54ce0d4627da31a48e64a215b4d7d14a))
* logo per origin ([7856261](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/78562619a7ff2ba7e6dfcd481dc53f8bd1f8b6b1))
* make sidebar collapsible ([de016de](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/de016de71c4e58b279aa3e50218b5f43cdce10da))
* support get-environment ([49fde74](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/49fde7436e73afcd0b7d6ae7385592d3d3fc9a1f))
* support get-parent-entity ([7f3c8be](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/7f3c8be1cfd3eb56fd271bcd7ff94ee6782eb60b))
* Sync top frame and iframe urls ([5780708](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/57807086df9cc28c5fd1d9d242d7d842812183d7))
* sync top level url with iframe src ([b7e0847](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/b7e084750edac4f044ee5a76e21d8e2f3532f87a))


### Bug Fixes

* Apps now load correctly when the hash is changed ([0e89b8f](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/0e89b8fca95b68883f82775a39812b91d25d4319))
* Fix app content hiding the top bar ([4335b8b](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/4335b8b00c7e1f93fa93426efd302b43d7f41be1))
* minor google analytics changes ([2abb0f0](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/2abb0f0809effcf68b79d8f9f187831406372adf))
* Minor sidebar fixes ([c313d04](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/c313d04a71943ceeeb7cd3bbe51ba6009764040f))
* Re-store token to fix redirecting when losing session storage token ([1edc027](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/1edc0278bc7e743789855b242691fbad576f2751))
* Reduced excessive /session calls ([47a86e9](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/47a86e92931bb3723c079168ab2512bac317d218))
* Remove loading Home before the app pings are complete ([a9faaaa](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/a9faaaa29a8f96b0e24421f9d89805dd965cc812))
* The ping endpoint is now proxied properly to the shell for hubdev.iqmetrix.net ([f291f88](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/f291f8808d02d6450340779d1f1c70334abb71bc)), closes [#200656](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/200656)
* Use hubdev.iqmetrix urls ([6d75ccd](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/6d75ccd809a6c4d59ea1b947d3f7d8c8a00158f5))
* Use iQmetrixHub client ID to fix host/app token sharing ([3d69697](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/3d69697949cf51facf1d16c4c032b1820a97307c)), closes [/github.com/iQmetrix/Hub/blob/30689b94bb74c83b7d08b1fc5f59d9aa514a98b1/projects/plugins/hub.plugin.auth/src/authenticator.ts#L114](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/L114)
* Users are now redirected back to apps after logging in ([3f42134](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/3f42134ad7dacf26a2b05c905f947bde99588385)), closes [#200861](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/200861)

## 1.0.0 (2020-12-08)


### Features

* add nav bar header ([4ce1257](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/4ce12572a9dc72603090403a87aa47c673e509da))
* Add session endpoint to api ([9ad464e](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/9ad464e9bb65e5b378ae62de805720793b319422))
* Added auth token endpoint to the BFF ([0c807fc](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/0c807fc29e8fa9d753b714ef16518a679cebeea5)), closes [#196789](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196789)
* Added frontend auth flow ([668be9a](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/668be9a70cefa7cd5086247c9d905a2a13d3b552))
* Auth tokens automatically refresh when fetched (if necessary) ([c227ebd](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/c227ebd8b42398f21d88c922cb9eac85fbc29355)), closes [#196790](https://dev.azure.com/iqmetrix/Hub/_workitems/edit/196790)


### Bug Fixes

* Auth controller tests are updated to match contract ([155c958](https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell/commit/155c958e5a6e90fb8f8e52794cfa98f2f4a2653d))
