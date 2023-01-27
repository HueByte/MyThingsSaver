import { useParams } from "react-router";
import { EntriesService } from "../../api";
import "./PublicEntry.scss";
import { useEffect, useState } from "react";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import DefaultAvatar from "../../assets/DefaultAvatar.png";
import { getSize } from "../../core/Lib";
import { FaFire, FaGhost } from "react-icons/fa";
import { AiFillCloud, AiFillRightCircle } from "react-icons/ai";
import { MdDateRange } from "react-icons/md";

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
      ) : entry ? (
        <>
          <div className="element element-owner">
            <div className="owner">
              <div className="avatar">
                <img src={entry.AvatarUrl ?? DefaultAvatar} alt="avatar" />
              </div>
              <div className="items">
                <div className="prop">
                  <div className="key">
                    <FaFire /> Author{" "}
                  </div>
                  <div className="value ellipsis">{entry.owner}</div>
                </div>
                <div className="prop">
                  <div className="key">
                    <AiFillCloud /> Size{""}
                  </div>
                  <div className="value">{getSize(entry.size)}</div>
                </div>
                <div className="prop">
                  <div className="key">
                    <AiFillRightCircle /> Title{" "}
                  </div>
                  <div className="value">{entry.title}</div>
                </div>
                <div className="prop desk">
                  <div className="key">
                    <MdDateRange /> Created{" "}
                  </div>
                  <div className="value">
                    {new Date(entry.createdOn + "Z").toLocaleDateString()}
                  </div>
                </div>
                <div className="prop desk">
                  <div className="key">
                    <MdDateRange /> Updated{" "}
                  </div>
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
      ) : (
        <div className="not-found">
          <div className="item">
            <div> Entry not found </div>
            <div>
              <FaGhost />
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default PublicEntryPage;
