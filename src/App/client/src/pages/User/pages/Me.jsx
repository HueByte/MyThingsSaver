import { useEffect, useState } from "react";
import { FaGhost } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import { AuthService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { capitalizeRole, getSize } from "../../../core/Lib";
import "../User.scss";

const MePage = () => {
  const [userData, setUserData] = useState({});
  const [isFetched, setIsFetched] = useState(false);

  useEffect(() => {
    (async () => {
      let result = await AuthService.getApiAuthMe();

      setUserData(result.data);
      setIsFetched(true);
    })();
  }, []);

  return (
    <>
      {isFetched ? (
        <>
          <div className="panel">
            <div className="panel-name">User Information</div>
            <div className="block">
              <span className="key">Email: </span>
              <span> {userData?.email ? userData.email : ""}</span>
            </div>
            <div className="block">
              <span className="key">Created date: </span>
              <span>
                {new Date(userData?.accountCreatedDate + "Z").toLocaleString()}
              </span>
            </div>
            <div className="block">
              <span className="key">Categories Count: </span>
              <span>{userData?.categoriesCount}</span>
            </div>
            <div className="block">
              <span className="key">Entries Count: </span>
              <span>{userData?.entriesCount}</span>
            </div>
            <div className="block">
              <span className="key">Account Size: </span>
              <span>{getSize(userData?.accountSize)}</span>
            </div>
            <div className="block">
              <span className="key">Roles: </span>
              <span>
                {userData?.roles.map((role, index) => {
                  return <span key={index}>{capitalizeRole(role)} </span>;
                })}
              </span>
            </div>
            <div className="block">
              <span className="key">Avatar Url: </span>
              <span>
                {userData?.avatarUrl ? (
                  <a target="_blank" href={userData?.avatarUrl}>
                    Click here
                  </a>
                ) : (
                  <FaGhost />
                )}
              </span>
            </div>
          </div>
          <div className="panel">
            <div className="panel-name">User Actions</div>
            <NavLink
              to="/account/user/avatar"
              className="mts-button action-button"
            >
              Change Avatar
            </NavLink>
            <NavLink
              to="/account/user/email"
              className="mts-button action-button"
            >
              Change Email
            </NavLink>
            <NavLink
              to="/account/user/username"
              className="mts-button action-button"
            >
              Change Username
            </NavLink>
            <NavLink
              to="/account/user/password"
              className="mts-button action-button"
            >
              Change Password
            </NavLink>
          </div>
        </>
      ) : (
        <Loader />
      )}
    </>
  );
};

export default MePage;
