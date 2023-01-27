import "./User.scss";
import { useContext } from "react";
import { AuthContext } from "../../contexts/AuthContext";
import { Outlet } from "react-router";
import { NavLink } from "react-router-dom";
import { AiFillCloud, AiFillFire, AiOutlineUser } from "react-icons/ai";
import { HiOutlineClipboard } from "react-icons/hi";
import { Role } from "../../api/Roles";
import { capitalizeRole } from "../../core/Lib";

const AccountPage = () => {
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

  return (
    <div className="user-panel-container">
      <div className="user-panel">
        <div className="user-card border-gradient">
          <div className="avatar">
            <img src={authContext.getAvatar()} alt="avatar" />
          </div>
          <abbr
            title={authContext.authState?.username}
            className="username ellipsis"
          >
            {authContext.authState?.username}
          </abbr>
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
            <NavLink to="user/me" activeClassName="active" className="item">
              <AiOutlineUser /> Me
            </NavLink>
            <NavLink to="user/logs" activeClassName="active" className="item">
              <HiOutlineClipboard /> Login logs
            </NavLink>
            {authContext.isInRole([Role.Admin]) ? (
              <>
                <NavLink
                  to="admin/logs"
                  activeClassName="active"
                  className="item"
                >
                  <AiFillFire /> Admin logs
                </NavLink>
                <NavLink
                  to="admin/usermanagement"
                  activeClassName="active"
                  className="item"
                >
                  <AiFillCloud /> Users
                </NavLink>
              </>
            ) : (
              <></>
            )}
          </div>
          <div className="content-page">
            <Outlet />
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccountPage;
