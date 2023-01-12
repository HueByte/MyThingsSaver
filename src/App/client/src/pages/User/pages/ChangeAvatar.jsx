import { useContext, useState, useRef, useEffect } from "react";
import { NavLink } from "react-router-dom";
import { AuthContext } from "../../../contexts/AuthContext";
import { FaGhost } from "react-icons/fa";
import DefaultAvatar from "../../../assets/DefaultAvatar.png";
import "./ChangeAvatar.scss";
import { UsersService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";

const ChangeAvatarPage = () => {
  const authContext = useContext(AuthContext);
  const [avatarUrl, setAvatarUrl] = useState(authContext.authState.avatarUrl);
  const [isUpdating, setIsUpdating] = useState(false);
  const avatarInput = useRef();

  useEffect(() => {
    avatarInput.current = document.getElementById("avatar-input");
  }, []);

  const acceptImage = async () => {
    setIsUpdating(true);

    let result = await UsersService.postApiUsersAvatar({
      requestBody: {
        avatarUrl: avatarInput.current.value,
      },
    });

    if (result.isSuccess) {
      let userState = authContext.authState;
      userState.avatarUrl = avatarInput.current.value;

      authContext.setAuthState(userState);
    }

    setIsUpdating(false);
  };

  const previewImage = () => {
    let url = avatarInput.current.value;
    setAvatarUrl(url);
  };

  return (
    <>
      <div className="panel">
        <div className="panel-name">Avatar</div>
        <div className="avatar-preview">
          {isUpdating ? (
            <Loader />
          ) : (
            <img
              src={
                avatarUrl && !avatarUrl.length >= 0 ? avatarUrl : DefaultAvatar
              }
              alt="avatar"
            />
          )}
        </div>
        <div className="block">
          <span className="key">Current Avatar Url: </span>
          <span>
            {authContext.authState.avatarUrl ? (
              <NavLink to={authContext.authState.avatarUrl}>Click here</NavLink>
            ) : (
              <FaGhost />
            )}
          </span>
        </div>
        <input
          id="avatar-input"
          type="text"
          className="mts-input avatar-input"
          placeholder="Place your new avatar url here"
        />
        <div className="action-buttons">
          <div className="mts-button item accept" onClick={acceptImage}>
            Accept
          </div>
          <div className="mts-button item" onClick={previewImage}>
            Preview
          </div>
          <NavLink to="user/me" className="mts-button item cancel">
            Cancel
          </NavLink>
        </div>
      </div>
    </>
  );
};

export default ChangeAvatarPage;
