import React, { useContext, useEffect, useRef, useState } from "react";
import { useParams } from "react-router";
import { NavLink } from "react-router-dom";
import { AddOneEntry, DeleteOneEntry, GetAllEntries } from "../../api/Entries";
import { AuthContext } from "../../auth/AuthContext";
import { BasicModal } from "../../components/BasicModal/BasicModal";
import { CategoryContext } from "../../contexts/CategoryContext";
import { successModal, warningModal } from "../../core/Modals";
import "./Category.css";

const Category = () => {
  const authContext = useContext(AuthContext);
  const categoryContext = useContext(CategoryContext);
  const [entries, setEntries] = useState([]);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEntryDeleteModalOpen, setIsDeleteEntryModalOpen] = useState(false);
  const [isDeleteCategoryModalOpen, setIsDeleteCategoryModalOpen] =
    useState(false);
  const entryToDelete = useRef();
  const categoryToDelete = useRef();
  const {
    categoryId: categoryName,
    subCategoryId: subCategoryName,
    entryId: mainCategoryId,
  } = useParams();
  const isSubCategory = subCategoryName ? true : false;

  useEffect(async () => {
    await GetAllEntries(authContext.authState?.token, mainCategoryId)
      .then((result) => setEntries(result.data))
      .catch((error) => console.error(error));
  }, [mainCategoryId]);

  // add entry
  const addEntry = async (name) => {
    if (name.length === 0) {
      warningModal("Name cannot be empty");
      setIsAddModalOpen(false);
      return;
    }

    await AddOneEntry(authContext.authState?.token, name, mainCategoryId)
      .then(async () => {
        await GetAllEntries(authContext.authState?.token, mainCategoryId)
          .then((result) => setEntries(result.data))
          .catch((error) => console.error(error));
      })
      .catch((error) => console.error(error));

    setIsAddModalOpen(false);
  };

  // add subcategory
  const addSubCategory = async (name, parentId) => {
    if (name.length === 0) {
      warningModal("Name cannot be empty");
      setIsAddModalOpen(false);
      return;
    }

    let result = await categoryContext
      .ContextAddCategory(name, parentId)
      .then(async () => {
        await GetAllEntries(authContext.authState?.token, mainCategoryId)
          .then((result) => setEntries(result.data))
          .catch((error) => console.error(error));
      });

    if (result) successModal("Created Subcategory");
    setIsAddModalOpen(false);
  };

  // close add modal
  const closeAddModal = () => setIsAddModalOpen(false);

  // Open delete subcategory modal
  const invokeDeleteCategoryModal = (category) => {
    categoryToDelete.current = category;
    setIsDeleteCategoryModalOpen(true);
  };

  // close delete subcategory modal
  const closeDeleteCategoryModal = () => setIsDeleteCategoryModalOpen(false);

  // remove subcategory
  const removeCategory = async (categoryId) => {
    await categoryContext.ContextRemoveCategory(categoryId).then(async () => {
      let newState = entries;
      let newCategories = newState.subCategories.filter((subCategory) => {
        return subCategory.categoryId !== categoryId;
      });

      newState.subCategories = newCategories;
      setEntries(newState);
      successModal("Successfully removed subcategory");
    });

    setIsDeleteCategoryModalOpen(false);
  };

  // open delete entry modal
  const invokeEntryDeleteModal = (entry) => {
    entryToDelete.current = entry;
    setIsDeleteEntryModalOpen(true);
  };

  // close delete entry modal
  const closeEntryDeleteModal = () => setIsDeleteEntryModalOpen(false);

  // delete entry
  const removeEntry = async (entryId) => {
    await DeleteOneEntry(authContext.authState?.token, entryId)
      .then(async () => {
        let newState = entries;
        console.log(newState);
        let newEntries = newState.categoryEntries.filter((entry) => {
          return entry.categoryEntryId !== entryId;
        });

        newState.categoryEntries = newEntries;
        setEntries(newState);
        successModal("Successfully removed entry");
      })
      .catch((error) => console.error(error));

    closeEntryDeleteModal();
  };

  return (
    <div className="entries__container enter-animation">
      <div className="entries-menu">
        <div className="category-name">
          <p className="ellipsis" style={{ textAlign: "start" }}>
            {subCategoryName
              ? `${categoryName} / ${subCategoryName}`
              : `${categoryName}`}
          </p>
        </div>
        <div
          className="basic-button entry-button-add"
          onClick={() => setIsAddModalOpen(true)}
        >
          <i class="fa fa-plus" aria-hidden="true"></i>
        </div>
      </div>
      <div className="entry-label__container">
        <h1 className="entry-label">Entries</h1>
      </div>
      {entries?.categoryEntries?.length > 0 ? (
        entries.categoryEntries.map((entry, index) => (
          <div className="entry" key={index}>
            <div className="entry-image">
              {entry.image && entry.image.length !== 0 ? (
                <img src={`${entry.image}`} />
              ) : (
                <i class="fas fa-sticky-note"></i>
              )}
            </div>
            <NavLink
              className="entry-name"
              to={`/entry/${mainCategoryId}/${entry.categoryEntryId}`}
            >
              <span className="ellipsis">{entry.categoryEntryName}</span>
            </NavLink>
            <div className="entry-date">
              {new Date(entry.lastUpdatedOn + "Z").toLocaleDateString()}
            </div>
            <div className="entry-size">{entry.size} B</div>
            <div className="entry-menu-buttons">
              <NavLink
                to={`/entry/${mainCategoryId}/${entry.categoryEntryId}`}
                className="entry-menu"
              >
                Show
              </NavLink>
              <div
                className="entry-menu"
                onClick={() => invokeEntryDeleteModal(entry)}
              >
                Remove
              </div>
            </div>
          </div>
        ))
      ) : entries?.subCategories?.length == 0 ? (
        <div
          style={{
            padding: "2em",
            display: "grid",
            placeItems: "center",
            width: "100%",
            height: "100%",
            fontSize: "xxx-large",
          }}
        >
          <i class="fas fa-folder-open"></i>
          <span
            style={{
              color: "var(--Rose)",
              fontSize: "x-large",
              textAlign: "center",
            }}
          >
            You haven't got any entries in this category yet
          </span>
        </div>
      ) : (
        <>/</>
      )}
      {entries?.subCategories?.length > 0 ? (
        <>
          <div className="entry-label__container">
            <h1 className="entry-label">Subcategories</h1>
          </div>
          {entries.subCategories.map((subCategory, index) => (
            <div className="entry" key={index}>
              <div className="entry-image">
                <i class="fa fa-folder" aria-hidden="true"></i>
              </div>
              <NavLink
                className="entry-name"
                to={`/category/${categoryName}/${subCategory.name}/${subCategory.categoryId}`}
              >
                <span className="ellipsis">{subCategory.name}</span>
              </NavLink>
              <div className="entry-date">
                {new Date(subCategory.lastEditedOn + "Z").toLocaleDateString()}
              </div>
              {/* <div className="entry-size">{subCategory.size} B</div> */}
              <div className="entry-menu-buttons">
                <NavLink
                  to={`/category/${categoryName}/${subCategory.name}/${subCategory.categoryId}`}
                  className="entry-menu"
                >
                  Show
                </NavLink>
                <div
                  className="entry-menu"
                  onClick={() => invokeDeleteCategoryModal(subCategory)}
                >
                  Remove
                </div>
              </div>
            </div>
          ))}
        </>
      ) : (
        <></>
      )}
      <BasicModal
        isOpen={isAddModalOpen}
        shouldCloseOnOverlayClick={true}
        onRequestClose={closeAddModal}
      >
        <AddModal
          addEntry={addEntry}
          addSubCategory={addSubCategory}
          mainCategoryId={mainCategoryId}
          isSubCategory={isSubCategory}
          closeAddModal={closeAddModal}
        />
      </BasicModal>
      <BasicModal
        isOpen={isEntryDeleteModalOpen}
        shouldCloseOnOverlayClick={true}
        onRequestClose={closeEntryDeleteModal}
      >
        <EntryDeleteModal
          entry={entryToDelete.current}
          onDelete={removeEntry}
          closeDeleteModal={closeEntryDeleteModal}
        />
      </BasicModal>
      <BasicModal
        isOpen={isDeleteCategoryModalOpen}
        shouldCloseOnOverlayClick={true}
        onRequestClose={closeDeleteCategoryModal}
      >
        <DeleteCategoryModal
          category={categoryToDelete.current}
          onDelete={removeCategory}
          closeDeleteModal={closeDeleteCategoryModal}
        />
      </BasicModal>
    </div>
  );
};

const AddModal = ({
  addEntry,
  addSubCategory,
  mainCategoryId,
  isSubCategory,
  closeAddModal,
}) => {
  const [isEntry, setIsEntry] = useState(true);
  const entryName = useRef();

  useEffect(
    () => (entryName.current = document.getElementById("add-name-input")),
    []
  );

  const handleEnter = (event) => {
    if (event.key === "Enter") sendRequest(entryName.current.value);
  };

  const switchType = (senderIsEntry) => {
    setIsEntry(senderIsEntry);
  };

  const sendRequest = (name) => {
    if (isEntry) addEntry(name);
    else addSubCategory(name, mainCategoryId);
  };

  return (
    <div className="modal-container">
      {!isSubCategory ? (
        <div className="modal-menu">
          <div className="basic-button" onClick={() => switchType(false)}>
            SubCategory
          </div>

          <div className="basic-button" onClick={() => switchType(true)}>
            Entry
          </div>
        </div>
      ) : (
        <></>
      )}
      <div className="modal-field">
        <div className="field-name">
          {isEntry ? "Entry name" : "Subcategory name"}
        </div>
        <input
          id="add-name-input"
          type="text"
          className="basic-input"
          onKeyDown={handleEnter}
          autoComplete="off"
        />
      </div>
      <div className="modal-menu">
        <div
          className="basic-button"
          onClick={() => sendRequest(entryName.current.value)}
        >
          Add
        </div>
        <div className="basic-button" onClick={closeAddModal}>
          Close
        </div>
      </div>
    </div>
  );
};

const EntryDeleteModal = ({ entry, onDelete, closeDeleteModal }) => {
  return (
    <div>
      <p style={{ fontSize: "larger", fontWeight: "bold" }}>
        Are you sure you want to delete{" "}
        <span className="ellipsis" style={{ color: "var(--Rose)" }}>
          {entry.categoryEntryName}
        </span>
      </p>
      <div className="modal-menu-buttons">
        <div
          className="basic-button accept"
          onClick={() => onDelete(entry.categoryEntryId)}
        >
          Yes
        </div>
        <div className="basic-button close" onClick={closeDeleteModal}>
          No
        </div>
      </div>
    </div>
  );
};

const DeleteCategoryModal = ({ category, onDelete, closeDeleteModal }) => {
  return (
    <div>
      <p style={{ fontSize: "larger", fontWeight: "bold" }}>
        Are you sure you want to delete{" "}
        <span className="ellipsis" style={{ color: "var(--Rose)" }}>
          {category.name}
        </span>
      </p>
      <div className="modal-menu-buttons">
        <div
          className="basic-button accept"
          onClick={() => onDelete(category.categoryId)}
        >
          Yes
        </div>
        <div className="basic-button close" onClick={closeDeleteModal}>
          No
        </div>
      </div>
    </div>
  );
};

export default Category;
