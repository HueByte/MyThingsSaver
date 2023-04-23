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
    <div className="user-panel-container animate__fadeIn animate__animated flex h-full max-h-full justify-center overflow-hidden p-4">
      <div className="flex w-[1024px] flex-row gap-6 lg:mb-4 lg:w-full lg:flex-col lg:items-center md:mb-0">
        <div className="mts-border-gradient-r border-1 flex h-fit w-[250px] shrink-0 flex-col border-2">
          <div className="h-[250px] w-full overflow-hidden rounded-t-xl bg-altBackgroundColor p-2">
            <img
              src={authContext.getAvatar()}
              alt="avatar"
              className="h-full w-full rounded-full"
            />
          </div>
          <abbr
            title={authContext.authState?.username}
            className="bold truncate bg-altBackgroundColor p-2 text-center text-3xl font-bold tracking-wide"
          >
            {authContext.authState?.username}
          </abbr>
          <div className="flex flex-row flex-wrap justify-center gap-4 rounded-b-xl bg-altBackgroundColor p-2">
            {authContext.authState.roles?.map((role, index) => (
              <div
                key={index}
                className="rounded px-2 py-1"
                style={getRoleStyle()}
              >
                {capitalizeRole(role)}
              </div>
            ))}
          </div>
        </div>
        <div className="flex h-full max-h-full w-[calc(1024px_-_250px)] flex-row rounded-xl bg-altBackgroundColor lg:w-3/4 lg:flex-col lg:overflow-hidden md:mb-0 md:w-full md:rounded-none md:pb-8">
          <div className="gap flex h-full w-[192px] flex-col gap-2 border-r-2 border-primary p-4 lg:w-full lg:flex-row lg:justify-center lg:border-r-0">
            <MenuOptions authContext={authContext} />
          </div>
          <div className="m-4 flex w-[calc(100%_-_192px)] flex-col gap-4 overflow-y-auto overflow-x-hidden pt-2 lg:w-full">
            <Outlet />
          </div>
        </div>
      </div>
    </div>
  );
};

const MenuOptions = ({ authContext }) => {
  const options = [
    {
      text: "Me",
      icon: <AiOutlineUser className="mr-2 lg:mr-0" />,
      path: "user/me",
      roles: [Role.User],
    },
    {
      text: "Login logs",
      icon: <HiOutlineClipboard className="mr-2 lg:mr-0" />,
      path: "user/logs",
      roles: [Role.User],
    },
    {
      text: "Admin logs",
      icon: <AiFillFire className="mr-2 lg:mr-0" />,
      path: "admin/logs",
      roles: [Role.User, Role.Admin],
    },
    {
      text: "Users",
      icon: <AiFillCloud className="mr-2 lg:mr-0" />,
      path: "admin/usermanagement",
      roles: [Role.User, Role.Admin],
    },
  ];

  return (
    <>
      {options.map((option, index) => {
        if (!authContext.isInRole(option.roles)) return <></>;
        return (
          <NavLink
            key={index}
            to={option.path}
            activeClassName="active"
            className="item flex items-center rounded bg-backgroundColor p-2 hover:text-primary lg:text-3xl"
          >
            {option.icon}
            <span className="lg:hidden">{option.text}</span>
          </NavLink>
        );
      })}
    </>
  );
};

export default AccountPage;
