import React, { useEffect } from "react";
import { useToken } from "../auth";
import detectInactivity from "@iqmetrix/detect-inactivity";
import { redirectToLogout } from "../auth/utilities";

const InactivityTimer: React.FunctionComponent = (props) => {
  const { getToken } = useToken();

  useEffect(() => {
    const enableInactivityTimer = async () => {
      try {
        const inactive = await detectInactivity({ target: window.document.documentElement });
        if (inactive) {
          const token = await getToken();
          redirectToLogout(token.value);
        } else {
          console.log("inactivity timeout not enabled");
        }
      } catch (error) {
        console.error("an error occured in the inactivity timeout: ", error);
      }
    };

    enableInactivityTimer();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return <>{props.children}</>;
};

export { InactivityTimer };
