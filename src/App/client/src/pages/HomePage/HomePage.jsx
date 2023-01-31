import React, { useContext, useEffect, useState } from "react";
import { FaFolderOpen, FaGhost, FaStickyNote } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import { EntriesService } from "../../api";
import Loader from "../../components/Loaders/Loader";
import { getSize } from "../../core/Lib";
import { errorModal } from "../../core/Modals";

const HomePage = () => {
  const [entries, setEntries] = useState([]);
  const [isFetching, setFetching] = useState(true);

  useEffect(() => {
    (async () => {
      let result = await EntriesService.getApiEntriesRecent();
      if (result.isSuccess) {
        setEntries(result.data);
      } else {
        errorModal(result.errors.join("\n"));
      }

      setFetching(false);
    })();
  }, []);

  return (
    <>
      <div className="m-auto flex h-full w-full max-w-[1920px] flex-row flex-wrap justify-center gap-8 px-32 py-16 lg:px-8">
        {isFetching ? (
          <Loader />
        ) : (
          <>
            {entries.length > 0 ? (
              entries.map((entry, index) => (
                <NavLink
                  to={`/entry/${entry.category.id}/${entry.id}`}
                  className="entry"
                  key={index}
                >
                  <div className="mts-border-gradient-r h-[200px] w-[250px] shadow-xl shadow-element transition duration-300 hover:text-accent7 hover:shadow-accent5">
                    <div className="h-full w-full rounded-lg bg-altBackgroundColor p-2">
                      <div className="text-2xl">
                        <FaStickyNote />
                      </div>
                      <div className="mts-text-gradient truncate text-lg font-bold">
                        {entry.name}
                      </div>
                      <div className="flex w-full flex-row gap-1 text-textColor">
                        <div className="flex flex-1 flex-col">
                          <div>Category</div>
                          <div>Edited on</div>
                          <div>Created on</div>
                          <div>Size</div>
                        </div>
                        <div className="flex flex-1 flex-col">
                          <div>{entry.category.name}</div>
                          <div>
                            {new Date(
                              entry.lastUpdatedOn + "Z"
                            ).toLocaleDateString()}
                          </div>
                          <div>
                            {new Date(
                              entry.createdOn + "Z"
                            ).toLocaleDateString()}
                          </div>
                          <div>{getSize(entry.size)}</div>
                        </div>
                      </div>
                    </div>
                  </div>
                </NavLink>
              ))
            ) : (
              <div className="grid h-full w-full place-items-center">
                <div className="text-4x4 flex flex-col items-center gap-4 text-4xl">
                  <FaGhost /> Kind of empty here
                </div>
              </div>
            )}
          </>
        )}
      </div>
    </>
  );
};

export default HomePage;
