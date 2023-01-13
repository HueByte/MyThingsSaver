import { useContext, useState, useRef, useEffect } from "react";
import { NavLink } from "react-router-dom";
import { AuthContext } from "../../../contexts/AuthContext";
import { FaGhost } from "react-icons/fa";
import DefaultAvatar from "../../../assets/DefaultAvatar.png";
import "./ChangeAvatar.scss";
import { UsersService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { errorModal } from "../../../core/Modals";

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

    let url = avatarInput.current.value;
    let result = await UsersService.postApiUsersAvatar({
      requestBody: {
        avatarUrl: url,
      },
    });

    if (result.isSuccess) {
      authContext.setAuthState({
        ...authContext.authState,
        avatarUrl: url,
      });
    } else {
      errorModal(result.errors);
    }

    setAvatarUrl(url);
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
              <a target="_blank" href={authContext.authState.avatarUrl}>
                Click here
              </a>
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
