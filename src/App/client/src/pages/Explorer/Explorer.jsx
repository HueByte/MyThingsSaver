import { useRef } from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useContext } from "react";
import { AuthContext } from "../../contexts/AuthContext";
import { Outlet, useNavigate, useParams } from "react-router";
import Loader from "../../components/Loaders/Loader";
import { CategoryContext } from "../../contexts/CategoryContext";
import "./Explorer.scss";
import ContextMenu from "./components/ContextMenu";
import Item from "./ExplorerItem";
import { FaArrowLeft, FaArrowRight } from "react-icons/fa";

const Explorer = () => {
  const categoryContext = useContext(CategoryContext);
  const auth = useContext(AuthContext);
  const navigate = useNavigate();
  let { categoryId } = useParams();

  const [isMenuExpanded, setIsMenuExpanded] = useState(false);
  const [initialPos, setInitialPos] = useState(null);
  const [initialSize, setInitialSize] = useState(null);
  const explorer = useRef();

  const [lastUsedId, setLastUsedId] = useState("");
  const [lastUsedPath, setLastUsedPath] = useState();
  const [finishedLoading, setFinishedLoading] = useState(false);
  const [contextMenuCategory, setContextMenuCategory] = useState();

  useEffect(() => {
    explorer.current = document.getElementById("explorer-menu");

    if (categoryId) {
      var result = categoryContext.categories.find((x) => x.id == categoryId);

      setLastUsedPath(result?.path?.split("/"));
    } else {
      let lastPath = localStorage.getItem("lastPath")?.split("/");

      let result = {};
      if (lastPath) {
        result = categoryContext.categories.find(
          (x) => x.id == lastPath[lastPath.length - 1]
        );
      } else {
        result = categoryContext.categories.sort(
          (a, b) =>
            new Date(b.lastEditedOnDate + "Z") == new Date(a.lastEditedOnDate)
        )[0];
      }

      setLastUsedPath(result?.path.split("/"));
      if (result) navigate(`/explore/${result?.id}`, { replace: true });
    }

    setFinishedLoading(true);
  }, []);

  useEffect(() => {
    if (lastUsedId) {
      var result = categoryContext.categories.find((x) => x.id == lastUsedId);

      localStorage.setItem("lastPath", result?.path);
      setLastUsedPath(result?.path);
    }
  }, [lastUsedId]);

  // TEMP workaround for firefox
  const initial = (e) => {
    document.getElementById("dragger").style =
      "width: 100vw; transform: translateX(50vw);";

    setInitialPos(e.clientX);
    setInitialSize(explorer.current.offsetWidth);
  };

  const stopDrag = (e) => {
    document.getElementById("dragger").style =
      "width: 20px; transform: translateX(0px);";
  };

  const resize = (e) => {
    e.preventDefault();
    requestAnimationFrame(() => performDrag(e));
  };

  const performDrag = (e) => {
    explorer.current.style.width = `${initialSize + e.clientX - initialPos}px`;
  };

  const switchExpandMenu = () => {
    setIsMenuExpanded(!isMenuExpanded);
  };

  return (
    <div className="categories__wrapper animate__fadeIn animate__animated flex h-full w-full items-center justify-center">
      <div className="explorer-container flex h-[90%] max-h-[800px] w-[1024px] flex-row rounded-xl shadow-lg shadow-element">
        <div
          className={`relative z-20 w-1/6 max-w-[90%] bg-altBackgroundColor left-menu${
            isMenuExpanded ? " expand" : ""
          } mts-border-gradient-b `}
          id="explorer-menu"
          onContextMenu={() => setContextMenuCategory(null)}
        >
          <div className="expander" onClick={switchExpandMenu}>
            {isMenuExpanded ? <FaArrowLeft /> : <FaArrowRight />}
          </div>
          <div
            id="dragger"
            className="draggable"
            draggable="true"
            onDragStart={initial}
            onDragOver={resize}
            onDragEnd={stopDrag}
          ></div>
          <div className="item-container flex h-full w-full flex-col overflow-auto overflow-x-hidden bg-altBackgroundColor p-2">
            <div className="item truncate">{auth.authState?.username}</div>
            {finishedLoading ? (
              <>
                {categoryContext.categories ? (
                  <>
                    {categoryContext.categories.map((category) => {
                      if (category.level == 0) {
                        return (
                          <Item
                            key={category.id}
                            category={category}
                            recentPath={lastUsedPath}
                            setCurrentContextItem={setContextMenuCategory}
                          />
                        );
                      }
                    })}
                  </>
                ) : (
                  <></>
                )}
              </>
            ) : (
              <Loader />
            )}
          </div>
        </div>
        <div className="content__wrapper flex flex-col justify-between">
          <Outlet context={[lastUsedId, setLastUsedId]} />
        </div>
      </div>
      <ContextMenu category={contextMenuCategory} />
    </div>
  );
};

export default Explorer;
