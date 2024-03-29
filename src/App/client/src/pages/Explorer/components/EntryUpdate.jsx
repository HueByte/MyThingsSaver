import React from "react";
import { useState } from "react";
import { EntriesService } from "../../../api";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { errorModal, warningModal } from "../../../core/Modals";

const EntryUpdate = ({
  isActive,
  setIsActive,
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

    let result = await EntriesService.patchApiEntries({
      requestBody: {
        entryId: entryToEdit.id,
        categoryId: entryToEdit.categoryId,
        entryName: name,
      },
    });

    if (!result?.isSuccess) {
      errorModal("Something went wrong while updating entry");
      return;
    }

    let newEntries = [...entries];
    let index = newEntries.findIndex((obj) => obj.id == entryToEdit.id);

    newEntries[index].name = name;
    setEntries(newEntries);

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
            <div className="block vertical">
              <div className="field-name">New name</div>
              <input
                id="entry-name-input"
                type="text"
                className="mts-input field-input"
                placeholder={entryToEdit ? entryToEdit.name : ""}
                autoComplete="off"
                onInput={(e) => setName(e.target.value)}
              />
            </div>
          </div>
          <div className="menu horizontal center">
            <div className="mts-button item" onClick={sendRequest}>
              Update
            </div>
            <div className="mts-button item" onClick={closeModal}>
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
