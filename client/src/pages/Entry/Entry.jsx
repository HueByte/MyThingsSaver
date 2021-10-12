import React, {
  useEffect,
  useState,
  useContext,
  useRef,
  useCallback,
} from "react";
import { Redirect, useParams } from "react-router";
import { DeleteOneEntry, GetOneEntry, UpdateOneEntry } from "../../api/Entries";
import { AuthContext } from "../../auth/AuthContext";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import "./Entry.css";
import { successModal } from "../../core/Modals";
import { BasicModal } from "../../components/BasicModal/BasicModal";
import { CategoryContext } from "../../contexts/CategoryContext";
import AwesomeDebouncePromise from "awesome-debounce-promise";

const Entry = () => {
  const authContext = useContext(AuthContext);
  const categoryContext = useContext(CategoryContext);
  const { categoryId, entryId } = useParams();
  const [categoryName, setCategoryName] = useState("");
  const [entry, setEntry] = useState();
  const [name, setName] = useState();
  const [editValue, setEditValue] = useState();
  const [isEditing, setIsEditing] = useState(false);
  const [shouldRedirect, setShouldRedirect] = useState(false);
  const [isMobileEdit, setIsMobileEdit] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const entryToDelete = useRef();

  useEffect(async () => {
    await GetOneEntry(authContext.authState?.token, entryId)
      .then((result) => {
        if (!result.isSuccess) {
          setShouldRedirect(true);
          return;
        }

        setEntry(result.data);
        setName(result.data.categoryEntryName);
        setEditValue(result.data.content);
      })
      .catch((error) => console.error(error));

    let catname = categoryContext.categories?.find(
      (c) => c.categoryId == categoryId
    ).name;
    setCategoryName(catname);
  }, []);

  const sendUpdateCallback = async (newName, data) => {
    console.log(newName);
    await UpdateOneEntry(
      authContext.authState?.token,
      entryId,
      newName,
      data
    ).catch((error) => console.error(error));
  };

  const performAutosave = AwesomeDebouncePromise(sendUpdateCallback, 2000);

  const autoSave = async (value) => {
    setEditValue(value);
    await performAutosave(name, value);
  };

  // Remove entry
  const invokeDeleteModal = (entry) => {
    entryToDelete.current = entry;
    setIsDeleteModalOpen(true);
  };

  const removeEntry = async () => {
    await DeleteOneEntry(authContext.authState?.token, entryId)
      .then(() => setShouldRedirect(true))
      .catch((error) => console.error(error));

    successModal("Sucessfully removed entry");
  };

  const handleChange = async (event) => {
    setName(event.target.value);
    await performAutosave(event.target.value, editValue);
  };

  const switchEdit = () => {
    window.innerWidth > 1024 ? setIsMobileEdit(false) : setIsMobileEdit(true);
    setIsEditing(!isEditing);
  };

  const closeDeleteModal = () => setIsDeleteModalOpen(false);

  if (shouldRedirect)
    return <Redirect to={`/category/${categoryName}/${categoryId}`} />;
  return (
    <div className="entry__container">
      {entry ? (
        <>
          <div className="basic-info">
            <div className="basic-info-left">
              <div className="icon">
                <i class="fas fa-sticky-note"></i>
              </div>
              <div className="name">
                <abbr title={name} className="ellipsis">
                  <span>{name}</span>
                </abbr>
              </div>
              <div className="date">
                {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
              </div>
            </div>
            <div className="basic-info-right">
              <div className="basic-button entry-button" onClick={switchEdit}>
                {isEditing ? "close" : "edit"}
              </div>
              <div
                className="basic-button entry-button"
                onClick={() => invokeDeleteModal(entry)}
              >
                Delete
              </div>
            </div>
          </div>
          <div className="basic-info basic-info-mobile-menu">
            <div className="basic-button entry-button" onClick={switchEdit}>
              {isEditing ? "close" : "edit"}
            </div>
            <div
              className="basic-button entry-button"
              onClick={() => invokeDeleteModal(entry)}
            >
              Delete
            </div>
          </div>
          <div className="entry-content">
            {isEditing ? (
              <>
                <div className="edit-name-menu">
                  <label>Name:</label>
                  <input
                    type="text"
                    className="basic-input edit-name"
                    placeholder={`${name}`}
                    onChange={handleChange}
                    autoComplete="off"
                  />
                </div>
                <MEDitor
                  value={editValue}
                  onChange={autoSave}
                  commands={[]}
                  height={500}
                  preview={isMobileEdit ? "edit" : "live"}
                />
              </>
            ) : (
              <MEDitor.Markdown source={editValue} />
            )}
          </div>
        </>
      ) : (
        <Loader />
      )}
      <BasicModal
        isOpen={isDeleteModalOpen}
        shouldCloseOnOverlayClick={true}
        onRequestClose={closeDeleteModal}
      >
        <DeleteModal
          entry={entryToDelete.current}
          onDelete={removeEntry}
          closeDeleteModal={closeDeleteModal}
        />
      </BasicModal>
    </div>
  );
};

const DeleteModal = ({ entry, onDelete, closeDeleteModal }) => {
  return (
    <div>
      <p style={{ fontSize: "larger", fontWeight: "bold" }}>
        Are you sure you want to delete{" "}
        <span className="ellipsis" style={{ color: "var(--Rose)" }}>
          {entry.categoryEntryName}
        </span>
      </p>
      <div className="modal-menu-buttons">
        <div
          className="basic-button accept"
          onClick={() => onDelete(entry.categoryEntryId)}
        >
          Yes
        </div>
        <div className="basic-button close" onClick={closeDeleteModal}>
          No
        </div>
      </div>
    </div>
  );
};

export default Entry;
