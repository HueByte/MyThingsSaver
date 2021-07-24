import React, { useContext } from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import { AuthContext } from '../auth/AuthContext';
import './Menu.css';
import logo from '../assets/CloudByte.svg';

const Menu = () => {
    const authContext = useContext(AuthContext);

    const logout = () => {
        authContext.signout();
    }

    return (
        <div className="nav-top">
            <div className="nav-logo">
                <div className="nav-logo img">
                    <img src={logo} />
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

export default Menu;