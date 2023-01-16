import React from "react";
import { EntriesService } from "../../../api";
// import EntriesRepository from "../../../api/repositories/EntriesRepository";
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
    await EntriesService.deleteApiEntries({ id: entryToDelete.id });

    let newEntries = entries;
    newEntries = newEntries.filter((entry) => {
      return entry.id !== entryToDelete.id;
    });

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
            <div className="block">
              <p style={{ textAlign: "center", width: "100%" }}>
                Are you sure you want to delete{" "}
                <span className="title">
                  {entryToDelete ? entryToDelete.name : ""}
                </span>
              </p>
            </div>
          </div>
          <div className="menu horizontal center">
            <div className="mts-button item" onClick={sendRequest}>
              Yes
            </div>
            <div className="mts-button item" onClick={closeModal}>
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
