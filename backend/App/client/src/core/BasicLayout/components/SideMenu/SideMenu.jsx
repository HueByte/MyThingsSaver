import React, { useEffect, useRef, useContext } from "react";
import { NavLink } from "react-router-dom";
import { CategoryContext } from "../../../../contexts/CategoryContext";
import "./SideMenu.scss";

const SideMenu = ({ isEnabled, setIsEnabled }) => {
  const categoryContext = useContext(CategoryContext);
  const categoryInput = useRef();

  useEffect(() => {
    categoryInput.current = document.getElementById("newCategoryInput");
  }, []);

  const addNewCategory = async () => {
    if (categoryInput.current.value.length === 0) return;
    await categoryContext.ContextAddCategory(categoryInput.current.value);

    categoryInput.current.value = "";
  };

  const inputHandler = (event) => {
    if (event.key === "Enter") addNewCategory();
  };

  const collapse = () => {
    setIsEnabled(!isEnabled);
  };

  return (
    <>
      <div
        className={`nav-side-expander ${isEnabled ? "" : "side-menu-collapse"}`}
        onClick={collapse}
      ></div>
      <div className={`nav-side ${isEnabled ? "" : "side-menu-collapse"}`}>
        <div className="title">
          <p>Your Categories</p>
        </div>
        <div className="controls">
          <div onClick={addNewCategory} className="basic-button button">
            <i class="fa fa-plus" aria-hidden="true"></i>
          </div>
          <input
            id="newCategoryInput"
            onKeyDown={inputHandler}
            className="basic-input input"
            type="text"
            placeholder="Category name"
            autoComplete="off"
          />
        </div>
        <div className="container">
          {categoryContext.categories ? (
            categoryContext.categories.map((category, index) => {
              if (category.parentCategoryId) {
                return null;
              }
              return (
                <NavLink
                  activeClassName="active"
                  to={`/category/${category.name}/${category.categoryId}`}
                  key={index}
                  className="item"
                >
                  {category.name}
                </NavLink>
              );
            })
          ) : (
            <></>
          )}
        </div>
      </div>
    </>
  );
};

export default SideMenu;
