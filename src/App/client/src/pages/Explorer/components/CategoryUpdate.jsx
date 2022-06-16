import { useContext } from "react";
import { useState } from "react";
import { BasicModal } from "../../../components/BasicModal/BasicModal";
import { CategoryContext } from "../../../contexts/CategoryContext";
import { errorModal, warningModal } from "../../../core/Modals";

const CategoryUpdate = ({ isActive, setIsActive, category }) => {
  const categoryContext = useContext(CategoryContext);
  const [categoryName, setCategoryName] = useState();

  const sendRequest = async () => {
    if (!category) {
      errorModal("Something went wrong");
      setIsActive(false);
      return;
    }
    if (category.name == categoryName) {
      setIsActive(false);
      return;
    }

    if (category.parentCategoryId) {
      await categoryContext.ContextEditChildCategory(
        category.categoryId,
        categoryName
      );
    } else {
      await categoryContext.ContextEditCategory(
        category.categoryId,
        categoryName
      );
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
            <div className="block vertical">
              <div className="field-name">Category new name</div>
              <input
                type="text"
                className="basic-input field-input"
                autoComplete="off"
                defaultValue={category.name}
                onInput={(e) => setCategoryName(e.target.value)}
              />
            </div>
          </div>
          <div className="menu horizontal">
            <div className="basic-button item" onClick={sendRequest}>
              Update
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

export default CategoryUpdate;
