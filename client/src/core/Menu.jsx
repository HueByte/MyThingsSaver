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
                <NavLink exact to="/" activeClassName="active" className="item">Home</NavLink>
                <NavLink to="/Categories" activeClassName="active" className="item">Categories</NavLink>
                <NavLink to="/Settings" activeClassName="active" className="item">Settings</NavLink>
            </div>
        </div>
    )
}

export default Menu;