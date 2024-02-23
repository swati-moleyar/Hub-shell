import { getHubAppUrl } from "./endpoints";

describe("endpoint tests", () => {
  describe("hub endpoints", () => {
    it("getHubAppUrl should return v1 formatted url", () => {
      const result = getHubAppUrl(1, "Pokedex");
      expect(result).toBe("/Pokedex");
    });

    it("getHubAppUrl should return hub v2 formatted url", () => {
      const result = getHubAppUrl(2, "Pokedex");
      expect(result).toBe("/apps/hub.app.Pokedex");
    });
  });
});
