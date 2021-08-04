import React, { useContext, useEffect, useRef, useState } from 'react';
import { NavLink, Redirect } from 'react-router-dom';
import './Auth.css';
import '../../core/BasicLayoutStyles.css';
import AuthTemplate from './AuthTemplate';
import { AuthRegister } from '../../auth/Auth';
import { AuthContext } from '../../auth/AuthContext';
import { errorModal, successModal } from '../../core/Modals';

const Register = () => {
    const authContext = useContext(AuthContext);
    const email = useRef();
    const username = useRef();
    const password = useRef();

    useEffect(() => {
        email.current = document.getElementById('email-input');
        username.current = document.getElementById('username-input');
        password.current = document.getElementById('password-input');
    }, [])

    const register = async () => {
        await AuthRegister(email.current.value, username.current.value, password.current.value)
            .then(result => {
                if (result.isSuccess)
                    successModal('You can now log in');
            })
            .catch((errors) => console.error(errors));
    }

    if (authContext.isAuthenticated()) return <Redirect to="/" />
    return (
        // <>
        //     <input id='email-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
        //     <input id='username-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
        //     <input id='password-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
        //     <button onClick={register} style={{ backgroundColor: 'white' }}>Send</button>
        // </>

        <AuthTemplate>
            <div className="auth-welcome">
                <span>Welcome to My things saver!</span><br />
                <span>Register your account here</span>
            </div>
            <div className="auth-input-container">
                <input id='email-input' type="text" className="basic-input auth-input" placeholder="E-mail" autocomplete="email" />
                <input id='username-input' type="text" className="basic-input auth-input" placeholder="Username" autocomplete="username" />
                <input id='password-input' type="password" className="basic-input auth-input" placeholder="Password" autocomplete="current-password" />
            </div>
            <div className="auth-menu">
                <div className="auth-menu-side"></div>
                <div className="auth-menu-side">
                    <div onClick={register} className="basic-button auth-button">Register</div>
                    <NavLink to="/auth/login" className="basic-button auth-button">Log in</NavLink>
                </div>
            </div>
        </AuthTemplate>
    )
}

export default Register;