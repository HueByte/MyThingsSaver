import { useContext } from "react";
import { useState } from "react";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { CategoryContext } from "../../../contexts/CategoryContext";
import { warningModal } from "../../../core/Modals";

const CategoryRemove = ({ isActive, setIsActive, category }) => {
  const categoryContext = useContext(CategoryContext);

  const sendRequest = async () => {
    await categoryContext.ContextRemoveCategory(category.categoryId);

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
            Are you sure you want to delete{" "}
            <span className="title">{category ? category.name : ""}</span>
          </div>
          <div className="menu horizontal">
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

export default CategoryRemove;
