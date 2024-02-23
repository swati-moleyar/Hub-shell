import { useContext, useMemo } from "react";
import { AuthContext } from "../auth-context";

const useSession = () => {
  const { state } = useContext(AuthContext);
  const session = useMemo(
    () => ({
      session: state.session
        ? {
            user: {
              id: state.session?.userId,
              firstName: state.session?.firstName,
              lastName: state.session?.lastName,
              canChangePassword: state.session?.canChangePassword,
            },
            company: {
              name: state.session?.companyName,
              id: state.session?.companyId,
            },
            brandingLogo: state.brandingLogo,
            applicationGroups: state.applicationGroups,
          }
        : undefined,
    }),
    [state.session, state.brandingLogo, state.applicationGroups]
  );

  return session;
};

export { useSession };
