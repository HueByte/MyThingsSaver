import { useEffect, useState, useContext } from "react";
import { AdminService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import { AuthContext } from "../../../contexts/AuthContext";
import "./UserManagement.scss";
import DefaultAvatar from "../../../assets/DefaultAvatar.png";

const UserManagementPage = () => {
  const authContext = useContext(AuthContext);
  const [users, setUsers] = useState([{}]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    (async () => {
      let result = await AdminService.getApiAdminManagementUsers();
      setUsers(result.data);
      setIsLoading(false);
    })();
  }, []);

  const colors = [
    { color: "#00fa9a", fontColor: "#000" },
    { color: "#c62368", fontColor: "#FFF" },
    { color: "#7300ff", fontColor: "#FFF" },
  ];

  const getRoleStyle = () => {
    let colorSet = colors[Math.floor(Math.random() * colors.length)];
    return { backgroundColor: colorSet.color, color: colorSet.fontColor };
  };

  const getSize = (size) => {
    if (size < 1024) return size + " B";
    else if (size < 1024 * 1024) return (size / 1024).toFixed(2) + " KB";
    else if (size < 1024 * 1024 * 1024)
      return (size / 1024 / 1024).toFixed(2) + " MB";
    else return (size / 1024 / 1024 / 1024).toFixed(2) + " GB";
  };

  return (
    <div className="user-management-container">
      {isLoading ? (
        <div>
          <Loader />
        </div>
      ) : (
        <>
          {users?.map((user) => (
            <div key={user.id} className="user">
              <div className="avatar">
                <img src={user.avatarUrl ?? DefaultAvatar} alt="avatar" />
              </div>
              <div className="user-info">
                <div className="row">
                  <div className="username">{user.username}</div>
                </div>
                <div className="row">
                  <abbr
                    title={user.email ?? "No Email"}
                    className="account-detail-long ellipsis"
                  >
                    {user.email ?? "No Email"}
                  </abbr>
                  <div className="account-detail account-size">
                    {getSize(user.accountSize)}
                  </div>
                  <div className="account-detail">
                    {new Date(
                      user.accountCreatedDate + "Z"
                    ).toLocaleDateString()}
                  </div>
                </div>
                <div className="row">
                  <div className="badges">
                    {user.roles?.length ? (
                      user.roles?.map((role) => (
                        <div className="badge" style={getRoleStyle()}>
                          {role}
                        </div>
                      ))
                    ) : (
                      <div className="badge" style={getRoleStyle()}>
                        None
                      </div>
                    )}
                  </div>
                </div>
              </div>
            </div>
          ))}
        </>
      )}
    </div>
  );
};

export default UserManagementPage;