import { useEffect, useState } from "react";
import { FaGhost } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import { UsersService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import "../User.scss";

const MePage = () => {
  const [userData, setUserData] = useState({});
  const [isFetched, setIsFetched] = useState(false);

  useEffect(() => {
    (async () => {
      let result = await UsersService.getApiUsersMe();

      setUserData(result.data);
      setIsFetched(true);
    })();
  }, []);

  const capitalizeRole = (role) => {
    return role.charAt(0).toUpperCase() + role.slice(1);
  };

  return (
    <>
      {isFetched ? (
        <>
          <div className="panel">
            <div className="panel-name">User Information</div>
            <div className="block">
              <span className="key">Created date: </span>
              {new Date(userData.accountCreatedDate).toDateString()}
            </div>
            <div className="block">
              <span className="key">Categories Count: </span>
              {userData.categoriesCount}
            </div>
            <div className="block">
              <span className="key">Entries Count: </span>
              {userData.entriesCount}
            </div>
            <div className="block">
              <span className="key">Roles: </span>
              {userData.roles.map((role, index) => {
                return <span key={index}>{capitalizeRole(role)} </span>;
              })}
            </div>
            <div className="block">
              <span className="key">Avatar Url: </span>
              <span>
                {userData.avatarUrl ? (
                  <a target="_blank" href={userData.avatarUrl}>
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
            <div className="mts-button action-button">Change Email</div>
            <div className="mts-button action-button">Change Password</div>
            <div className="mts-button action-button">Change Username</div>
            <NavLink to="/user/avatar" className="mts-button action-button">
              Change Avatar
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
