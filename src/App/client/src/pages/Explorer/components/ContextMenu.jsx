import React from "react";
import { useState } from "react";
import { useRef } from "react";
import { useEffect } from "react";
import { useNavigate } from "react-router";
import { NavLink } from "react-router-dom";
import CategoryAdd from "./CategoryAdd";
import CategoryRemove from "./CategoryRemove";
import CategoryUpdate from "./CategoryUpdate";
import "./ContextMenu.scss";

const ContextMenu = ({ category }) => {
  const [isAddActive, setIsAddActive] = useState(false);
  const [isRemoveActive, setIsRemoveActive] = useState(false);
  const [isEditActive, setIsEditActive] = useState(false);

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
    if (
      contextMenu.current.style.top != "unset" ||
      contextMenu.current.style.bottom != "unset"
    ) {
      contextMenu.current.style.top = "unset";
      contextMenu.current.style.bottom = `unset`;
    }

    event.preventDefault();

    const { clientX: mouseX, clientY: mouseY } = event;
    let isDown =
      window.innerHeight - (mouseY + contextMenu.current.offsetHeight) > 0;

    isDown
      ? (contextMenu.current.style.top = `${mouseY}px`)
      : (contextMenu.current.style.bottom = `${window.innerHeight - mouseY}px`);

    contextMenu.current.style.left = `${mouseX}px`;

    contextMenu.current.classList.add("visible");
  };

  const onBodyClick = (event) => {
    if (event.target.offsetParent != contextMenu.current) {
      contextMenu.current.classList.remove("visible");
    }
  };

  // commands
  const Open = () => {
    navigate(`/explore/${category.categoryId}`);
    contextMenu.current.classList.remove("visible");
  };

  const invokeAdd = () => {
    setIsAddActive(true);
    contextMenu.current.classList.remove("visible");
  };

  const invokeRemove = () => {
    setIsRemoveActive(true);
    contextMenu.current.classList.remove("visible");
  };

  const invokeEdit = () => {
    setIsEditActive(true);
    contextMenu.current.classList.remove("visible");
  };

  return (
    <>
      <div id="context-menu">
        {category ? (
          <>
            <div className="item title ellipsis" onClick={Open}>
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
            <div className="item" onClick={invokeRemove}>
              Remove
            </div>
            <div className="item" onClick={invokeEdit}>
              Edit
            </div>
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
      <CategoryRemove
        isActive={isRemoveActive}
        setIsActive={setIsRemoveActive}
        category={category}
      />
      <CategoryUpdate
        isActive={isEditActive}
        setIsActive={setIsEditActive}
        category={category}
      />
    </>
  );
};

export default ContextMenu;
