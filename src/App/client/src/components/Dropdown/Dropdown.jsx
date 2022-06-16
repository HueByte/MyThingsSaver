import React from "react";
import { useState } from "react";
import HamburgerMenu from "../HamburgerMenu/HamburgerMenu";
import "./Dropdown.scss";

const DropdownButton = ({ title, children }) => {
  const [isEnabled, setIsEnabled] = useState(false);

  const toggleMenu = () => {
    setIsEnabled(isEnabled);
  };

  return (
    <div className="dropdown-menu" onClick={toggleMenu}>
      {title ? (
        <div className="button">{title}</div>
      ) : (
        <div className="hamburger">
          <HamburgerMenu size={40} />
        </div>
      )}
      <div className="content">{children}</div>
    </div>
  );
};

export default DropdownButton;
