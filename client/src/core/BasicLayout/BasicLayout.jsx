import React, { useContext, useEffect } from "react";
import Menu from "./components/Menu/Menu";
import SideMenu from "./components/SideMenu/SideMenu";
import "./BasicLayout.css";
import "./BasicLayoutStyles.css";
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

    const tweek1 = KUTE.fromTo(
      "#p2",
      { path: "#p2" },
      { path: "#pp2" },
      { repeat: 999, duration: 20000, yoyo: true }
    );

    const tweek2 = KUTE.fromTo(
      "#p3",
      { path: "#p3" },
      { path: "#pp3" },
      { repeat: 999, duration: 20000, yoyo: true }
    );

    tweek1.start();
    tweek2.start();
  }, []);

  if (authContext.isAuthenticated())
    return (
      <div className="interface__wrapper">
        <Menu />
        <SideMenu isEnabled={isEnabled} setIsEnabled={setIsEnabled} />
        <main className={`main__wrapper ${isEnabled ? "" : "expanded"}`}>
          <Outlet />
        </main>
        <Wave />
      </div>
    );
  else return <Navigate to="/auth/login" />;
};

export default BasicLayout;
