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
    <div className="user-panel-container flex justify-center">
      <div className="user-panel flex w-[1024px] flex-row gap-6 pt-12">
        <div className="user-card border-gradient border-1 flex h-fit w-[250px] flex-col">
          <div className="avatar h-[250px] w-full overflow-hidden rounded-t-xl">
            <img
              src={authContext.getAvatar()}
              alt="avatar"
              className="h-full w-full rounded-full"
            />
          </div>
          <abbr
            title={authContext.authState?.username}
            className="username ellipsis bold text-center text-4xl font-bold tracking-wide"
          >
            {authContext.authState?.username}
          </abbr>
          <div className="badges flex flex-row flex-wrap justify-center gap-4 rounded-b-xl">
            {authContext.authState.roles?.map((role, index) => (
              <div
                key={index}
                className="badge rounded px-2 py-1"
                style={getRoleStyle()}
              >
                {capitalizeRole(role)}
              </div>
            ))}
          </div>
        </div>
        <div className="content mb-8 flex flex-row rounded-xl bg-altBackgroundColor">
          <div className="menu flex h-full flex-col border-r-2 border-primary p-4">
            <NavLink
              to="user/me"
              activeClassName="active"
              className="item flex items-center rounded bg-backgroundColor p-2 hover:text-primary"
            >
              <AiOutlineUser /> <span>Me</span>
            </NavLink>
            <NavLink
              to="user/logs"
              activeClassName="active"
              className="item flex items-center rounded bg-backgroundColor p-2 hover:text-primary"
            >
              <HiOutlineClipboard /> <span>Login logs</span>
            </NavLink>
            {authContext.isInRole([Role.Admin]) ? (
              <>
                <NavLink
                  to="admin/logs"
                  activeClassName="active"
                  className="item flex items-center rounded bg-backgroundColor p-2 hover:text-primary"
                >
                  <AiFillFire /> <span>Admin logs</span>
                </NavLink>
                <NavLink
                  to="admin/usermanagement"
                  activeClassName="active"
                  className="item flex items-center rounded bg-backgroundColor p-2 hover:text-primary"
                >
                  <AiFillCloud /> <span>Users</span>
                </NavLink>
              </>
            ) : (
              <></>
            )}
          </div>
          <div className="content-page relative w-full p-4">
            <Outlet />
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccountPage;
