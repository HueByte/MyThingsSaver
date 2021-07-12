import React, { Suspense } from 'react';
import { BrowserRouter, Route, Switch, Link, Redirect, NavLink } from 'react-router-dom';

// components/pages
import BasicLayout from '../core/BasicLayout';
import HomePage from '../pages/HomePage/HomePage';
import TestingPage from '../pages/TestingPage/TestingPage';
import Settings from '../pages/Settings/Settings';
import Categories from '../pages/Categories/Categories';

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
                    <Route path="/Categories" component={Categories} />
                    <Route path="/Settings" component={Settings} />
                </BasicLayout>
            </Route>

            <Route path="*" component={FOUR_ZERO_FOUR} />
        </Switch>
    )
}

// temp
const FOUR_ZERO_FOUR = () => {
    return (
        <div style={{display: 'flex', alignItems: 'center', justifyContent: 'center'}}>404</div>
    )
}