import React, { Suspense } from "react";
import { Route, Switch } from "react-router-dom";

// components/pages
import BasicLayout from "../core/BasicLayout";
import HomePage from "../pages/HomePage/HomePage";
import TestingPage from "../pages/TestingPage/TestingPage";
import Settings from "../pages/Settings/Settings";
import Categories from "../pages/Categories/Categories";
import Category from "../pages/Category/Category";
import Login from "../pages/Authentication/Login";
import Register from "../pages/Authentication/Register";
import Entry from "../pages/Entry/Entry";
import Logout from "../pages/Logout/Logout";

// other
import PrivateRoute from "./AuthenticatedRoute";
import { CategoryProvider } from "../contexts/CategoryContext";
import { Role } from "../api/Roles";

export const Routes = () => {
  const basicLayoutRoutes = [
    "/",
    "/Testing",
    "/category/:categoryId/:entryId",
    "/categories",
    "/Settings",
    "/entry/:categoryId/:subCategoryId?/:entryId",
    "/logout",
  ];

  return (
    <Switch>
      <Route path="/auth/login" component={Login} />
      <Route path="/auth/register" component={Register} />

      <PrivateRoute path={basicLayoutRoutes} component={BasicLayout}>
        <CategoryProvider>
          <BasicLayout>
            <Switch>
              <PrivateRoute exact path="/" component={HomePage} />
              <PrivateRoute
                path="/category/:categoryId/:subCategoryId?/:entryId"
                component={Category}
              />
              <PrivateRoute path="/categories" component={Categories} />
              <PrivateRoute path="/Testing" component={TestingPage} />
              <PrivateRoute
                roles={Role.Admin}
                path="/Settings"
                component={Settings}
              />
              <PrivateRoute
                path="/entry/:categoryId/:subCategoryId?/:entryId"
                component={Entry}
              />
              <PrivateRoute path="/logout" component={Logout} />
              <PrivateRoute component={FOUR_ZERO_FOUR} />
            </Switch>
          </BasicLayout>
        </CategoryProvider>
      </PrivateRoute>
    </Switch>
  );
};

// temp
const FOUR_ZERO_FOUR = () => {
  return (
    <div
      style={{
        height: "100%",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <h2 style={{ textAlign: "center" }}>
        Couldn't find anything
        <br />
        404
      </h2>
    </div>
  );
};
