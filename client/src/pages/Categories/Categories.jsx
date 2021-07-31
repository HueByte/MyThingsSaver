import React, { useContext, useEffect, useRef, useState } from 'react';
import { AuthContext } from '../../auth/AuthContext';
import { NavLink as div, NavLink } from 'react-router-dom';
import './Categories.css';
import { BasicModal } from '../../components/BasicModal/BasicModal';
import { warningModal } from '../../core/Modals';
import Loader from '../../components/Loaders/Loader';
import { CategoryContext } from '../../contexts/CategoryContext';

const Categories = () => {
    const authContext = useContext(AuthContext);
    const categoryContext = useContext(CategoryContext);
    const [isFetching, setIsFetching] = useState(true);

    // add modal
    const [shouldEditModalOpen, setShouldEditModalOpen] = useState(false);

    // edit modal
    const [shouldAddModalOpen, setShouldAddModalOpen] = useState(false);
    const categoryEditable = useRef();


    useEffect(async () => setIsFetching(false), []);

    // Remove category
    const remove = async (id) => await categoryContext.ContextRemoveCategory(id);

    // Edit category
    const invokeEditModal = (category) => {
        categoryEditable.current = category;
        setShouldEditModalOpen(!shouldEditModalOpen);
    }

    const sendEditRequest = async (categoryId, name) => {
        if (name.length === 0) {
            warningModal('Name cannot be empty');
            setShouldEditModalOpen(false);
            return;
        }

        await categoryContext.ContextEditCategory(categoryId, name);

        setShouldEditModalOpen(false);
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

        setShouldAddModalOpen(false);
    }

    const closeEditModal = () => setShouldEditModalOpen(false);

    const closeAddModal = () => setShouldAddModalOpen(false);

    return (
        <div className="categories__container">
            {isFetching ? <Loader />
                : <>
                    <div className="category add-new" onClick={invokeAddModal}><i class="fa fa-plus" aria-hidden="true"></i></div>
                    {categoryContext.categories ? categoryContext.categories.map((category, index) => (
                        <div key={index} className="category">
                            <NavLink to={`/category/${category.name}`} className="category-link">
                                <div className="category-name">{category.name}</div>
                                <div className="category-id">ID: {category.categoryId}</div>
                                <div className="category-date-created">Date Created: {new Date(category.dateCreated).toISOString().slice(0, 10)}</div>
                            </NavLink>
                            <div className="edit" onClick={() => invokeEditModal(category)}><i class="fas fa-pen-square"></i></div>
                            <div className="delete" onClick={() => remove(category.categoryId)}><i class="fa fa-times" aria-hidden="true"></i></div>
                        </div>
                    ))
                        : <>Empty</>
                    }
                </>
            }
            <BasicModal isOpen={shouldEditModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeEditModal}>
                <EditDocument category={categoryEditable.current} closeEditModal={closeEditModal} sendRequest={sendEditRequest} />
            </BasicModal>
            <BasicModal isOpen={shouldAddModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeAddModal}>
                <AddDocument closeAddModal={closeAddModal} sendRequest={sendAddRequest} />
            </BasicModal>
        </div>
    )
}

const EditDocument = ({ category, closeEditModal, sendRequest }) => {
    const modalEditInput = useRef();
    useEffect(() => modalEditInput.current = document.getElementById('edit-modal-input'), []);

    return (
        <div className="modal">
            <div className="modal-info">Editing: {category.name}</div>
            <div className="modal-menu">
                <label>New Name: </label>
                <input id="edit-modal-input" type="text" className="basic-input" />
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

    return (
        <div className="modal">
            <div className="modal-info"></div>
            <div className="modal-menu">
                <label>Name: </label>
                <input id="add-modal-input" type="text" className="basic-input" />
                <div className="modal-menu-buttons">
                    <div className="basic-button" onClick={() => sendRequest(modalAddInput.current.value)}>Accept</div>
                    <div className="basic-button" onClick={closeAddModal}>Close</div>
                </div>
            </div>
        </div>
    )
}

export default Categories;