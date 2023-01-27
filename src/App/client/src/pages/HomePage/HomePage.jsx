import React, { useContext, useEffect, useState } from "react";
import { FaFolderOpen, FaStickyNote } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import { EntriesService } from "../../api";
import Loader from "../../components/Loaders/Loader";
import { getSize } from "../../core/Lib";
import { errorModal } from "../../core/Modals";
import "./HomePage.scss";

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
    <div className="homepage__container">
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
                <div className="image">
                  <FaStickyNote />
                </div>
                <div className="name ellipsis">{entry.name}</div>
                <div className="information">
                  <div className="line">
                    <div className="item">Category:</div>
                    <div className="item ellipsis">{entry.category.name}</div>
                  </div>
                  <div className="line">
                    <div className="item">Edited on:</div>
                    <div className="item">
                      {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
                    </div>
                  </div>
                  <div className="line">
                    <div className="item">Created on:</div>
                    <div className="item">
                      {new Date(entry.createdOn + "Z").toLocaleDateString()}
                    </div>
                  </div>
                  <div className="line">
                    <div className="item">Size:</div>
                    <div className="item">
                      <div>{getSize(entry.size)}</div>
                    </div>
                  </div>
                </div>
              </NavLink>
            ))
          ) : (
            <div
              style={{
                display: "grid",
                placeItems: "center",
                width: "100%",
                height: "100%",
                fontSize: "xxx-large",
              }}
            >
              <FaFolderOpen />
              <span
                style={{
                  color: "var(--Rose)",
                  fontSize: "x-large",
                  textAlign: "center",
                }}
              >
                You haven't got any recent entries
              </span>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default HomePage;
