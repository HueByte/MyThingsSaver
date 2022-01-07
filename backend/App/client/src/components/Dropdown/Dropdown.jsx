import React from "react";
import { useState } from "react";
import "./Dropdown.css";

const DropdownButton = ({ title, children }) => {
  const [isEnabled, setIsEnabled] = useState(false);

  const toggleMenu = () => {
    setIsEnabled(isEnabled);
  };

  return (
    <div className="dropdown-menu" onClick={toggleMenu}>
      <div className="dropdown-button">{title}</div>
      <div className="dropdown-content">{children}</div>
    </div>
  );
};

export default DropdownButton;
