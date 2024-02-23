import React, { useEffect, useContext } from "react";
import { useToken, AuthContext } from "../auth";
import { addInteropHandler, removeInteropHandler } from "@iqmetrix/host-interop";
import { Config, Environment } from "../constants";

const InteropHandlers: React.FunctionComponent = (props) => {
  const { getToken } = useToken();
  const { state } = useContext(AuthContext);

  useEffect(() => {
    const authHandler = addInteropHandler<void, object>({
      target: window.top,
      name: "get-authentication-token",
      version: "1",
      requestHandler: async () => {
        const token = await getToken();
        return token.value;
      },
    });

    const envHandler = addInteropHandler<void, Environment>({
      target: window.top,
      name: "get-environment",
      version: "1",
      requestHandler: () => Config.environment,
    });

    const hostContextHandler = addInteropHandler<void, object>({
      target: window.top,
      name: "get-host-context",
      version: "1",
      requestHandler: () => ({
        hostName: "hub",
        hostVersion: require("../../package.json").version,
      }),
    });

    return () => {
      removeInteropHandler({ listener: authHandler });
      removeInteropHandler({ listener: envHandler });
      removeInteropHandler({ listener: hostContextHandler });
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const getParentEntityHandler = addInteropHandler<void, object>({
      target: window.top,
      name: "get-parent-entity",
      version: "1",
      requestHandler: () => ({ ...state.parentEntity }),
    });

    return () => removeInteropHandler({ listener: getParentEntityHandler });
  }, [state.parentEntity]);

  return <>{props.children}</>;
};

export { InteropHandlers };
