import { useContext, useRef, useState } from "react";
import { NavLink } from "react-router-dom";
import { AuthService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { AuthContext } from "../../../contexts/AuthContext";
import { errorModal, successModal } from "../../../core/Modals";

const ChangeEmailPage = () => {
  const authContext = useContext(AuthContext);
  const [isUpdating, setIsUpdating] = useState(false);
  const emailInput = useRef();
  const passwordInput = useRef();

  const updateEmail = async () => {
    let newEmail = emailInput.current.value;
    let password = passwordInput.current.value;

    if (newEmail.length === 0 || password.length === 0) return;

    setIsUpdating(true);

    let result = await AuthService.postApiAuthEmail({
      requestBody: { email: newEmail, password: password },
    });

    if (result.isSuccess) {
      successModal("Email changed successfully", 10000);
      authContext.setAuthState({
        ...authContext.authState,
        email: newEmail,
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
            <div className="panel-name">Change Email </div>
            <div className="input-block">
              <div className="key">New email: </div>
              <input
                ref={emailInput}
                type="text"
                className="mts-input input"
                placeholder={authContext.authState?.email}
                autoComplete="off"
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
              <div className="mts-button item accept" onClick={updateEmail}>
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

export default ChangeEmailPage;
