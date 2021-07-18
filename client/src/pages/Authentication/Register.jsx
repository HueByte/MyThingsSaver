import React, { useContext, useEffect, useRef, useState } from 'react';
import { AuthRegister } from '../../auth/Auth';
import { AuthContext } from '../../auth/AuthContext';

const Register = () => {
    const authContext = useContext(AuthContext);
    const email = useRef();
    const username = useRef();
    const password = useRef();

    useEffect(() => {
        email.current = document.getElementById('email-input');
        username.current = document.getElementById('username-input');
        password.current = document.getElementById('password-input');

        email.current.value = 'Tester@wp.pl';
        username.current.value = 'Tester';
        password.current.value = 'String12';
    }, [])

    const authenticate = async () => {
        console.log(email.current.value, username.current.value, password.current.value)
        var x = await AuthRegister(email.current.value, username.current.value, password.current.value);
        console.log(x);
        // .then(result => {
        //     console.log(result);
        // })
    }

    return (
        <>
            <input id='email-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <input id='username-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <input id='password-input' type="text" style={{ backgroundColor: 'white', width: '100%' }} />
            <button onClick={authenticate} style={{ backgroundColor: 'white' }}>Send</button>
        </>
    )
}

export default Register;