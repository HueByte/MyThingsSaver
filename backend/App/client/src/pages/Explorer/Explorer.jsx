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

  const [lastUsedId, setLastUsedId] = useState("");
  const [lastUsedPath, setLastUsedPath] = useState();
  const [finishedLoading, setFinishedLoading] = useState(false);
  const [contextMenuCategory, setContextMenuCategory] = useState();

  useEffect(() => {
    let lastPath = localStorage.getItem("lastPath")?.split("/");
    if (lastPath && !categoryId) {
      var result = categoryContext.categories.find(
        (x) => x.categoryId == lastPath[lastPath.length - 1]
      );

      setLastUsedPath(result?.path.split("/"));
      navigate(`/explore/${result.categoryId}`);
    } else if (categoryId) {
      var result = categoryContext.categories.find(
        (x) => x.categoryId == categoryId
      );

      setLastUsedPath(result?.path.split("/"));
    }

    setFinishedLoading(true);
  }, []);

  // once entires are fetched update lastPath in localstorage
  useEffect(() => {
    if (lastUsedId) {
      var result = categoryContext.categories.find(
        (x) => x.categoryId == lastUsedId
      );

      localStorage.setItem("lastPath", result?.path);
      setLastUsedPath(result?.path);
    }
  }, [lastUsedId]);

  return (
    <div className="categories__wrapper">
      <div className="container">
        <div
          className="left-menu"
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
                        <>
                          <Item
                            key={category.categoryId}
                            category={category}
                            recentPath={lastUsedPath}
                            setCurrentContextItem={setContextMenuCategory}
                          />
                        </>
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
        <div className="content">
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
    console.log(recentPath);

    if (recentPath?.includes(category.categoryId)) {
      console.log("bonker", category.name);
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
