import "./User.scss";
import { useContext, useEffect } from "react";
import { AuthContext } from "../../contexts/AuthContext";
import { Outlet } from "react-router";
import { NavLink } from "react-router-dom";
import DefaultAvatar from "../../assets/DefaultAvatar.png";
import { AiOutlineUser } from "react-icons/ai";

const UserPage = () => {
  const authContext = useContext(AuthContext);

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
