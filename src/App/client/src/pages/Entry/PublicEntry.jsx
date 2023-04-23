import { useState, useEffect } from "react";
import { useParams } from "react-router";
import MEDitor from "@uiw/react-md-editor";
import { EntriesService } from "../../api";
import Loader from "../../components/Loaders/Loader";
import DefaultAvatar from "../../assets/DefaultAvatar.png";
import { getSize } from "../../core/Lib";
import { FaFire } from "react-icons/fa";
import {
  AiFillCaretLeft,
  AiFillCloud,
  AiFillLike,
  AiFillRightCircle,
  AiFillSave,
} from "react-icons/ai";
import { MdDateRange } from "react-icons/md";

const PublicEntryPage = ({ isLogged = true }) => {
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

  const userItems = [
    {
      title: "Title",
      value: entry.title,
      icon: <AiFillRightCircle />,
    },
    {
      title: "Author",
      value: entry.owner,
      icon: <FaFire />,
    },
    {
      title: "Size",
      value: getSize(entry.size),
      icon: <AiFillCloud />,
    },
    {
      title: "Created",
      value: new Date(entry.createdOn + "Z").toLocaleDateString(),
      icon: <MdDateRange />,
    },
    {
      title: "Updated",
      value: new Date(entry.lastUpdatedOn + "Z").toLocaleDateString(),
      icon: <MdDateRange />,
    },
  ];

  return (
    <>
      {!isLoading ? (
        <>
          <div className="flex flex-row justify-center gap-4 p-4 lg:flex-col md:gap-0 md:px-0">
            <div className="mts-border-gradient-r h-fit w-[250px] flex-shrink-0 border-2 lg:h-24 lg:w-full md:rounded-none">
              <div className="flex h-full w-full flex-col items-center rounded-xl bg-altBackgroundColor lg:flex-row  lg:px-4">
                <div className="h-[246px] w-[246px] lg:mr-2 lg:h-[64px] lg:w-[64px]">
                  <img
                    className="h-full w-full rounded-full"
                    src={
                      entry.ownerAvatar || entry.ownerAvatar.length > 0
                        ? entry.ownerAvatar
                        : DefaultAvatar
                    }
                    alt="avatar"
                  />
                </div>
                <div className="flex w-full flex-row gap-1 p-2 lg:flex-col">
                  <div className="flex w-full flex-col gap-1 lg:flex-row lg:justify-around md:flex-wrap md:text-sm">
                    {userItems.map((item, index) => (
                      <div
                        className="flex flex-row lg:flex-col lg:items-center"
                        key={index}
                      >
                        <div className="flex-1 text-accent7">
                          {item.icon} {item.title}
                        </div>
                        <div className="flex-1 truncate">{item.value}</div>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </div>
            <div className="mb-4 h-fit min-h-[700px] w-[1024px] min-w-[calc(1024px_-_250px_-_4rem)] rounded-xl bg-altBackgroundColor p-8 pb-16 shadow-lg shadow-element lg:w-full lg:min-w-full md:rounded-none">
              <div className="mx-auto w-full max-w-[724px]">
                <div className="actions mb-4 flex w-full flex-row gap-4">
                  <div className="mts-btn-primary grid w-36 place-items-center">
                    <div>
                      <span>
                        <AiFillCaretLeft />
                      </span>
                      <span className="md:hidden"> Back</span>
                    </div>
                  </div>
                  <div className="mts-btn-primary grid w-36 place-items-center">
                    <div>
                      <AiFillLike />
                      <span className="md:hidden"> Like it</span>
                    </div>
                  </div>
                  <div className="mts-btn-primary grid w-36 place-items-center">
                    <div>
                      <span>
                        <AiFillSave />
                      </span>
                      <span className="md:hidden"> Save</span>
                    </div>
                  </div>
                </div>
                <MEDitor.Markdown source={entry.content} />
              </div>
            </div>
            <div className="w-[250px] min-w-0"></div>
          </div>
        </>
      ) : (
        <>
          <Loader />
        </>
      )}
    </>
  );
};

export default PublicEntryPage;
