import React, { useContext, useEffect } from "react";
import Menu from "./components/Menu/Menu";
import SideMenu from "./components/SideMenu/SideMenu";
import "./BasicLayout.scss";
import "./BasicLayoutStyles.scss";
import "../OverrideDefaultMD.css";
import { AuthContext } from "../../auth/AuthContext";
import { Navigate, Outlet } from "react-router";
import KUTE from "kute.js";
import Wave from "./components/Wave";
import { useState } from "react";
import { CategoryProvider } from "../../contexts/CategoryContext";

const BasicLayout = () => {
  const authContext = useContext(AuthContext);
  const [isEnabled, setIsEnabled] = useState(false);

  useEffect(() => {
    if (!authContext.isAuthenticated()) return;
  }, []);

  if (authContext.isAuthenticated())
    return (
      <CategoryProvider>
        <div className="interface__wrapper">
          <Menu />
          {/* <SideMenu isEnabled={isEnabled} setIsEnabled={setIsEnabled} /> */}
          <main className={`main__wrapper ${isEnabled ? "" : "expanded"}`}>
            <Outlet />
          </main>
          <Wave />
        </div>
      </CategoryProvider>
    );
  else return <Navigate to="/auth/login" />;
};

export default BasicLayout;
