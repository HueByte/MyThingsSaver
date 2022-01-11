import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useContext } from "react";
import { NavLink } from "react-router-dom";
import { AuthContext } from "../../auth/AuthContext";
import { Outlet, useNavigate } from "react-router";
import Loader from "../../components/Loaders/Loader";
import { CategoryContext } from "../../contexts/CategoryContext";
import "./Explorer.scss";

const Explorer = () => {
  const categoryContext = useContext(CategoryContext);
  const auth = useContext(AuthContext);
  const navigate = useNavigate();

  const [lastUsedId, setLastUsedId] = useState("");
  const [lastUsedPath, setLastUsedPath] = useState();
  const [finishedLoading, setFinishedLoading] = useState(false);

  useEffect(() => {
    let lastPath = localStorage.getItem("lastPath")?.split("/");
    if (lastPath) {
      var result = categoryContext.categories.find(
        (x) => x.categoryId == lastPath[lastPath.length - 1]
      );

      setLastUsedPath(result?.path.split("/"));
      navigate(`/explore/${result.categoryId}`);
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
        <div className="left-menu border-gradient border-gradient-purple">
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
    </div>
  );
};

export default Explorer;

const Item = ({ category, recentPath }) => {
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
          <>
            <i style={{ marginLeft: "10px" }}></i>
          </>
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
