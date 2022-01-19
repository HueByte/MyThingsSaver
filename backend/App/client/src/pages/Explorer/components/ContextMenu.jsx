import React from "react";
import { useState } from "react";
import { useRef } from "react";
import { useEffect } from "react";
import { useNavigate } from "react-router";
import { NavLink } from "react-router-dom";
import CategoryAdd from "./CategoryAdd";
import "./ContextMenu.scss";

const ContextMenu = ({ category }) => {
  const [isAddActive, setIsAddActive] = useState(false);

  const contextMenu = useRef();
  const contextScope = useRef();
  const bodyScope = useRef();
  const navigate = useNavigate();

  useEffect(() => {
    contextMenu.current = document.getElementById("context-menu");
    contextScope.current = document.getElementById("explorer-menu");
    bodyScope.current = document.querySelector("body");

    contextScope.current.addEventListener("contextmenu", onContextMenu);
    bodyScope.current.addEventListener("click", onBodyClick);

    return () => {
      contextScope.current.removeEventListener("contextmenu", onContextMenu);
      bodyScope.current.removeEventListener("click", onBodyClick);
    };
  }, []);

  const onContextMenu = (event) => {
    event.preventDefault();

    const { clientX: mouseX, clientY: mouseY } = event;

    contextMenu.current.style.top = `${mouseY}px`;
    contextMenu.current.style.left = `${mouseX}px`;

    contextMenu.current.classList.add("visible");
  };

  const onBodyClick = (event) => {
    if (event.target.offsetParent != contextMenu.current) {
      contextMenu.current.classList.remove("visible");
    }
  };

  const Open = () => {
    navigate(`/explore/${category.categoryId}`);
  };

  const invokeAdd = () => {
    setIsAddActive(true);
  };

  const Delete = () => {};

  const Edit = () => {};

  return (
    <>
      <div id="context-menu">
        {category ? (
          <>
            <div className="item ellipsis" onClick={Open}>
              {category.name}
            </div>

            <NavLink
              to={`/explore/${category.categoryId}`}
              className="item"
              target="_blank"
            >
              Open in new tab
            </NavLink>
            <div className="item" onClick={invokeAdd}>
              Add
            </div>
            <div className="item">Remove</div>
          </>
        ) : (
          <div className="item" onClick={invokeAdd}>
            Add New
          </div>
        )}
      </div>
      <CategoryAdd
        isActive={isAddActive}
        setIsActive={setIsAddActive}
        parentCategory={category}
      />
    </>
  );
};

export default ContextMenu;
