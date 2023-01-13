import { useContext, useEffect, useRef, useState } from "react";
import { NavLink } from "react-router-dom";
import { UsersService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { AuthContext } from "../../../contexts/AuthContext";
import { errorModal } from "../../../core/Modals";

const ChangePasswordPage = () => {
  const authContext = useContext(AuthContext);
  const [isUpdating, setIsUpdating] = useState(false);
  const currentPasswordInput = useRef();
  const newPasswordInput = useRef();
  const repeatedPasswordInput = useRef();

  useEffect(() => {
    currentPasswordInput.current = document.getElementById(
      "current-password-input"
    );
    newPasswordInput.current = document.getElementById("new-password-input");
    repeatedPasswordInput.current = document.getElementById(
      "repeated-password-input"
    );
  }, []);

  const updatePassword = async () => {
    if (
      currentPasswordInput.length === 0 ||
      newPasswordInput.length === 0 ||
      repeatedPasswordInput.length === 0
    ) {
      return;
    }

    if (
      newPasswordInput.current.value !== repeatedPasswordInput.current.value
    ) {
      errorModal("Repeated password is not the same as new password", 10000);
      return;
    }

    setIsUpdating(true);

    let result = await UsersService.postApiUsersPassword({
      requestBody: {
        currentPassword: currentPasswordInput.current.value,
        newPassword: newPasswordInput.current.value,
      },
    });

    if (!result.isSuccess) {
      errorModal(result.errors, 10000);
    }

    setIsUpdating(false);
  };

  return (
    <>
      <div className="panel">
        {!isUpdating ? (
          <>
            <div className="panel-name">Change Password</div>
            <div className="input-block">
              <div className="key">Current Password: </div>
              <input
                id="current-password-input"
                type="password"
                className="mts-input input"
              />
            </div>
            <div className="input-block">
              <div className="key">New Password: </div>
              <input
                id="new-password-input"
                type="password"
                className="mts-input input"
              />
            </div>
            <div className="input-block">
              <div className="key">Repeat Password: </div>
              <input
                id="repeated-password-input"
                type="password"
                className="mts-input input"
              />
            </div>
            <div className="action-buttons">
              <div className="mts-button item accept" onClick={updatePassword}>
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

export default ChangePasswordPage;
