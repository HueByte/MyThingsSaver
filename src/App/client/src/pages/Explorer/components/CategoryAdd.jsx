import { useContext } from "react";
import { useState } from "react";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { CategoryContext } from "../../../contexts/CategoryContext";
import { warningModal } from "../../../core/Modals";

const CategoryAdd = ({ isActive, setIsActive, parentCategory }) => {
  const categoryContext = useContext(CategoryContext);
  const [categoryName, setCategoryName] = useState();

  const sendRequest = async () => {
    if (categoryName?.length === 0) {
      warningModal("Name cannot be empty");
      setIsActive(false);
      return;
    }

    await categoryContext.ContextAddCategory(categoryName, parentCategory?.id);

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
              <div className="field-name">Category Name</div>
              <input
                type="text"
                className="basic-input field-input"
                autoComplete="off"
                onInput={(e) => setCategoryName(e.target.value)}
              />
            </div>
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

export default CategoryAdd;
