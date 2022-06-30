import React from "react";
import { useState } from "react";
import "./Dropdown.scss";
import { FaEllipsisV } from "react-icons/fa";

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
        <div className="drop-icon">
          <FaEllipsisV />
        </div>
      )}
      <div className="content">{children}</div>
    </div>
  );
};

export default DropdownButton;
