import React, { useEffect, useState, useRef } from "react";
import { Navigate, useParams } from "react-router";
import EntriesRepository from "../../api/repositories/EntriesRepository";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import "./Entry.scss";
import "./Markdown-overrides.scss";
import { successModal } from "../../core/Modals";
import { BasicModal } from "../../components/BasicModal/BasicModal";
import AwesomeDebouncePromise from "awesome-debounce-promise";
import DropdownButton from "../../components/Dropdown/Dropdown";

const Entry = () => {
  // const authContext = useContext(AuthContext);
  const { categoryId, entryId } = useParams();

  const [entry, setEntry] = useState();
  const [name, setName] = useState();
  const [editValue, setEditValue] = useState();
  const [isEditing, setIsEditing] = useState(false);
  const [shouldRedirect, setShouldRedirect] = useState(false);
  const [isMobileEdit, setIsMobileEdit] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const entryToDelete = useRef();

  useEffect(() => {
    (async () => {
      await EntriesRepository.Get(entryId)
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
    })();
  }, []);

  const sendUpdateCallback = async (newName, data) => {
    await EntriesRepository.Update(entryId, newName, data).catch((error) =>
      console.error(error)
    );
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
    await EntriesRepository.Delete(entryId)
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

  if (shouldRedirect) return <Navigate to={`/explore/${categoryId}`} />;
  return (
    <div className="entry__container">
      {entry ? (
        <>
          <div className="top-info">
            <div className="left">
              <div className="icon">
                <i class="fas fa-sticky-note"></i>
              </div>
              <div className="title">
                <abbr title={name} className="ellipsis">
                  <span>{name}</span>
                </abbr>
              </div>
              <div className="date">
                {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
              </div>
            </div>
            <div className="right">
              <DropdownButton>
                <div className="option" onClick={switchEdit}>
                  {isEditing ? "Close" : "Edit"}
                </div>
                <div
                  className="option"
                  onClick={() => invokeDeleteModal(entry)}
                >
                  Delete
                </div>
              </DropdownButton>
            </div>
          </div>
          <div className="content__container">
            <div className={`content${isEditing ? " content-expand" : ""}`}>
              {isEditing ? (
                <>
                  <div className="edit-name">
                    <label>Name:</label>
                    <input
                      type="text"
                      className="basic-input"
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
    <>
      <div className="content">
        Are you sure you want to delete{" "}
        <span class="title">{entry.categoryEntryName} ?</span>
      </div>
      <div className="menu horizontal">
        <div
          className="basic-button item"
          onClick={() => onDelete(entry.categoryEntryId)}
        >
          Yes
        </div>
        <div className="basic-button item" onClick={closeDeleteModal}>
          No
        </div>
      </div>
    </>
  );
};

export default Entry;
