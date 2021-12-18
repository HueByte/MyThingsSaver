import React, { Suspense } from "react";
import { Routes, Route, Switch } from "react-router-dom";

// elements/pages
import BasicLayout from "../core/BasicLayout/BasicLayout.jsx";
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

export const ClientRouter = () => {
  return (
    <Routes>
      <Route path="auth/login" element={<Login />} />
      <Route path="auth/register" element={<Register />} />

      <Route
        path="/"
        element={
          <PrivateRoute>
            <BasicLayout />
          </PrivateRoute>
        }
      >
        <Route
          index
          element={
            <PrivateRoute>
              <HomePage />
            </PrivateRoute>
          }
        />
        <Route
          path="/categories"
          element={
            <PrivateRoute>
              <Categories />
            </PrivateRoute>
          }
        />
        <Route path="category">
          <Route
            path=":categoryName/:subCategoryName/:fetchCategoryId"
            element={
              <PrivateRoute>
                <Category />
              </PrivateRoute>
            }
          />
          <Route
            path=":categoryName/:fetchCategoryId"
            element={
              <PrivateRoute>
                <Category />
              </PrivateRoute>
            }
          />
        </Route>
        <Route
          path="logout"
          element={
            <PrivateRoute>
              <Logout />
            </PrivateRoute>
          }
        />
        <Route
          path="/settings"
          element={
            <PrivateRoute roles={Role.Admin}>
              <Settings />
            </PrivateRoute>
          }
        />
        <Route path="*" element={<FOUR_ZERO_FOUR />} />
      </Route>
    </Routes>
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
      <h2 style={{ textAlign: "center", color: "#FFF" }}>
        Couldn't find anything
        <br />
        404
      </h2>
    </div>
  );
};
