import { useContext, useEffect, useRef, useState } from "react";
import { NavLink } from "react-router-dom";
import { UsersService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { AuthContext } from "../../../contexts/AuthContext";
import { errorModal } from "../../../core/Modals";

const ChangeUsernamePage = () => {
  const authContext = useContext(AuthContext);
  const [isUpdating, setIsUpdating] = useState(false);
  const usernameInput = useRef();

  useEffect(() => {
    usernameInput.current = document.getElementById("username-input");
  }, []);

  const updateUsername = async () => {
    setIsUpdating(true);

    let newUsername = usernameInput.current.value;

    if (
      !newUsername.length > 0 ||
      newUsername === authContext.authState.username
    ) {
      setIsUpdating(false);
      return;
    }

    let result = await UsersService.postApiUsersUsername({
      requestBody: { username: newUsername },
    });

    if (result.isSuccess) {
      authContext.setAuthState({
        ...authContext.authState,
        username: newUsername,
      });
    } else {
      errorModal(result.errors, 10000);
    }

    setIsUpdating(false);
  };
  return (
    <>
      <div className="panel">
        {!isUpdating ? (
          <>
            <div className="panel-name">Change Username </div>
            <div className="input-block">
              <div className="key">New username: </div>
              <input
                id="username-input"
                type="text"
                className="mts-input input"
                placeholder={authContext.authState.username}
              />
            </div>
            <div className="action-buttons">
              <div className="mts-button item accept" onClick={updateUsername}>
                Accept
              </div>
              <NavLink to="user/me" className="mts-button item cancel">
                Cancel
              </NavLink>
            </div>
          </>
        ) : (
          <div style={{ height: "200px" }}>
            <Loader />
          </div>
        )}
      </div>
    </>
  );
};

export default ChangeUsernamePage;
