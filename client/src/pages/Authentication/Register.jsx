import React, { useContext, useEffect, useRef, useState } from 'react';
import { Redirect } from 'react-router-dom';
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
                successModal('You can now log in');
            })
            .catch((errors) => {
                console.log('')
            });
    }

    if (authContext.isAuthenticated()) return <Redirect to="/" />
    return (
        <>
            <input id='email-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <input id='username-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <input id='password-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <button onClick={register} style={{ backgroundColor: 'white' }}>Send</button>
        </>
    )
}

export default Register;