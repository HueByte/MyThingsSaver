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

    let result = await categoryContext.ContextEditChildCategory(
      category.id,
      category.parentCategoryId,
      categoryName
    );

    if (!result?.isSuccess) {
      warningModal(result.errors?.join(".\n"));
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
                className="mts-input field-input"
                autoComplete="off"
                defaultValue={category.name}
                onInput={(e) => setCategoryName(e.target.value)}
              />
            </div>
          </div>
          <div className="menu horizontal">
            <div className="mts-button item" onClick={sendRequest}>
              Update
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

export default CategoryUpdate;
