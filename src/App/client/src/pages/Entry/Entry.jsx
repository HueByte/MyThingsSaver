import React, { useEffect, useState, useRef } from "react";
import { Navigate, useParams } from "react-router";
// import EntriesRepository from "../../api/repositories/EntriesRepository";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import "./Entry.scss";
import "./Markdown-overrides.scss";
import { successModal, warningModal } from "../../core/Modals";
import { BasicModal } from "../../components/BasicModal/BasicModal";
import AwesomeDebouncePromise from "awesome-debounce-promise";
import { DropdownItem, DropdownButton } from "../../components/Dropdown";
import { FaRegStickyNote } from "react-icons/fa";
import { EntriesService } from "../../api";

const sendUpdateCallback = async (entryId, newName, data) => {
  await EntriesService.putApiEntries({
    requestBody: {
      entryId: entryId,
      content: data,
      entryName: newName,
    },
  });

  // await EntriesRepository.Update(entryId, newName, data).catch((error) =>
  //   console.error(error)
  // );
};

const performAutosave = AwesomeDebouncePromise(sendUpdateCallback, 2000);

const Entry = () => {
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
      let result = await EntriesService.getApiEntries(entryId);
      if (!result.isSuccess) {
        setShouldRedirect(true);
        return;
      }

      setEntry(result.data);
      setName(result.data.categoryEntryName);
      setEditValue(result.data.content);
      checkEntrySize(result.data.content.length);

      // await EntriesRepository.Get(entryId)
      //   .then((result) => {
      //     if (!result.isSuccess) {
      //       setShouldRedirect(true);
      //       return;
      //     }

      //     setEntry(result.data);
      //     setName(result.data.categoryEntryName);
      //     setEditValue(result.data.content);

      //     checkEntrySize(result.data.content.length);
      //   })
      //   .catch((error) => console.error(error));
    })();
  }, []);

  const checkEntrySize = (length) => {
    if (length / 1024 > 100) {
      warningModal(
        `Your entry has size of ${Math.floor(
          length / 1024
        )} KB, using preview might affect your performance.`,
        10000
      );
    }
  };

  const handleChange = async (event) => {
    setName(event.target.value);
    await performAutosave(entryId, event.target.value, editValue);
  };

  const autoSave = async (value) => {
    setEditValue(value);
    await performAutosave(entryId, name, value);
  };

  // Remove entry
  const invokeDeleteModal = (entry) => {
    entryToDelete.current = entry;
    setIsDeleteModalOpen(true);
  };

  const removeEntry = async () => {
    await EntriesService.deleteApiEntries({ id: entryId });

    setShouldRedirect(true);

    // await EntriesRepository.Delete(entryId)
    //   .then(() => setShouldRedirect(true))
    //   .catch((error) => console.error(error));

    successModal("Sucessfully removed entry");
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
                <FaRegStickyNote />
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
                <DropdownItem onClick={switchEdit}>
                  {isEditing ? "Close" : "Edit"}
                </DropdownItem>
                <DropdownItem onClick={() => invokeDeleteModal(entry)}>
                  Delete
                </DropdownItem>
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
        <div className="block">
          <p style={{ textAlign: "center", width: "100%" }}>
            Are you sure you want to delete{" "}
            <span class="title">{entry.categoryEntryName} ?</span>
          </p>
        </div>
      </div>
      <div className="menu horizontal center">
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
