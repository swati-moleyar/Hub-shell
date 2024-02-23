import { Session } from ".";
import { authReducer, initialState } from "./reducer";

describe("auth reducer", () => {
  it("should set referrer", () => {
    const referrer = "great ref right here";
    const result = authReducer(initialState, { type: "set_referrer", referrer: "great ref right here" });

    expect(result.referrer).toEqual(referrer);
  });

  it("login should set isLoggedIn", () => {
    const result = authReducer(initialState, { type: "login" });

    expect(result.isLoggedIn).toBeTruthy();
  });

  it("should set session", () => {
    const session: Session = {
      companyId: 234,
      companyName: "Tree Hugger Co.",
      firstName: "Marcia",
      lastName: "Wine",
      userId: 283,
      canChangePassword: false,
    };

    const result = authReducer(initialState, { type: "set_session", session });

    expect(result.session).toEqual(session);
  });

  it("logout should clear state", () => {
    const state = {
      referrer: "such a great ref",
      isLoggedIn: true,
      session: {
        companyId: 234,
        companyName: "Tree Hugger Co.",
        firstName: "Marcia",
        lastName: "Wine",
        userId: 283,
        canChangePassword: false,
      },
    };

    const result = authReducer(state, { type: "logout" });

    expect(result).toEqual(initialState);
  });
});
