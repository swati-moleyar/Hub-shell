import { useCallback, useState } from "react";

const useAsyncError = () => {
  const [, setError] = useState();

  const throwErrorOnRender = useCallback(error => {
    setError(() => {
      throw error;
    })
  }, [setError]);

  return {
    throwErrorOnRender
  };
};

export { useAsyncError };