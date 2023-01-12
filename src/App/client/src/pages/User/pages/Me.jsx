import { useEffect, useState } from "react";
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
          <div className="panel user-info">
            <div className="panel-name">User Info</div>
            <div className="info">
              <span className="key">Created date: </span>
              {new Date(userData.accountCreatedDate).toDateString()}
            </div>
            <div className="info">
              <span className="key">Categories Count: </span>
              {userData.categoriesCount}
            </div>
            <div className="info">
              <span className="key">Entries Count: </span>
              {userData.entriesCount}
            </div>
            <div className="info">
              <span className="key">Roles: </span>
              {userData.roles.map((role, index) => {
                return <span key={index}> {capitalizeRole(role)}</span>;
              })}
            </div>
          </div>
          <div className="panel user-actions">
            <div className="panel-name">User Actions</div>
            <div className="mts-button action">Change Email</div>
            <div className="mts-button action">Change Password</div>
            <div className="mts-button action">Change Username</div>
            <div className="mts-button action">Change Avatar</div>
          </div>
        </>
      ) : (
        <Loader />
      )}
    </>
  );
};

export default MePage;
