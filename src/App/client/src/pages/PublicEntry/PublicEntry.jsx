import { useParams } from "react-router";
import { EntriesService } from "../../api";
import "./PublicEntry.scss";
import { useEffect, useState } from "react";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import DefaultAvatar from "../../assets/DefaultAvatar.png";

const PublicEntryPage = () => {
  const { link } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const [entry, setEntry] = useState({});

  useEffect(() => {
    (async () => {
      let result = await EntriesService.getApiEntriesPublic({ url: link });

      setEntry(result.data);
      setIsLoading(false);
      console.log(result);
    })();
  }, []);

  return (
    <div className="public-entry-container">
      {isLoading ? (
        <Loader />
      ) : (
        <>
          <div className="owner">
            <div className="avatar">
              <img src={entry.AvatarUrl ?? DefaultAvatar} alt="avatar" />
            </div>
            <div className="prop ellipsis">
              <div className="key">Author: </div>
              <div className="value">{entry.owner}</div>
            </div>
            <div className="prop ellipsis">
              <div className="key">Size: </div>
              <div className="value">{entry.size}</div>
            </div>
            <div className="prop ellipsis">
              <div className="key">Title: </div>
              <div className="value">{entry.title}</div>
            </div>
            <div className="prop ellipsis">
              <div className="key">Created: </div>
              <div className="value">{entry.createdOn}</div>
            </div>
            <div className="prop ellipsis">
              <div className="key">Updated: </div>
              <div className="value">{entry.lastUpdatedOn}</div>
            </div>
          </div>
          <div className="public-entry border-gradient">
            <MEDitor.Markdown source={entry.content} />
          </div>
        </>
      )}
    </div>
  );
};

export default PublicEntryPage;
