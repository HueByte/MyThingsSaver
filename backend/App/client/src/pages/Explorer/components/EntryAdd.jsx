import React from "react";
import { useState } from "react";
import { useEffect } from "react";
import EntriesRepository from "../../../api/repositories/EntriesRepository";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { warningModal } from "../../../core/Modals";

const EntryAdd = ({ isActive, setIsActive, auth, categoryId, setEntries }) => {
  const [entryName, setEntryName] = useState();

  const sendRequest = async () => {
    console.log(entryName);
    if (entryName?.length === 0) {
      warningModal("Name cannot be empty");
      setIsActive(false);
      return;
    }

    await EntriesRepository.Add(auth?.token, entryName, categoryId)
      .then(async () => {
        await EntriesRepository.GetAll(auth?.token, categoryId)
          .then((result) => setEntries(result?.data?.categoryEntries))
          .catch((error) => console.error(error));
      })
      .catch((error) => console.error(error));

    setIsActive(false);
  };

  const closeModal = () => setIsActive(false);

  return (
    <>
      <BasicModal
        isOpen={isActive}
        shouldCloseOnOverlayClick={true}
        onRequestClose={closeModal}
      >
        {isActive ? (
          <div className="modal-container">
            <div className="modal-field">
              <div className="field-name">Entry name</div>
            </div>
            <input
              id="entry-name-input"
              type="text"
              className="basic-input"
              autoComplete="off"
              onInput={(e) => setEntryName(e.target.value)}
            />
            <div className="modal-menu">
              <div className="basic-button" onClick={sendRequest}>
                Add
              </div>
              <div className="basic-button" onClick={closeModal}>
                Close
              </div>
            </div>
          </div>
        ) : (
          <></>
        )}
      </BasicModal>
    </>
  );
};

export default EntryAdd;
