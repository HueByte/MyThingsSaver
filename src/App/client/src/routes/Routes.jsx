import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";

// other
import PrivateRoute from "./AuthenticatedRoute";
import { Role } from "../api/Roles";

// elements/pages
const HomePage = React.lazy(() => import("../pages/HomePage/HomePage"));
const TestingPage = React.lazy(() =>
  import("../pages/TestingPage/TestingPage")
);
const Settings = React.lazy(() => import("../pages/Settings/Settings"));
const Login = React.lazy(() => import("../pages/Authentication/Login"));
const Register = React.lazy(() => import("../pages/Authentication/Register"));
const Entry = React.lazy(() => import("../pages/Entry/Entry"));
const Logout = React.lazy(() => import("../pages/Logout/Logout"));
const Explorer = React.lazy(() => import("../pages/Explorer/Explorer"));
const UserPage = React.lazy(() => import("../pages/User/User"));
const MePage = React.lazy(() => import("../pages/User/Me"));

const ExplorerContent = React.lazy(() =>
  import("../pages/Explorer/ExplorerContent")
);

const BasicLayout = React.lazy(() =>
  import("../core/BasicLayout/BasicLayout.jsx")
);

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
        <Route index element={<HomePage />} />

        <Route path="explore" element={<Explorer />}>
          <Route path=":categoryId" element={<ExplorerContent />} />
        </Route>

        <Route path="entry/:categoryId/:entryId" element={<Entry />} />

        <Route path="user/*" element={<UserPage />}>
          <Route path="me" element={<MePage />} />
          <Route path="*" element={<Navigate to="me" replace />} />
        </Route>

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

      <Route
        path="logout"
        element={
          <PrivateRoute>
            <Logout />
          </PrivateRoute>
        }
      />
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
