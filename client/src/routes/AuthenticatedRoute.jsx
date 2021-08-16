import React, { Component, useContext, useEffect } from 'react'
import { Redirect, Route } from 'react-router';
import { AuthContext } from '../auth/AuthContext'

export const PrivateRoute = ({ component: Component, roles, ...rest }) => {
    const authContext = useContext(AuthContext);
    return (
        <Route {...rest} render={props => {
            if (!(authContext.isAuthenticated()))
                return <Redirect to="/auth/login" />

            if (roles && roles.indexOf(authContext.authState.roles) === -1)
                return <Redirect to="/" />

            return <Component {...props} />
        }} />
    )
}

export default PrivateRoute;