import React, { useContext } from "react";
import Menu from "./components/Menu/Menu";
import "./BasicLayout.scss";
import "./BasicLayoutStyles.scss";
import { AuthContext } from "../../contexts/AuthContext";
import { Navigate, Outlet } from "react-router";
// import Wave from "./components/Wave/Wave";
import { CategoryProvider } from "../../contexts/CategoryContext";

const BasicLayout = () => {
  const authContext = useContext(AuthContext);

  if (authContext.isAuthenticated())
    return (
      <CategoryProvider>
        <div className="interface__wrapper">
          <Menu />
          <main className={"main__wrapper"}>
            <Outlet />
          </main>
          {/* <Wave /> */}
        </div>
      </CategoryProvider>
    );
  else return <Navigate to="/auth/login" />;
};

export default BasicLayout;
