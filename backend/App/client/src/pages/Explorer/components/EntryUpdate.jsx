import React from "react";
import { useState } from "react";
import EntriesRepository from "../../../api/repositories/EntriesRepository";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { errorModal, warningModal } from "../../../core/Modals";

const EntryUpdate = ({
  isActive,
  setIsActive,
  auth,
  entryToEdit,
  entries,
  setEntries,
}) => {
  const [name, setName] = useState("");

  const sendRequest = async () => {
    if (name.length === 0) {
      warningModal("Name cannot be empty");
      setIsActive(false);
      return;
    }

    await EntriesRepository.UpdateWithoutContent(
      entryToEdit.categoryEntryId,
      name,
      entryToEdit.categoryId
    )
      .then(() => {
        let newEntries = [...entries];
        let index = newEntries.findIndex(
          (obj) => obj.categoryEntryId == entryToEdit.categoryEntryId
        );

        newEntries[index].categoryEntryName = name;
        setEntries(newEntries);
      })
      .catch(() => errorModal("Something went wrong while updating entry"));

    setIsActive(false);
  };

  const closeModal = () => setIsActive(false);

  return (
    <BasicModal
      isOpen={isActive}
      shouldCloseOnOverlayClick={true}
      onRequestClose={closeModal}
    >
      {isActive ? (
        <>
          <div className="content">
            <div className="field-name">New entry name</div>
            <input
              id="entry-name-input"
              type="text"
              className="basic-input field-input"
              placeholder={entryToEdit ? entryToEdit.categoryEntryName : ""}
              autoComplete="off"
              onInput={(e) => setName(e.target.value)}
            />
          </div>
          <div className="menu horizontal">
            <div className="basic-button item" onClick={sendRequest}>
              Update
            </div>
            <div className="basic-button item" onClick={closeModal}>
              Cancel
            </div>
          </div>
        </>
      ) : (
        <></>
      )}
    </BasicModal>
  );
};

export default EntryUpdate;
