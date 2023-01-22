import { useContext, useEffect, useRef, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { AuthService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { AuthContext } from "../../../contexts/AuthContext";
import { errorModal } from "../../../core/Modals";

const ChangePasswordPage = () => {
  const authContext = useContext(AuthContext);
  const [isUpdating, setIsUpdating] = useState(false);
  const currentPasswordInput = useRef();
  const newPasswordInput = useRef();
  const repeatedPasswordInput = useRef();
  const navigate = useNavigate();

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

    let result = await AuthService.postApiAuthPassword({
      requestBody: {
        currentPassword: currentPasswordInput.current.value,
        newPassword: newPasswordInput.current.value,
      },
    });

    if (!result.isSuccess) {
      errorModal(result.errors, 10000);
    }

    setIsUpdating(false);
    if (result.isSuccess) navigate("/account/user/me");
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
                ref={currentPasswordInput}
                type="password"
                className="mts-input input"
              />
            </div>
            <div className="input-block">
              <div className="key">New Password: </div>
              <input
                ref={newPasswordInput}
                type="password"
                className="mts-input input"
              />
            </div>
            <div className="input-block">
              <div className="key">Repeat Password: </div>
              <input
                ref={repeatedPasswordInput}
                type="password"
                className="mts-input input"
              />
            </div>
            <div className="action-buttons">
              <div className="mts-button item accept" onClick={updatePassword}>
                Accept
              </div>
              <NavLink to="/account/user/me" className="mts-button item cancel">
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
