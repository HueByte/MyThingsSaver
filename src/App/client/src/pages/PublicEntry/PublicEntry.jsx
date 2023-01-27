import { useParams } from "react-router";
import { EntriesService } from "../../api";
import "./PublicEntry.scss";
import { useEffect, useState } from "react";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import DefaultAvatar from "../../assets/DefaultAvatar.png";
import { getSize } from "../../core/Lib";

const PublicEntryPage = () => {
  const { link } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const [entry, setEntry] = useState({});

  useEffect(() => {
    (async () => {
      let result = await EntriesService.getApiEntriesPublic({ url: link });

      setEntry(result.data);
      setIsLoading(false);
    })();
  }, []);

  return (
    <div className="public-entry-container">
      {isLoading ? (
        <Loader />
      ) : (
        <>
          <div className="element element-owner">
            <div className="owner">
              <div className="avatar">
                <img src={entry.AvatarUrl ?? DefaultAvatar} alt="avatar" />
              </div>
              <div className="items">
                <div className="prop">
                  <div className="key">Author: </div>
                  <div className="value ellipsis">{entry.owner}</div>
                </div>
                <div className="prop">
                  <div className="key">Size: </div>
                  <div className="value">{getSize(entry.size)}</div>
                </div>
                <div className="prop">
                  <div className="key">Title: </div>
                  <div className="value">{entry.title}</div>
                </div>
                <div className="prop desk">
                  <div className="key">Created: </div>
                  <div className="value">
                    {new Date(entry.createdOn + "Z").toLocaleDateString()}
                  </div>
                </div>
                <div className="prop desk">
                  <div className="key">Updated: </div>
                  <div className="value">
                    {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="element">
            <div className="public-entry border-gradient">
              <MEDitor.Markdown source={entry.content} />
            </div>
          </div>
          <div className="element"></div>
        </>
      )}
    </div>
  );
};

export default PublicEntryPage;
