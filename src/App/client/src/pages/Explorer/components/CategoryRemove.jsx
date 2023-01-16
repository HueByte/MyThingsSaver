import { useContext } from "react";
import { useState } from "react";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { CategoryContext } from "../../../contexts/CategoryContext";
import { warningModal } from "../../../core/Modals";

const CategoryRemove = ({ isActive, setIsActive, category }) => {
  const categoryContext = useContext(CategoryContext);

  const sendRequest = async () => {
    if (!category.parentCategoryId) {
      await categoryContext.ContextRemoveCategory(category.id);
    } else {
      await categoryContext.ContextRemoveChildCategory(category.id);
    }

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
                <span className="title">{category ? category.name : ""}</span>
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

export default CategoryRemove;
