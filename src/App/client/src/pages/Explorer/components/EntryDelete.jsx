import React from "react";
import EntriesRepository from "../../../api/repositories/EntriesRepository";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { errorModal, successModal } from "../../../core/Modals";

const EntryDelete = ({
  isActive,
  setIsActive,
  entryToDelete,
  entries,
  setEntries,
}) => {
  const sendRequest = async () => {
    await EntriesRepository.Delete(entryToDelete.categoryEntryId)
      .then(async () => {
        let newEntries = entries;
        newEntries = newEntries.filter((entry) => {
          return entry.categoryEntryId !== entryToDelete.categoryEntryId;
        });

        setEntries(newEntries);
        successModal("Successfully removed entry");
      })
      .catch(() => errorModal("Removing entry failed"));

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
            <div className="block">
              <p style={{ textAlign: "center", width: "100%" }}>
                Are you sure you want to delete{" "}
                <span className="title">
                  {entryToDelete ? entryToDelete.categoryEntryName : ""}
                </span>
              </p>
            </div>
          </div>
          <div className="menu horizontal center">
            <div className="basic-button item" onClick={sendRequest}>
              Yes
            </div>
            <div className="basic-button item" onClick={closeModal}>
              No
            </div>
          </div>
        </>
      ) : (
        <></>
      )}
    </BasicModal>
  );
};

export default EntryDelete;
