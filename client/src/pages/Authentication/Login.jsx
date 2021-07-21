import React, { useContext, useEffect, useRef } from 'react';
import { Redirect } from 'react-router-dom';
import { AuthLogin } from '../../auth/Auth';
import { AuthContext } from '../../auth/AuthContext';

const Login = () => {
    const authContext = useContext(AuthContext);
    const username = useRef();
    const password = useRef();

    useEffect(() => {
        username.current = document.getElementById('username-input');
        password.current = document.getElementById('password-input');
    }, [])

    const authenticate = async () => {
        await AuthLogin(username.current.value, password.current.value)
            .then(result => {
                console.log(result);
                authContext.setAuthState(result.data);
            })
            .catch((errors) => {
                console.log(errors);
            });
    }

    if (authContext.isAuthenticated()) return <Redirect to="/" />
    return (
        <>
            <input id='username-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <input id='password-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <button onClick={authenticate} style={{ backgroundColor: 'white' }}>Send</button>
        </>
    )
}

export default Login;