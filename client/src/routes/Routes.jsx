import React, { Suspense } from 'react';
import { BrowserRouter, Route, Switch, Link, Redirect, NavLink } from 'react-router-dom';

// components/pages
import HomePage from '../pages/HomePage/HomePage';


export const Routes = () => {
    return(
        <Switch>
            <Route path="/" component={HomePage} />
        </Switch>
    )
}