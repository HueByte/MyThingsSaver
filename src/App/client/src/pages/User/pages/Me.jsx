import { useEffect, useState } from "react";
import { FaGhost } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import { AuthService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
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

  const capitalizeRole = (role) => {
    return role.charAt(0).toUpperCase() + role.slice(1);
  };

  const getSize = (size) => {
    if (size < 1024) return size + " B";
    else if (size < 1024 * 1024) return (size / 1024).toFixed(2) + " KB";
    else if (size < 1024 * 1024 * 1024)
      return (size / 1024 / 1024).toFixed(2) + " MB";
    else return (size / 1024 / 1024 / 1024).toFixed(2) + " GB";
  };

  return (
    <>
      {isFetched ? (
        <>
          <div className="panel">
            <div className="panel-name">User Information</div>
            <div className="block">
              <span className="key">Email: </span>
              {userData?.email ? userData.email : ""}
            </div>
            <div className="block">
              <span className="key">Created date: </span>
              {new Date(userData?.accountCreatedDate).toDateString()}
            </div>
            <div className="block">
              <span className="key">Categories Count: </span>
              {userData?.categoriesCount}
            </div>
            <div className="block">
              <span className="key">Entries Count: </span>
              {userData?.entriesCount}
            </div>
            <div className="block">
              <span className="key">Account Size: </span>
              {getSize(userData?.accountSize)}
            </div>
            <div className="block">
              <span className="key">Roles: </span>
              {userData?.roles.map((role, index) => {
                return <span key={index}>{capitalizeRole(role)} </span>;
              })}
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
            <NavLink to="/user/avatar" className="mts-button action-button">
              Change Avatar
            </NavLink>
            <NavLink to="/user/email" className="mts-button action-button">
              Change Email
            </NavLink>
            <NavLink to="/user/username" className="mts-button action-button">
              Change Username
            </NavLink>
            <NavLink to="/user/password" className="mts-button action-button">
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
