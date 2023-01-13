import { useContext, useRef, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { AuthService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { AuthContext } from "../../../contexts/AuthContext";
import { errorModal } from "../../../core/Modals";

const ChangeUsernamePage = () => {
  const authContext = useContext(AuthContext);
  const navigate = useNavigate();
  const [isUpdating, setIsUpdating] = useState(false);
  const usernameInput = useRef();
  const passwordInput = useRef();

  const updateUsername = async () => {
    let newUsername = usernameInput.current.value;
    let password = passwordInput.current.value;

    if (
      !newUsername.length > 0 ||
      !password.length > 0 ||
      newUsername === authContext.authState.username
    ) {
      return;
    }

    setIsUpdating(true);

    let result = await AuthService.postApiAuthUsername({
      requestBody: { username: newUsername, password: password },
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
    navigate("/user/me");
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
                ref={usernameInput}
                type="text"
                className="mts-input input"
                placeholder={authContext.authState.username}
              />
            </div>
            <div className="input-block">
              <div className="key">Password: </div>
              <input
                ref={passwordInput}
                type="password"
                className="mts-input input"
                placeholder="* * * * * * *"
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
