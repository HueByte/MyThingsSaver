import React from "react";
import { useState } from "react";
import EntriesRepository from "../../../api/repositories/EntriesRepository";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { errorModal, warningModal } from "../../../core/Modals";

const EntryAdd = ({ isActive, setIsActive, auth, categoryId, setEntries }) => {
  const [entryName, setEntryName] = useState();

  const sendRequest = async () => {
    if (entryName?.length === 0) {
      warningModal("Name cannot be empty");
      setIsActive(false);
      return;
    }

    await EntriesRepository.Add(auth?.token, entryName, categoryId)
      .then(async () => {
        await EntriesRepository.GetAll(auth?.token, categoryId)
          .then((result) => setEntries(result?.data?.categoryEntries))
          .catch(() =>
            errorModal("Something went wrong while fetching updated entries")
          );
      })
      .catch(() => errorModal("Something went wrong with adding entry"));

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
            <div className="field-name">Entry name</div>
            <input
              id="entry-name-input"
              type="text"
              className="basic-input field-input"
              autoComplete="off"
              onInput={(e) => setEntryName(e.target.value)}
            />
          </div>
          <div className="menu horizontal">
            <div className="basic-button item" onClick={sendRequest}>
              Add
            </div>
            <div className="basic-button item" onClick={closeModal}>
              Close
            </div>
          </div>
        </>
      ) : (
        <></>
      )}
    </BasicModal>
  );
};

export default EntryAdd;
