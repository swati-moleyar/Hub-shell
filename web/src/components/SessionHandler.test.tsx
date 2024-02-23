import React from "react";
import { render } from "@testing-library/react";
import { SessionHandler } from "./SessionHandler";
import { AuthProvider, AuthState } from "../auth";

const mockLogin = jest.fn();
const mockToken = jest.fn();
const mockSession = jest.fn();

jest.mock("../auth", () => ({
  ...(jest.requireActual("../auth") as any),
  useLogin: () => ({ login: mockLogin }),
  useToken: () => ({ getToken: mockToken }),
}));

jest.mock("../utilities", () => ({ useAsyncError: () => ({}) }));
jest.mock("../http", () => ({
  useHttp: () => ({
    httpClient: {
      getSession: mockSession,
    },
  }),
}));

const withContext = (component: JSX.Element, authState?: Partial<AuthState>) => (
  <AuthProvider
    value={{
      state: {
        isLoggedIn: true,
        referrer: "blah",
        ...authState,
      },
      dispatch: () => false,
    }}
  >
    {component}
  </AuthProvider>
);

describe("SessionHandler tests", () => {
  test("attempts login if can't get token", async () => {
    mockToken.mockImplementation(() => Promise.reject());

    render(
      withContext(
        <SessionHandler>
          <div />
        </SessionHandler>
      )
    );
    // Note: this is required because the expect finishes running before the
    // useEffect does, so add this to the end of the Promise queue to "guarantee" it
    // to finish running the useEffect
    await Promise.resolve();

    expect(mockLogin).toBeCalled();
  });

  test("gets session if you're logged in", () => {
    mockToken.mockImplementationOnce(() => Promise.reject());

    render(
      withContext(
        <SessionHandler>
          <div />
        </SessionHandler>,
        { isLoggedIn: true }
      )
    );

    expect(mockSession).toBeCalled();
  });

  test("Shows spinner when not logged in", async () => {
    mockToken.mockImplementationOnce(() => new Promise(() => {}));

    const { queryByText } = render(withContext(<SessionHandler>Hello there</SessionHandler>));

    const text = queryByText("Hello there");
    expect(text).not.toBeInTheDocument();
  });

  test("Shows text when authenticated with session", async () => {
    mockToken.mockImplementationOnce(() => Promise.resolve());

    const { queryByText } = render(
      withContext(<SessionHandler>Hello there</SessionHandler>, { session: {} as any, isLoggedIn: true })
    );

    const text = queryByText("Hello there");
    expect(text).toBeInTheDocument();
  });
});
