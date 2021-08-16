import React, { useContext, useEffect, useRef, useState } from 'react';
import { AuthContext } from '../../auth/AuthContext';
import { NavLink } from 'react-router-dom';
import './Categories.css';
import { BasicModal } from '../../components/BasicModal/BasicModal';
import { successModal, warningModal } from '../../core/Modals';
import Loader from '../../components/Loaders/Loader';
import { CategoryContext } from '../../contexts/CategoryContext';

const Categories = () => {
    const authContext = useContext(AuthContext);
    const categoryContext = useContext(CategoryContext);
    const [isFetching, setIsFetching] = useState(false); // TODO: support loader for fetching from context
    const lastBuddy = useRef();

    // add modal
    const [shouldEditModalOpen, setShouldEditModalOpen] = useState(false);

    // delete modal
    const [shouldDeleteModalOpen, setShouldDeleteModalOpen] = useState(false);
    const categoryToDelete = useRef();

    // edit modal
    const [shouldAddModalOpen, setShouldAddModalOpen] = useState(false);
    const categoryToEdit = useRef();


    useEffect(() => lastBuddy.current = '', []);

    const getCategoryBuddy = () => {
        let buddy = buddyItems[Math.floor(Math.random() * buddyItems.length)];
        if (buddy === lastBuddy.current)
            return getCategoryBuddy(); // repeat if last buddy is equal to current

        lastBuddy.current = buddy;
        return buddy;
    }

    // Remove category
    const invokeDeleteModal = (category) => {
        categoryToDelete.current = category;
        setShouldDeleteModalOpen(!shouldDeleteModalOpen);
    }

    const remove = async (id) => {
        await categoryContext.ContextRemoveCategory(id);
        successModal('Successfully removed category')
        closeDeleteModal();
    }

    // Edit category
    const invokeEditModal = (category) => {
        categoryToEdit.current = category;
        setShouldEditModalOpen(!shouldEditModalOpen);
    }

    const sendEditRequest = async (categoryId, name) => {
        if (name.length === 0) {
            warningModal('Name cannot be empty');
            setShouldEditModalOpen(false);
            return;
        }

        await categoryContext.ContextEditCategory(categoryId, name);

        closeEditModal();
    }

    // Add category
    const invokeAddModal = () => setShouldAddModalOpen(true);

    const sendAddRequest = async (Name) => {
        if (Name.length === 0) {
            warningModal('Name cannot be empty');
            setShouldAddModalOpen(false);
            return;
        }

        await categoryContext.ContextAddCategory(Name);
        successModal('Created category');
        closeAddModal();
    }

    const closeEditModal = () => setShouldEditModalOpen(false);

    const closeAddModal = () => setShouldAddModalOpen(false);

    const closeDeleteModal = () => setShouldDeleteModalOpen(false);

    return (
        <div className="categories__container enter-animation">
            <>
                <div className="category add-new" onClick={invokeAddModal}><i class="fa fa-plus" aria-hidden="true"></i></div>
                {categoryContext.categories ? categoryContext.categories.map((category, index) => (
                    <div key={index} className="category">
                        <NavLink to={`/category/${category.name}/${category.categoryId}`} className="category-link">
                            <div className="category-name">{category.name}</div>
                            <div className="category-id">ID: {category.categoryId}</div>
                            <div className="category-date-created">Date Created: {new Date(category.dateCreated).toISOString().slice(0, 10)}</div>
                            <div id="buddy" className="category-buddy">
                                {getCategoryBuddy()}
                            </div>
                        </NavLink>
                        <div className="edit" onClick={() => invokeEditModal(category)}><i class="fas fa-pen-square"></i></div>
                        <div className="delete" onClick={() => invokeDeleteModal(category)}><i class="fa fa-times" aria-hidden="true"></i></div>
                    </div>
                ))
                    : <>EMPTY ICON</>
                }
            </>
            <BasicModal isOpen={shouldDeleteModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeDeleteModal}>
                <DeleteDocument category={categoryToDelete.current} closeDeleteModal={closeDeleteModal} onDelete={remove} />
            </BasicModal>
            <BasicModal isOpen={shouldEditModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeEditModal}>
                <EditDocument category={categoryToEdit.current} closeEditModal={closeEditModal} sendRequest={sendEditRequest} />
            </BasicModal>
            <BasicModal isOpen={shouldAddModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeAddModal}>
                <AddDocument closeAddModal={closeAddModal} sendRequest={sendAddRequest} />
            </BasicModal>
        </div>
    )
}

const DeleteDocument = ({ category, onDelete, closeDeleteModal }) => {
    return (
        <div>
            <p style={{ fontSize: 'larger', fontWeight: 'bold' }}>
                Are you sure you want to delete <span className="ellipsis" style={{ color: 'var(--Rose)' }}>{category.name}</span>
            </p>
            <div className="modal-menu-buttons">
                <div className="basic-button accept" onClick={() => onDelete(category.categoryId)}>Yes</div>
                <div className="basic-button close" onClick={closeDeleteModal}>No</div>
            </div>
        </div>
    )
}

const EditDocument = ({ category, closeEditModal, sendRequest }) => {
    const modalEditInput = useRef();
    useEffect(() => {
        modalEditInput.current = document.getElementById('edit-modal-input');
        modalEditInput.current.value = category.name;
    }, []);

    const handleEnter = (event) => {
        if (event.key === "Enter") sendRequest(category.categoryId, modalEditInput.current.value)
    }

    return (
        <div className="modal">
            <div className="modal-info">Editing: {category.name}</div>
            <div className="modal-menu">
                <label>New Name: </label>
                <input id="edit-modal-input" type="text" className="basic-input" onKeyDown={handleEnter} />
                <div className="modal-menu-buttons">
                    <div className="basic-button accept" onClick={() => sendRequest(category.categoryId, modalEditInput.current.value)}>Accept</div>
                    <div className="basic-button close" onClick={closeEditModal}>Close</div>
                </div>
            </div>
        </div>
    )
}

const AddDocument = ({ closeAddModal, sendRequest }) => {
    const modalAddInput = useRef();
    useEffect(() => modalAddInput.current = document.getElementById('add-modal-input'), []);

    const handleEnter = (event) => {
        if (event.key === "Enter") sendRequest(modalAddInput.current.value)
    }

    return (
        <div className="modal">
            <div className="modal-info"></div>
            <div className="modal-menu">
                <label>Name: </label>
                <input id="add-modal-input" type="text" className="basic-input" onKeyDown={handleEnter} />
                <div className="modal-menu-buttons">
                    <div className="basic-button" onClick={() => sendRequest(modalAddInput.current.value)}>Accept</div>
                    <div className="basic-button" onClick={closeAddModal}>Close</div>
                </div>
            </div>
        </div>
    )
}

const buddyItems = ['ヾ(•ω•`)o', '(°ロ°)', '╰（‵□′）╯', '╚(•⌂•)╝', '┗|｀O′|┛', '✪ ω ✪', '(¬‿¬)', '(. ❛ ᴗ ❛.)']

export default Categories;