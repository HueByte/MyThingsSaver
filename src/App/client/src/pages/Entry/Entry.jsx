import { useEffect, useState, useRef } from "react";
import { Navigate, useParams } from "react-router";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import { successModal, warningModal } from "../../core/Modals";
import { BasicModal } from "../../components/BasicModal/BasicModal";
import AwesomeDebouncePromise from "awesome-debounce-promise";
import { DropdownItem, DropdownButton } from "../../components/Dropdown";
import { FaRegStickyNote } from "react-icons/fa";
import { EntriesService } from "../../api";
import { NavLink } from "react-router-dom";

const sendUpdateCallback = async (entryId, newName, data) => {
  await EntriesService.putApiEntries({
    requestBody: {
      entryId: entryId,
      content: data,
      entryName: newName,
    },
  });
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
  const [isPublic, setIsPublic] = useState(false);
  const [publicUrl, setPublicUrl] = useState();
  const entryToDelete = useRef();

  useEffect(() => {
    (async () => {
      let result = await EntriesService.getApiEntries({ id: entryId });

      if (!result.isSuccess) {
        setShouldRedirect(true);
        return;
      }

      setEntry(result.data);
      setIsPublic(result.data.publicEntryId);
      setName(result.data.name);
      setEditValue(result.data.content);
      setPublicUrl(result.data.publicEntry?.publicUrl);
      checkEntrySize(result.data.content.length);
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

  const togglePublic = async () => {
    if (entry) {
      let result = await EntriesService.patchApiEntriesMakePublic({
        requestBody: {
          targetId: entryId,
        },
      });

      setIsPublic(result.data ? true : false);
      setPublicUrl(result.data);
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
    let result = await EntriesService.deleteApiEntries({ id: entryId });

    if (result.isSuccess) setShouldRedirect(true);

    successModal("Sucessfully removed entry");
  };

  const switchEdit = () => {
    window.innerWidth > 1024 ? setIsMobileEdit(false) : setIsMobileEdit(true);
    setIsEditing(!isEditing);
  };

  const closeDeleteModal = () => setIsDeleteModalOpen(false);

  const copyLink = () => {
    navigator.clipboard.writeText(
      `${window.location.protocol}//${window.location.hostname}:${window.location.port}/public/${publicUrl}`
    );
    successModal("Copied link to clipboard");
  };

  if (shouldRedirect) return <Navigate to={`/explore/${categoryId}`} />;
  return (
    <>
      {entry ? (
        <div className="animate__fadeIn animate__animated mx-auto flex min-h-full w-[1024px] flex-col items-center gap-6 p-4 lg:w-5/6 md:h-full md:w-full md:gap-0 md:p-0 md:pt-4">
          <div className="mts-border-gradient-r h-16 w-full border-2 md:rounded-none">
            <div className="relative flex h-full w-full flex-row items-center gap-8 rounded-xl bg-altBackgroundColor p-2">
              <div className="grid place-items-center text-4xl text-neutralDarker">
                <FaRegStickyNote />
              </div>
              <div className="max-w-sm truncate text-lg">{name}</div>
              <div className="text-accent7 md:invisible">
                {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
              </div>
              <div className="absolute right-10 grid place-items-center text-2xl">
                <DropdownButton>
                  <DropdownItem onClick={switchEdit}>
                    {isEditing ? "Close" : "Edit"}
                  </DropdownItem>
                  <DropdownItem onClick={() => invokeDeleteModal(entry)}>
                    Delete
                  </DropdownItem>
                  {isPublic ? (
                    <DropdownItem onClick={copyLink}>Copy Link</DropdownItem>
                  ) : (
                    <></>
                  )}
                </DropdownButton>
              </div>
            </div>
          </div>
          <div className="mb-4 min-h-[700px] w-full rounded-xl bg-altBackgroundColor px-4 pt-8 pb-16 shadow-lg shadow-element lg:w-full md:h-[calc(100%_-_4rem)] md:rounded-none">
            {isEditing ? (
              <div className="flex w-full flex-col gap-4">
                <div
                  className="mts-btn-primary mts-bg-gradient-r w-fit self-end"
                  onClick={switchEdit}
                >
                  Close Editor
                </div>
                <div className="flex flex-col gap-6 rounded-xl bg-backgroundColor p-4">
                  <div className="flex flex-row items-center gap-2">
                    <div className="text-lg">Name:</div>
                    <input
                      type="text"
                      className="mts-input"
                      placeholder={`${name}`}
                      onChange={handleChange}
                      autoComplete="off"
                    />
                  </div>
                  <div className="flex flex-row items-center gap-2">
                    <label>Is Public?</label>
                    <label className="switch-s switch">
                      <input
                        type="checkbox"
                        checked={isPublic}
                        onChange={togglePublic}
                      />
                      <span class="slider"></span>
                    </label>
                  </div>
                  {isPublic ? (
                    <div className="flex flex-row items-center gap-2">
                      <label>
                        Public Link:{" "}
                        <NavLink to={`/public/${publicUrl}`}>
                          {publicUrl}
                        </NavLink>
                      </label>
                    </div>
                  ) : (
                    <> </>
                  )}
                </div>
                <MEDitor
                  value={editValue}
                  onChange={autoSave}
                  commands={[]}
                  height={500}
                  preview={isMobileEdit ? "edit" : "live"}
                />
              </div>
            ) : (
              <div className="mx-auto w-[724px] lg:w-full">
                <MEDitor.Markdown source={editValue} />
              </div>
            )}
          </div>
        </div>
      ) : (
        <>
          <Loader />
        </>
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
    </>
  );
};

const DeleteModal = ({ entry, onDelete, closeDeleteModal }) => {
  return (
    <>
      <div className="content">
        <div className="block">
          <p style={{ textAlign: "center", width: "100%" }}>
            Are you sure you want to delete{" "}
            <span className="title">{entry.name} ?</span>
          </p>
        </div>
      </div>
      <div className="menu horizontal center">
        <div
          className="mts-button item"
          onClick={() => onDelete(entry.entryId)}
        >
          Yes
        </div>
        <div className="mts-button item" onClick={closeDeleteModal}>
          No
        </div>
      </div>
    </>
  );
};

export default Entry;
