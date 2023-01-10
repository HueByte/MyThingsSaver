import { useContext, useState } from "react";
import { NavLink } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
import "./Menu.scss";
import "./MobileMenu.scss";
import logo from "../../../../assets/CloudByteColor.png";
import HamburgerMenu from "../../../../components/HamburgerMenu/HamburgerMenu";
import { Role } from "../../../../api/Roles";

const Menu = () => {
  const authContext = useContext(AuthContext);

  const logout = () => authContext.signout();

  return (
    <>
      <DesktopMenu logout={logout} authContext={authContext} />
      <MobileMenu logout={logout} authContext={authContext} />
    </>
  );
};

const DesktopMenu = ({ logout, authContext }) => {
  return (
    <div className="nav-top border-gradient">
      <div className="logo">
        <div className="img">
          <img src={logo} width="60" height="40" alt="CloudByte logo" />
        </div>
      </div>
      <div className="content">
        <div className="left">
          <NavLink exact to="/" activeClassName="active" className="item">
            Home
          </NavLink>
          <NavLink to="/explore" activeClassName="active" className="item">
            Explorer
          </NavLink>
          {authContext.authState?.roles?.find((role) => role == Role.Admin) ? (
            <NavLink to="/Settings" activeClassName="active" className="item">
              Settings
            </NavLink>
          ) : (
            <></>
          )}
        </div>
        <div className="right">
          <div className="item user">{authContext.authState?.username}</div>
          <div className="item" onClick={logout}>
            Log out
          </div>
        </div>
      </div>
    </div>
  );
};

const MobileMenu = ({ logout, authContext }) => {
  const [isActiveMenu, setIsActiveMenu] = useState(false);

  const toggleMenu = () => {
    setIsActiveMenu(!isActiveMenu);
    document.documentElement.classList.toggle("hide-overflow");
  };

  return (
    <div className={`nav-top-mobile${isActiveMenu ? "" : " mobile-hide"}`}>
      <div className="icon">
        <img src={logo} width="60" height="41" alt="CloudByte logo" />
      </div>
      <div className="open" onClick={toggleMenu}>
        <HamburgerMenu shouldClose={isActiveMenu} />
      </div>
      <div className="menu">
        <NavLink
          onClick={toggleMenu}
          exact
          to="/"
          activeClassName="active"
          className="item"
        >
          Home
        </NavLink>
        <NavLink
          onClick={toggleMenu}
          to="/explore"
          activeClassName="active"
          className="item"
        >
          Explorer
        </NavLink>
        {authContext.authState?.roles?.find((role) => role == Role.Admin) ? (
          <NavLink
            onClick={toggleMenu}
            to="/Settings"
            activeClassName="active"
            className="item"
          >
            Settings
          </NavLink>
        ) : (
          <></>
        )}
        <div className="item" onClick={logout}>
          Log out
        </div>
      </div>
    </div>
  );
};

export default Menu;
