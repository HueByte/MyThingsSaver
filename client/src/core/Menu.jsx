import React from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import './Menu.css';
import logo from '../assets/CloudByte.svg';

const Menu = () => {
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
                    <NavLink to="/auth/login" className="right-item">Login</NavLink>
                    <NavLink to="/auth/register" className="right-item">Register</NavLink>
                </div>
            </div>
        </div>
    )
}

export default Menu;