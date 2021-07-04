import React, { Suspense } from 'react';
import { BrowserRouter, Route, Switch, Link, Redirect, NavLink } from 'react-router-dom';

// components/pages
import BasicLayout from '../core/BasicLayout';
import HomePage from '../pages/HomePage/HomePage';
import TestingPage from '../pages/TestingPage/TestingPage';

export const Routes = () => {
    const basicLayoutRoutes = [
        '/',
        '/Testing'
    ]

    return (
        <Switch>
            <Route path={basicLayoutRoutes} component={BasicLayout}>
                <BasicLayout>
                    <Route exact path="/" component={HomePage} />
                    <Route path="/Testing" component={TestingPage} />
                </BasicLayout>
            </Route>

            <Route path="*" component={fzf} />
        </Switch>
    )
}

// temp
const fzf = () => {
    return (
        <>404</>
    )
}