import React, { useRef } from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useContext } from "react";
import { NavLink } from "react-router-dom";
import { AuthContext } from "../../auth/AuthContext";
import { Outlet, useNavigate, useParams } from "react-router";
import Loader from "../../components/Loaders/Loader";
import { CategoryContext } from "../../contexts/CategoryContext";
import "./Explorer.scss";
import ContextMenu from "./components/ContextMenu";

const Explorer = () => {
  const categoryContext = useContext(CategoryContext);
  const auth = useContext(AuthContext);
  const navigate = useNavigate();
  let { categoryId } = useParams();

  const [isMenuExpanded, setIsMenuExpanded] = useState(false);
  const [initialPos, setInitialPos] = useState(null);
  const [initialSize, setInitialSize] = useState(null);
  const explorer = useRef();

  const [lastUsedId, setLastUsedId] = useState("");
  const [lastUsedPath, setLastUsedPath] = useState();
  const [finishedLoading, setFinishedLoading] = useState(false);
  const [contextMenuCategory, setContextMenuCategory] = useState();

  useEffect(() => {
    explorer.current = document.getElementById("explorer-menu");

    if (categoryId) {
      var result = categoryContext.categories.find(
        (x) => x.categoryId == categoryId
      );

      setLastUsedPath(result?.path?.split("/"));
    } else {
      let lastPath = localStorage.getItem("lastPath")?.split("/");

      if (lastPath) {
        var result = categoryContext.categories.find(
          (x) => x.categoryId == lastPath[lastPath.length - 1]
        );

        setLastUsedPath(result?.path.split("/"));
        if (result) navigate(`/explore/${result?.categoryId}`);
      }
    }

    setFinishedLoading(true);
  }, []);

  useEffect(() => {
    if (lastUsedId) {
      var result = categoryContext.categories.find(
        (x) => x.categoryId == lastUsedId
      );

      localStorage.setItem("lastPath", result?.path);
      setLastUsedPath(result?.path);
    }
  }, [lastUsedId]);

  // TEMP fix for firefox
  const initial = (e) => {
    console.log("start");
    document.getElementById("dragger").style =
      "width: 400px; transform: translateX(200px);";

    setInitialPos(e.clientX);
    setInitialSize(explorer.current.offsetWidth);
  };

  const stopDrag = (e) => {
    document.getElementById("dragger").style =
      "width: 20px; transform: translateX(0px);";
  };

  const resize = (e) => {
    e.preventDefault();
    requestAnimationFrame(() => performDrag(e));
  };

  const performDrag = (e) => {
    explorer.current.style.width = `${initialSize + e.clientX - initialPos}px`;
  };

  const switchExpandMenu = () => {
    setIsMenuExpanded(!isMenuExpanded);
  };

  return (
    <div className="categories__wrapper">
      <div className="container">
        <div
          className={`left-menu${isMenuExpanded ? " expand" : ""}`}
          id="explorer-menu"
          onContextMenu={() => setContextMenuCategory(null)}
        >
          <div className="expander" onClick={switchExpandMenu}>
            {isMenuExpanded ? (
              <i class="fa fa-arrow-left" aria-hidden="true"></i>
            ) : (
              <i class="fa fa-arrow-right" aria-hidden="true"></i>
            )}
          </div>
          <div
            id="dragger"
            className="draggable"
            draggable="true"
            onDragStart={initial}
            // onDrag={resize}
            onDragOver={resize}
            onDragEnd={stopDrag}
          ></div>
          <div className="item-container">
            <div className="item ellipsis">{auth.authState?.username}</div>
            {finishedLoading ? (
              <>
                {categoryContext.categories ? (
                  <>
                    {categoryContext.categories.map((category) => {
                      if (category.level == 0) {
                        return (
                          <Item
                            key={category.categoryId}
                            category={category}
                            recentPath={lastUsedPath}
                            setCurrentContextItem={setContextMenuCategory}
                          />
                        );
                      }
                    })}
                  </>
                ) : (
                  <></>
                )}
              </>
            ) : (
              <Loader />
            )}
          </div>
        </div>
        <div className="content__wrapper">
          <Outlet context={[lastUsedId, setLastUsedId]} />
        </div>
      </div>
      <ContextMenu category={contextMenuCategory} />
    </div>
  );
};

export default Explorer;

const Item = ({ category, recentPath, setCurrentContextItem }) => {
  const [showChilds, setShowChilds] = useState(false);

  useEffect(() => {
    if (recentPath?.includes(category.categoryId)) {
      setShowChilds(true);
    }
  }, []);

  return (
    <>
      <NavLink
        to={`/explore/${category.categoryId}`}
        className="item ellipsis"
        style={determineDepth(category)}
        onContextMenu={(e) => {
          e.stopPropagation();
          setCurrentContextItem(category);
        }}
      >
        {category.childCategories ? (
          showChilds ? (
            <i
              onClick={(e) => {
                e.preventDefault();
                setShowChilds(!showChilds);
              }}
              class="fa fa-angle-down"
            ></i>
          ) : (
            <i
              onClick={(e) => {
                e.preventDefault();
                setShowChilds(!showChilds);
              }}
              class="fa fa-angle-right"
            ></i>
          )
        ) : (
          <i style={{ marginLeft: "10px" }}></i>
        )}
        {category.name}
      </NavLink>
      {showChilds ? (
        category.childCategories?.map((subCategory) => {
          return (
            <Item
              key={subCategory.categoryId}
              category={subCategory}
              recentPath={recentPath}
              setCurrentContextItem={setCurrentContextItem}
            />
          );
        })
      ) : (
        <></>
      )}
    </>
  );
};

const determineDepth = (category) => {
  if (category.level > 0) {
    const style = {
      marginLeft: `${category.level * 5 + 10}px`,
      border: "none",
    };

    return style;
  }
};
