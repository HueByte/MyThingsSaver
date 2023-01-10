import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useOutletContext, useParams } from "react-router";
import { NavLink } from "react-router-dom";
// import EntriesRepository from "../../api/repositories/EntriesRepository";
import Loader from "../../components/Loaders/Loader";
import EntryDelete from "./components/EntryDelete";
import EntryUpdate from "./components/EntryUpdate";
import EntryAdd from "./components/EntryAdd";
import {
  FaGhost,
  FaPenSquare,
  FaPlus,
  FaStickyNote,
  FaTimes,
} from "react-icons/fa";
import { EntriesService } from "../../api";

const ExplorerContent = () => {
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
    (async () => {
      await fetchEntries(categoryId);
    })();
  }, [categoryId]);

  useEffect(() => {
    setIsLoadingEntries(false);
  }, [currentEntries]);

  const fetchEntries = async (categoryId) => {
    setIsLoadingEntries(true);

    let result = await EntriesService.getApiEntriesAll({
      categoryId: categoryId,
      withContent: false,
    });

    setCurrentEntries(result.data.entries);
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
                <FaPlus
                  class="icon-action"
                  onClick={() => setIsAddActive(true)}
                />
              </div>
            </div>
            {currentEntries.length > 0 ? (
              currentEntries.map((entry) => {
                return (
                  <NavLink
                    key={entry.id}
                    className="row item"
                    to={`/entry/${lastUsedId}/${entry.id}`}
                  >
                    <div className="space icon">
                      <FaStickyNote />
                    </div>
                    <div className="column title ellipsis">
                      <abbr
                        title={entry.name}
                        className="ellipsis"
                        style={{ cursor: "pointer" }}
                      >
                        {entry.name}
                      </abbr>
                    </div>
                    <div className="column date">
                      {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
                    </div>
                    <div className="column size">{entry.size} B</div>
                    <div className="column size">md</div>
                    <div className="column actions">
                      <FaPenSquare
                        onClick={(e) => {
                          e.preventDefault();
                          invokeEdit(entry);
                        }}
                      />

                      <FaTimes
                        onClick={(e) => {
                          e.preventDefault();
                          invokeDelete(entry);
                        }}
                      />
                    </div>
                  </NavLink>
                );
              })
            ) : (
              <div className="empty">
                <FaGhost />
              </div>
            )}

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
          <>
            <div className="empty">
              <FaGhost />
            </div>
          </>
        )
      ) : (
        <Loader />
      )}
    </>
  );
};

export default ExplorerContent;
