import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useContext } from "react";
import { NavLink } from "react-router-dom";
import EntriesRepository from "../../api/repositories/EntriesRepository";
import { AuthContext } from "../../auth/AuthContext";
import Loader from "../../components/Loaders/Loader";
import { CategoryContext } from "../../contexts/CategoryContext";
import "./Explorer.scss";

const Explorer = () => {
  const categoryContext = useContext(CategoryContext);
  const auth = useContext(AuthContext);
  const [currentEntries, setCurrentEntries] = useState([]);
  const [currentCategoryID, setCurrentCategoryID] = useState();
  const [lastUsedPath, setLastUsedPath] = useState();
  const [finishedFetching, setFinishedFetching] = useState(false);
  const [isLoadingEntries, setIsLoadingEntries] = useState(true);

  useEffect(async () => {
    let lastPath = localStorage.getItem("lastPath")?.split("/");
    let lastCategoryID = lastPath ? lastPath[lastPath.length - 1] : null;

    if (lastCategoryID) {
      let result = await EntriesRepository.GetAll(
        auth?.authState?.token,
        lastCategoryID
      ).catch((error) => console.error(error));

      console.log(lastPath);

      setCurrentEntries(result.data.categoryEntries);
      setCurrentCategoryID(lastCategoryID);
      setLastUsedPath(lastPath);
    }

    setFinishedFetching(true);
    setIsLoadingEntries(false);
  }, []);

  useEffect(() => setIsLoadingEntries(false), [currentEntries]);

  const fetchEntries = async (category) => {
    setIsLoadingEntries(true);

    let result = await EntriesRepository.GetAll(
      auth?.authState?.token,
      category.categoryId
    ).catch((error) => console.error(error));

    setCurrentEntries(result.data.categoryEntries);
    setCurrentCategoryID(category.categoryId);
    setLastUsedPath(category.path);
    localStorage.setItem("lastPath", category.path);
  };

  return (
    <div className="categories__wrapper">
      <div className="container">
        <div className="left-menu border-gradient border-gradient-purple">
          <div className="item ellipsis">{auth.authState?.username}</div>
          {finishedFetching && categoryContext.categories ? (
            <>
              {categoryContext.categories.map((category, index) => {
                if (category.level == 0) {
                  return (
                    <>
                      <Item
                        index={index}
                        category={category}
                        fetch={fetchEntries}
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
        </div>
        <div className="content">
          {!isLoadingEntries ? (
            currentEntries ? (
              <>
                {currentEntries.map((entry, index) => {
                  return (
                    <NavLink
                      key={index}
                      className="item"
                      to={`/entry/${currentCategoryID}/${entry.categoryEntryId}`}
                    >
                      <div className="information">
                        <div className="icon">
                          <i class="fas fa-sticky-note"></i>
                        </div>
                        <div className="text ellipsis">
                          {entry.categoryEntryName}
                        </div>
                        <div className="date">
                          {new Date(
                            entry.lastUpdatedOn + "Z"
                          ).toLocaleDateString()}
                        </div>
                        <div className="size">{entry.size} B</div>
                        <div className="size">md</div>
                      </div>
                      <div className="actions">
                        <i class="fas fa-pen-square"></i>
                        <i class="fa fa-times" aria-hidden="true"></i>
                      </div>
                    </NavLink>
                  );
                })}
              </>
            ) : (
              <></>
            )
          ) : (
            <Loader />
          )}
        </div>
      </div>
    </div>
  );
};

export default Explorer;

const Item = ({ index, category, fetch, recentPath }) => {
  const [showChilds, setShowChilds] = useState(false);

  useEffect(() => {
    if (recentPath?.includes(category.categoryId)) {
      setShowChilds(true);
    }
  }, []);

  return (
    <>
      <div
        key={index}
        className="item ellipsis"
        onClick={() => fetch(category)}
        style={determineDepth(category)}
      >
        {category.childCategories ? (
          showChilds ? (
            <i
              onClick={() => setShowChilds(!showChilds)}
              class="fa fa-angle-down"
            ></i>
          ) : (
            <i
              onClick={() => setShowChilds(!showChilds)}
              class="fa fa-angle-right"
            ></i>
          )
        ) : (
          <>
            <i style={{ marginLeft: "10px" }}></i>
          </>
        )}
        {category.name}
      </div>
      {showChilds ? (
        category.childCategories?.map((subCategory, index) => {
          return (
            <Item
              index={index}
              category={subCategory}
              fetch={() => fetch(subCategory)}
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
