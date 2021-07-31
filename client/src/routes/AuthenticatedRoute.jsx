import React, { Component, useContext, useEffect } from 'react'
import { Redirect, Route } from 'react-router';
import { AuthContext } from '../auth/AuthContext'

export const PrivateRoute = ({ component: Component, ...rest }) => {
    const authContext = useContext(AuthContext);

    useEffect(() => console.log('Private hit'));
    return (
        <Route {...rest} render={props => {
            // if (!authContext.isAuthenticated()) {
            //     return <Redirect to="/auth/login" />
            // }

            // return <Component {...props} />
            if (authContext.isAuthenticated()) console.log('authenticated');

            return authContext.isAuthenticated() ? <Component {...props} /> : <Redirect to="/auth/login" />
        }} />
    )
}

export default PrivateRoute;