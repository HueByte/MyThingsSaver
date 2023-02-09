import { useParams } from "react-router";
import { useEffect, useState } from "react";
import { AuthService } from "../../../api";

const UserModerationPage = () => {
  const { username } = useParams();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    (async () => {
      let result = await AuthService.getApiAuthUser({
        username: username,
      });

      setIsLoading(false);
    })();
  }, []);

  return <>{username}</>;
};

export default UserModerationPage;
