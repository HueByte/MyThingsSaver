import React, { useContext, useState } from 'react';
import { NavLink, Redirect, useLocation } from 'react-router-dom';
import { AuthContext } from '../auth/AuthContext';
import './Menu.css';
import './MobileMenu.css';
import logo from '../assets/CloudByteColor.png';
import HamburgerMenu from '../components/HamburgerMenu/HamburgerMenu';

const Menu = () => {
    const authContext = useContext(AuthContext);

    const logout = () => authContext.signout();

    return (
        <>
            {authContext.isAuthenticated() ?
                <>
                    <DesktopMenu logout={logout} authContext={authContext} />
                    <MobileMenu logout={logout} authContext={authContext} />
                </> :
                <Redirect to="/auth/login" />
            }
        </>
    )
}

const DesktopMenu = ({ logout, authContext }) => {
    return (
        <div className="nav-top">
            <div className="nav-logo">
                <div className="nav-logo img">
                    <img src={logo} alt="logo" width="60" height="41" alt="CloudByte logo" />
                </div>
            </div>
            <div className="nav-content__container">
                <div className="nav-content__container left">
                    <NavLink exact to="/" activeClassName="active" className="item">Home</NavLink>
                    <NavLink to="/Categories" activeClassName="active" className="item">Categories</NavLink>
                    <NavLink to="/Settings" activeClassName="active" className="item">Settings</NavLink>
                </div>
                <div className="nav-content__container right">
                    {!(authContext.isAuthenticated()) ?
                        <>
                            <NavLink to="/auth/login" className="right-item">Login</NavLink>
                            <NavLink to="/auth/register" className="right-item">Register</NavLink>
                        </>
                        :
                        <>
                            <div className="right-item user">{authContext.authState?.username}</div>
                            <div className="right-item" onClick={logout}>Log out</div>
                        </>
                    }
                </div>
            </div>
        </div>
    )
}

const MobileMenu = ({ logout, authContext }) => {
    const [isActiveMenu, setIsActiveMenu] = useState(false);

    const toggleMenu = () => {
        setIsActiveMenu(!isActiveMenu);
        document.documentElement.classList.toggle('hide-overflow')
    }

    return (
        <div className={`nav-top-mobile__wrapper${isActiveMenu ? '' : ' hide'}`}>
            <div className="icon">
                <img src={logo} width="60" height="41" alt="CloudByte logo" />
            </div>
            <div className="open" onClick={toggleMenu}>
                <HamburgerMenu shouldClose={isActiveMenu} />
            </div>
            <div className="menu">
                <NavLink onClick={toggleMenu} exact to="/" activeClassName="active" className="item">Home</NavLink>
                <NavLink onClick={toggleMenu} to="/Categories" activeClassName="active" className="item">Categories</NavLink>
                <NavLink onClick={toggleMenu} to="/Settings" activeClassName="active" className="item">Settings</NavLink>
                <div className="item" onClick={logout}>Log out</div>
            </div>
        </div>
    )
}

export default Menu;