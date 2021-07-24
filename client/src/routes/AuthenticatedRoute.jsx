import React, { Component, useContext } from 'react'
import { Redirect, Route } from 'react-router';
import { AuthContext } from '../auth/AuthContext'

export const PrivateRoute = ({ component: Component, ...rest }) => {
    const authContext = useContext(AuthContext);

    return (
        <Route {...rest} render={props => {
            if (!authContext.isAuthenticated()) {
                return <Redirect to="/auth/login" />
            }

            return <Component {...props} />
        }} />
    )
}

export default PrivateRoute;