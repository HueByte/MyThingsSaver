import React, { Component, useContext, useEffect } from 'react'
import { Redirect, Route } from 'react-router';
import { AuthContext } from '../auth/AuthContext'

export const PrivateRoute = ({ component: Component, ...rest }) => {
    const authContext = useContext(AuthContext);
    return (
        <Route {...rest} render={props => {
            return authContext.isAuthenticated() ? <Component {...props} /> : <Redirect to="/auth/login" />
        }} />
    )
}

export default PrivateRoute;