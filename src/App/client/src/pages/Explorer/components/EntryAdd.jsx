import { useState } from "react";
import { EntriesService } from "../../../api";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { errorModal, warningModal } from "../../../core/Modals";

const EntryAdd = ({ isActive, setIsActive, categoryId, setEntries }) => {
  const [entryName, setEntryName] = useState();

  const sendRequest = async () => {
    if (entryName?.length === 0) {
      warningModal("Name cannot be empty");
      setIsActive(false);
      return;
    }

    let result = await EntriesService.postApiEntries({
      requestBody: {
        entryName: entryName,
        categoryId: categoryId,
      },
    });

    if (!result?.isSuccess) {
      errorModal("Something went wrong with adding entry");
      return;
    }

    let newEntries = await EntriesService.getApiEntriesAll({
      categoryId: categoryId,
      withContent: false,
    });

    setEntries(newEntries?.data?.entries);
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
              <div className="field-name">Entry name</div>
              <input
                type="text"
                className="mts-input field-input"
                autoComplete="off"
                onInput={(e) => setEntryName(e.target.value)}
              />
            </div>
          </div>
          <div className="menu horizontal center">
            <div className="mts-button item" onClick={sendRequest}>
              Add
            </div>
            <div className="mts-button item" onClick={closeModal}>
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
