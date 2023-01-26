import React, { useContext } from "react";
import Menu from "./components/Menu/Menu";
import "./BasicLayout.scss";
import "./BasicLayoutStyles.scss";
import { AuthContext } from "../../contexts/AuthContext";
import { Outlet } from "react-router";
import { CategoryProvider } from "../../contexts/CategoryContext";

const BasicLayout = () => {
  const authContext = useContext(AuthContext);

  return (
    <CategoryProvider>
      <div className="interface__wrapper">
        <Menu />
        <main className={"main__wrapper"}>
          <Outlet />
        </main>
      </div>
    </CategoryProvider>
  );
};

export default BasicLayout;
