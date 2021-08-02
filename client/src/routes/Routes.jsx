import React, { Suspense } from 'react';
import { BrowserRouter, Route, Switch, Link, Redirect, NavLink } from 'react-router-dom';

// components/pages
import BasicLayout from '../core/BasicLayout';
import HomePage from '../pages/HomePage/HomePage';
import TestingPage from '../pages/TestingPage/TestingPage';
import Settings from '../pages/Settings/Settings';
import Categories from '../pages/Categories/Categories';
import Category from '../pages/Category/Category';
import Login from '../pages/Authentication/Login';
import Register from '../pages/Authentication/Register';

// other
import PrivateRoute from './AuthenticatedRoute';
import Entry from '../pages/Entry/Entry';

export const Routes = () => {
    const basicLayoutRoutes = [
        '/',
        '/Testing',
        '/category/:id',
        '/categories',
        '/Settings',
        '/entry/:categoryId/:entryId'
    ]

    return (
        <Switch>
            <Route path="/auth/login" component={Login} />
            <Route path="/auth/register" component={Register} />

            <PrivateRoute path={basicLayoutRoutes} component={BasicLayout}>
                <BasicLayout>
                    <PrivateRoute exact path="/" component={HomePage} />
                    <PrivateRoute path="/category/:id" component={Category} />
                    <PrivateRoute path="/categories" component={Categories} />
                    <PrivateRoute path="/Testing" component={TestingPage} />
                    <PrivateRoute path="/Settings" component={Settings} />
                    <PrivateRoute path="/entry/:categoryId/:entryId" component={Entry} />
                </BasicLayout>
            </PrivateRoute>

            <Route path="*" component={FOUR_ZERO_FOUR} />
        </Switch>
    )
}

// temp
const FOUR_ZERO_FOUR = () => {
    return (
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>404</div>
    )
}