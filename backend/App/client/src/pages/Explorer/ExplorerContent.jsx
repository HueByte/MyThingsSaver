import React from "react";
import { useEffect } from "react";
import { useContext } from "react";
import { useState } from "react";
import { useOutletContext, useParams } from "react-router";
import { NavLink } from "react-router-dom";
import EntriesRepository from "../../api/repositories/EntriesRepository";
import { AuthContext } from "../../auth/AuthContext";
import Loader from "../../components/Loaders/Loader";
import EntryDelete from "./components/EntryDelete";
import EntryUpdate from "./components/EntryUpdate";
import EntryAdd from "./components/EntryAdd";

const ExplorerContent = () => {
  const auth = useContext(AuthContext);
  const [lastUsedId, setLastUsedId] = useOutletContext();
  const [isLoadingEntries, setIsLoadingEntries] = useState(true);
  const [currentEntries, setCurrentEntries] = useState([]);
  let { categoryId } = useParams();

  //actions
  const [currentEntry, setCurrentEntry] = useState(null);
  const [isAddActive, setIsAddActive] = useState(false);
  const [isEditActive, setIsEditActive] = useState(false);
  const [isDeleteActive, setIsDeleteActive] = useState(false);

  // fetch entries once url parameter is retrieved
  useEffect(() => {
    // (async () => {
    //   await fetchEntries(categoryId);
    // })();
    fetchEntries(categoryId);
  }, [categoryId]);

  useEffect(() => {
    setIsLoadingEntries(false);
  }, [currentEntries]);

  const fetchEntries = async (categoryId) => {
    setIsLoadingEntries(false);

    let result = await EntriesRepository.GetAll(categoryId).catch((error) =>
      console.error(error)
    );

    setCurrentEntries(result.data.categoryEntries);
    setLastUsedId(() => categoryId);
  };

  const invokeDelete = (entry) => {
    setCurrentEntry(entry);
    setIsDeleteActive(true);
  };

  const invokeEdit = (entry) => {
    setCurrentEntry(entry);
    setIsEditActive(true);
  };

  return (
    <>
      {!isLoadingEntries ? (
        currentEntries ? (
          <div className="content">
            <div className="row header">
              <div className="space"></div>
              <div className="column header-item title">Title</div>
              <div className="column header-item">Date</div>
              <div className="column header-item">Size</div>
              <div className="column header-item">Type</div>
              <div className="column actions">
                <i
                  class="fa fa-plus icon-action"
                  aria-hidden="true"
                  onClick={() => setIsAddActive(true)}
                ></i>
              </div>
            </div>
            {currentEntries.map((entry) => {
              return (
                <NavLink
                  key={entry.categoryEntryId}
                  className="row item"
                  to={`/entry/${lastUsedId}/${entry.categoryEntryId}`}
                >
                  <div className="space icon">
                    <i class="fas fa-sticky-note"></i>
                  </div>
                  <div className="column title ellipsis">
                    {entry.categoryEntryName}
                  </div>
                  <div className="column date">
                    {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
                  </div>
                  <div className="column size">{entry.size} B</div>
                  <div className="column size">md</div>
                  <div className="column actions">
                    <i
                      class="fas fa-pen-square"
                      onClick={(e) => {
                        e.preventDefault();
                        invokeEdit(entry);
                      }}
                    ></i>
                    <i
                      class="fa fa-times"
                      aria-hidden="true"
                      onClick={(e) => {
                        e.preventDefault();
                        invokeDelete(entry);
                      }}
                    ></i>
                  </div>
                </NavLink>
              );
            })}

            <EntryAdd
              isActive={isAddActive}
              setIsActive={setIsAddActive}
              categoryId={categoryId}
              setEntries={setCurrentEntries}
            />

            <EntryDelete
              isActive={isDeleteActive}
              setIsActive={setIsDeleteActive}
              entryToDelete={currentEntry}
              entries={currentEntries}
              setEntries={setCurrentEntries}
            />

            <EntryUpdate
              isActive={isEditActive}
              setIsActive={setIsEditActive}
              entryToEdit={currentEntry}
              entries={currentEntries}
              setEntries={setCurrentEntries}
            />
          </div>
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
