import { object } from 'prop-types';
import React, { createContext, useState } from 'react';
import { Redirect } from 'react-router';

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const user = JSON.parse(localStorage.getItem('user'));
    const [authState, setAuthState] = useState(user);

    const setAuthInfo = (userData) => {
        localStorage.setItem('user', JSON.stringify(userData));
        setAuthState(userData);
    }

    const signout = () => {
        localStorage.clear();
        setAuthState({});
        window.location.reload();
    }

    const isAuthenticated = () => {
        if (authState == null || authState == undefined || Object.keys(authState).length === 0 && authState.constructor === object)
            return false
        return true
    }

    const value = {
        authState,
        setAuthState: (authInfo) => setAuthInfo(authInfo),
        signout,
        isAuthenticated
    }

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    )
}

export { AuthContext, AuthProvider }