import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useNavigate, useOutletContext, useParams } from "react-router";
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
import { getSize } from "../../core/Lib";

const ExplorerContent = () => {
  const navigate = useNavigate();
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
      setIsLoadingEntries(true);
      await fetchEntries(categoryId);
    })();
  }, [categoryId]);

  const fetchEntries = async (categoryId) => {
    let result = await EntriesService.getApiEntriesAll({
      categoryId: categoryId,
      withContent: false,
    });

    setCurrentEntries(result.data.entries);
    setLastUsedId(() => categoryId);
    setIsLoadingEntries(false);
  };

  const invokeDelete = (entry) => {
    setCurrentEntry(entry);
    setIsDeleteActive(true);
  };

  const invokeEdit = (entry) => {
    setCurrentEntry(entry);
    setIsEditActive(true);
  };

  const nav = (e) => {
    navigate(e);
  };

  return (
    <>
      {!isLoadingEntries ? (
        currentEntries ? (
          <div className="relative overflow-x-auto shadow-md sm:rounded-lg">
            <table class="w-full text-sm text-left text-textColor border-collapse">
              <thead class="text-accent4 text-base uppercase bg-altBackgroundColor">
                <tr>
                  <th scope="col" class="px-6 py-3">
                    Title
                  </th>
                  <th scope="col" class="px-6 py-3">
                    Date
                  </th>
                  <th scope="col" class="px-6 py-3">
                    Size
                  </th>
                  <th scope="col" class="px-6 py-3">
                    Type
                  </th>
                  <th scope="col" class="px-6 py-3">
                    Action
                  </th>
                </tr>
              </thead>
              <tbody>
                {currentEntries.map((entry) => {
                  return (
                    <tr
                      scope="row"
                      className="px-6 py-4 font-medium whitespace-nowrap even:bg-altBackgroundColor cursor-pointer transition duration-300 hover:bg-neutralDarker"
                      onClick={() => nav(`/entry/${lastUsedId}/${entry.id}`)}
                    >
                      <th className="px-6 py-4" key={entry.Id}>
                        <FaStickyNote className="mr-2" />
                        {entry.name}
                      </th>
                      <td className="px-6 py-4">
                        {new Date(
                          entry.lastUpdatedOn + "Z"
                        ).toLocaleDateString()}
                      </td>
                      <td className="px-6 py-4">{getSize(entry.size)}</td>
                      <td className="px-6 py-4">md</td>
                      <td className="px-6 py-4">
                        <FaPenSquare
                          className="mr-2 hover:text-accent"
                          onClick={(e) => {
                            e.stopPropagation();
                            invokeEdit(entry);
                          }}
                        />

                        <FaTimes
                          className="hover:text-accent"
                          onClick={(e) => {
                            e.stopPropagation();
                            invokeDelete(entry);
                          }}
                        />
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>

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
          <div className="empty">
            <FaGhost />
          </div>
        )
      ) : (
        <Loader />
      )}
    </>
  );
};

export default ExplorerContent;
