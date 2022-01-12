import React, { useEffect, useRef } from "react";

const EditEntryModal = ({ category, closeEditModal, sendRequest }) => {
  const modalEditInput = useRef();
  useEffect(() => {
    modalEditInput.current = document.getElementById("edit-modal-input");
    modalEditInput.current.value = category.name;
  }, []);

  const handleEnter = (event) => {
    if (event.key === "Enter")
      sendRequest(category.categoryId, modalEditInput.current.value);
  };

  return (
    <div className="modal">
      <div className="modal-info">Editing: {category.name}</div>
      <div className="modal-menu">
        <label>New Name: </label>
        <input
          id="edit-modal-input"
          type="text"
          className="basic-input"
          onKeyDown={handleEnter}
          autoComplete="off"
        />
        <div className="modal-menu-buttons">
          <div
            className="basic-button accept"
            onClick={() =>
              sendRequest(category.categoryId, modalEditInput.current.value)
            }
          >
            Accept
          </div>
          <div className="basic-button close" onClick={closeEditModal}>
            Close
          </div>
        </div>
      </div>
    </div>
  );
};

export default EditEntryModal;
