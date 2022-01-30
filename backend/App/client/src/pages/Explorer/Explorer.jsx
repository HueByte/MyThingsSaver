import React from "react";
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
  const [touchStart, setTouchStart] = useState();
  const [touchEnd, setTouchEnd] = useState();
  const [isMenuExpanded, setIsMenuExpanded] = useState(false);

  const [lastUsedId, setLastUsedId] = useState("");
  const [lastUsedPath, setLastUsedPath] = useState();
  const [finishedLoading, setFinishedLoading] = useState(false);
  const [contextMenuCategory, setContextMenuCategory] = useState();

  useEffect(() => {
    // if mobile
    document
      .getElementById("explorer-menu")
      .addEventListener("touchstart", handleGestureStart, false);
    document
      .getElementById("explorer-menu")
      .addEventListener("touchend", handlegestureEnd, false);

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

    return () => {
      document
        .getElementById("explorer-menu")
        .removeEventListener("touchstart", handleGestureStart, false);
      document
        .getElementById("explorer-menu")
        .removeEventListener("touchend", handlegestureEnd, false);
    };
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

  const handleGestureStart = (e) => {
    setTouchStart(e.changedTouches[0].screenX);
    console.log(e.changedTouches[0].screenX);
  };

  const handlegestureEnd = (e) => {
    setTouchEnd(e.changedTouches[0].screenX);
    console.log(e.changedTouches[0].screenX);
    handleGesture();
  };

  const handleGesture = () => {
    //swipe left
    console.log(touchEnd > touchStart);
    if (touchEnd > touchStart) setIsMenuExpanded(true);
    if (touchEnd < touchStart) setIsMenuExpanded(false);
  };

  return (
    <div className="categories__wrapper">
      <div className="container">
        <div
          className={`left-menu${isMenuExpanded ? " expand" : ""}`}
          id="explorer-menu"
          onContextMenu={() => setContextMenuCategory(null)}
        >
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
