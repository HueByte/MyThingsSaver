import React, { useContext, useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import EntriesRepository from "../../api/repositories/EntriesRepository";
import Loader from "../../components/Loaders/Loader";
import "./HomePage.scss";

const HomePage = () => {
  const [entries, setEntries] = useState([]);
  const [isFetching, setFetching] = useState(true);

  useEffect(() => {
    (async () => {
      await EntriesRepository.GetRecent()
        .then((result) => {
          if (result.isSuccess) setEntries(result.data);
        })
        .catch((error) => console.error(error));

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
                to={`/entry/${entry.category.categoryId}/${entry.categoryEntryId}`}
                className="entry"
                key={index}
              >
                <div className="image">
                  <i class="fas fa-sticky-note"></i>
                </div>
                <div className="name ellipsis">{entry.categoryEntryName}</div>
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
                      <div>{entry.size} B</div>
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
              <i class="fas fa-folder-open"></i>
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
