import { SessionResponse, useToken } from "../auth";
import axios from "axios";
import { SessionEndpoint } from "../constants";
import { useCallback, useMemo } from "react";

const useHttp = () => {
  const { getToken } = useToken();

  const getSession = useCallback(async (): Promise<SessionResponse> => {
    const token = await getToken();
    const headers = { Authorization: `Bearer ${token.value}` };
    try {
      const response = await axios.get(SessionEndpoint(), { headers });
      return response.data as SessionResponse;
    } catch (e) {
      const message = e.response?.data?.message ?? e;
      throw new Error(`Unable to get session: ${message}`);
    }
  }, [getToken]);

  const httpClient = useMemo(
    () => ({
      getSession,
    }),
    [getSession]
  );

  return { httpClient };
};

export { useHttp };
