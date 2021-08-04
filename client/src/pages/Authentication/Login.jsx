import React, { useContext, useEffect, useRef, useState } from 'react';
import { NavLink, Redirect } from 'react-router-dom';
import { AuthLogin } from '../../auth/Auth';
import { AuthContext } from '../../auth/AuthContext';
import './Auth.css';
import '../../core/BasicLayoutStyles.css';
import AuthTemplate from './AuthTemplate';
import { warningModal } from '../../core/Modals';

const Login = () => {
    const authContext = useContext(AuthContext);
    const [isWorking, setIsWorking] = useState(false);
    const username = useRef();
    const password = useRef();

    useEffect(() => {
        username.current = document.getElementById('username-input');
        password.current = document.getElementById('password-input');
    }, [])

    const authenticate = async () => {
        if (username.current.value.length === 0 || password.current.value.length === 0) {
            warningModal('Please fill all of the fields');
            setIsWorking(false);
            return;
        }

        await AuthLogin(username.current.value, password.current.value)
            .then(result => {
                if (result.isSuccess)
                    authContext.setAuthState(result.data);
            })
            .catch((errors) => {
                console.log(errors);
            });

        setIsWorking(false);
    }

    if (authContext.isAuthenticated()) return <Redirect to="/" />
    return (
        <AuthTemplate isWorking={isWorking}>
            <div className="auth-welcome">Welcome to My things saver!</div>
            <div className="auth-input-container">
                <input id='username-input' type="text" className="basic-input auth-input" placeholder="Username" autocomplete="username" />
                <input id='password-input' type="password" className="basic-input auth-input" placeholder="Password" autocomplete="current-password" />
            </div>
            <div className="auth-menu">
                <div className="auth-menu-side">
                    <NavLink to="/HelpMe" className="auth-button-help">Can't log in?</NavLink>
                </div>
                <div className="auth-menu-side">
                    <div onClick={authenticate} className="basic-button auth-button">Log in</div>
                    <NavLink to="/auth/register" className="basic-button auth-button">Register</NavLink>
                </div>
            </div>
        </AuthTemplate >
    )
}

export default Login;