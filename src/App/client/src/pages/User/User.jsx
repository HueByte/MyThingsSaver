import "./User.scss";
import { useContext } from "react";
import { AuthContext } from "../../contexts/AuthContext";
import { Outlet } from "react-router";
import { NavLink } from "react-router-dom";
import DefaultAvatar from "../../assets/DefaultAvatar.png";
import { AiOutlineUser } from "react-icons/ai";

const UserPage = () => {
  const authContext = useContext(AuthContext);
  const colors = [
    { color: "#00fa9a", fontColor: "#000" },
    { color: "#c62368", fontColor: "#FFF" },
    { color: "#7300ff", fontColor: "#FFF" },
  ];

  const getRoleStyle = () => {
    let colorSet = colors[Math.floor(Math.random() * colors.length)];
    return { backgroundColor: colorSet.color, color: colorSet.fontColor };
  };

  const capitalizeRole = (role) => {
    return role.charAt(0).toUpperCase() + role.slice(1);
  };

  return (
    <div className="user-panel-container">
      <div className="user-panel">
        <div className="user-card border-gradient">
          <div className="avatar">
            <img
              src={authContext.authState.avatarUrl ?? DefaultAvatar}
              alt="avatar placeholder"
            />
          </div>
          <div className="username">{authContext.authState.username}</div>
          <div className="badges">
            {authContext.authState.roles?.map((role, index) => (
              <div key={index} className="badge" style={getRoleStyle()}>
                {capitalizeRole(role)}
              </div>
            ))}
          </div>
        </div>
        <div className="content">
          <div className="menu">
            <NavLink to="me" activeClassName="active" className="item">
              <AiOutlineUser /> Me
            </NavLink>
            <NavLink to="temp" activeClassName="active" className="item">
              <AiOutlineUser /> About me
            </NavLink>
            <NavLink to="temp" activeClassName="active" className="item">
              <AiOutlineUser /> About me
            </NavLink>
          </div>
          <div className="content-page">
            <Outlet />
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserPage;
