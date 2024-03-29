import React, { useContext } from "react";
import { Routes, Route, Navigate, Outlet } from "react-router-dom";

// other
import PrivateRoute from "./AuthenticatedRoute";
import { Role } from "../api/Roles";
import { AuthContext } from "../contexts/AuthContext";

// elements/pages
const HomePage = React.lazy(() => import("../pages/HomePage/HomePage"));
const TestingPage = React.lazy(() =>
  import("../pages/TestingPage/TestingPage")
);
const UserModerationPage = React.lazy(() =>
  import("../pages/User/pages/UserModeration")
);
const Settings = React.lazy(() => import("../pages/Settings/Settings"));
const Login = React.lazy(() => import("../pages/Authentication/Login"));
const LegalNoticePage = React.lazy(() =>
  import("../pages/Policies/LegalNotice")
);
const Register = React.lazy(() => import("../pages/Authentication/Register"));
const Entry = React.lazy(() => import("../pages/Entry/Entry"));
const PublicEntryPage = React.lazy(() => import("../pages/Entry/PublicEntry"));
const Logout = React.lazy(() => import("../pages/Logout/Logout"));
const Explorer = React.lazy(() => import("../pages/Explorer/Explorer"));
const Account = React.lazy(() => import("../pages/User/Account"));
const MePage = React.lazy(() => import("../pages/User/pages/Me"));
const LoginLogsPage = React.lazy(() => import("../pages/User/pages/LoginLogs"));
const LoginLogsPaginatorPage = React.lazy(() =>
  import("../pages/User/components/LoginLogsPaginator")
);
const UserManagementPage = React.lazy(() =>
  import("../pages/User/pages/UserManagement")
);
const ChangeEmailPage = React.lazy(() =>
  import("../pages/User/pages/ChangeEmail")
);
const ChangePasswordPage = React.lazy(() =>
  import("../pages/User/pages/ChangePassword")
);
const ChangeUsernamePage = React.lazy(() =>
  import("../pages/User/pages/ChangeUsername")
);
const ChangeAvatarPage = React.lazy(() =>
  import("../pages/User/pages/ChangeAvatar")
);
const ExplorerContent = React.lazy(() =>
  import("../pages/Explorer/ExplorerContent")
);
const BasicLayout = React.lazy(() =>
  import("../core/BasicLayout/BasicLayout.jsx")
);

export const ClientRouter = () => {
  const authContext = useContext(AuthContext);

  return (
    <Routes>
      <Route path="auth/login" element={<Login />} />

      <Route path="auth/register" element={<Register />} />

      <Route path="LegalNotice" element={<LegalNoticePage />} />

      {!authContext.isAuthenticated() ? (
        <Route
          path="public/:link"
          element={<PublicEntryPage isLogged={false} />}
        />
      ) : (
        <></>
      )}

      <Route
        path="/"
        element={
          <PrivateRoute source="BasicLayout">
            <BasicLayout />
          </PrivateRoute>
        }
      >
        <Route index element={<HomePage />} />

        <Route path="explore" element={<Explorer />}>
          <Route path=":categoryId" element={<ExplorerContent />} />
        </Route>

        <Route path="entry/:categoryId/:entryId" element={<Entry />} />

        <Route
          path="testing"
          element={
            <PrivateRoute roles={[Role.Admin]}>
              <TestingPage />
            </PrivateRoute>
          }
        />

        <Route path="account/*" element={<Account />}>
          <Route path="user/*" element={<Outlet />}>
            <Route path="me" element={<MePage />} />
            <Route path="avatar" element={<ChangeAvatarPage />} />
            <Route path="email" element={<ChangeEmailPage />} />
            <Route path="username" element={<ChangeUsernamePage />} />
            <Route path="password" element={<ChangePasswordPage />} />
            <Route path="logs/*" element={<LoginLogsPage />}>
              <Route path=":page" element={<LoginLogsPaginatorPage />} />
              <Route path="*" element={<Navigate to="1" replace />} />
            </Route>
            <Route path="*" element={<Navigate to="me" replace />} />
          </Route>

          <Route
            path="admin/*"
            element={
              <PrivateRoute roles={[Role.Admin]} source="AdminPage">
                <Outlet isAdmin={true} />
              </PrivateRoute>
            }
          >
            <Route path="logs/*" element={<LoginLogsPage isAdmin={true} />}>
              <Route
                path=":page"
                element={<LoginLogsPaginatorPage isAdmin={true} />}
              />
              <Route path="*" element={<Navigate to="1" replace />} />
            </Route>
            <Route path="usermanagement" element={<UserManagementPage />} />
            <Route
              path="*"
              element={<Navigate to="usermanagement" replace />}
            />
            <Route path="usermod/:username" element={<UserModerationPage />} />
          </Route>

          <Route path="*" element={<Navigate to="user/me" replace />} />
        </Route>

        <Route
          path="/settings"
          element={
            <PrivateRoute roles={[Role.Admin]}>
              <Settings />
            </PrivateRoute>
          }
        />

        <Route path="public/:link" element={<PublicEntryPage />} />

        <Route path="*" element={<FOUR_ZERO_FOUR />} />
      </Route>

      <Route
        path="logout"
        element={
          <PrivateRoute source="BasicLayout">
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
