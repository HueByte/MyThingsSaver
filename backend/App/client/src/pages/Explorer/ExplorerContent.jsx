import React from "react";
import { useEffect } from "react";
import { useContext } from "react";
import { useState } from "react";
import { useOutletContext, useParams } from "react-router";
import { NavLink } from "react-router-dom";
import EntriesRepository from "../../api/repositories/EntriesRepository";
import { AuthContext } from "../../auth/AuthContext";
import Loader from "../../components/Loaders/Loader";

const ExplorerContent = () => {
  const auth = useContext(AuthContext);
  const [lastUsedId, setLastUsedId] = useOutletContext();
  const [isLoadingEntries, setIsLoadingEntries] = useState(true);
  const [currentEntries, setCurrentEntries] = useState([]);
  let { categoryId } = useParams();

  // fetch entries once url parameter is retrieved
  useEffect(() => fetchEntries(categoryId), [categoryId]);

  useEffect(() => setIsLoadingEntries(false), [currentEntries]);

  const fetchEntries = async (categoryId) => {
    setIsLoadingEntries(true);

    let result = await EntriesRepository.GetAll(
      auth?.authState?.token,
      categoryId
    ).catch((error) => console.error(error));

    setCurrentEntries(result.data.categoryEntries);
    setLastUsedId(() => categoryId);
  };

  return (
    <>
      {!isLoadingEntries ? (
        currentEntries ? (
          <>
            {currentEntries.map((entry) => {
              return (
                <NavLink
                  key={entry.categoryEntryId}
                  className="item"
                  to={`/entry/${lastUsedId}/${entry.categoryEntryId}`}
                >
                  <div className="information">
                    <div className="icon">
                      <i class="fas fa-sticky-note"></i>
                    </div>
                    <div className="text ellipsis">
                      {entry.categoryEntryName}
                    </div>
                    <div className="date">
                      {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
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
    </>
  );
};

export default ExplorerContent;
