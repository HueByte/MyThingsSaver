import React, { useContext, useEffect, useRef, useState } from 'react';
import { AddCategory, GetAllCategories, RemoveCategory, UpdateCategory } from '../../api/Categories';
import { AuthContext } from '../../auth/AuthContext';
import { NavLink as div, NavLink } from 'react-router-dom';
import './Categories.css';
import { BasicModal } from '../../components/BasicModal/BasicModal';

const Categories = () => {
    const authContext = useContext(AuthContext);
    const [categories, setCategories] = useState([]);
    const [shouldEditModalOpen, setShouldEditModalOpen] = useState(false);
    const [shouldAddModalOpen, setShouldAddModalOpen] = useState(false);
    const editCategory = useRef();

    useEffect(async () => {
        await GetAllCategories(authContext.authState?.token)
            .then(result => {
                setCategories(result.data);
            })
            .catch((error) => console.log(error));
    }, []);

    const remove = async (id) => {
        await RemoveCategory(authContext.authState?.token, id)
            .then(result => {
                let newCategories = categories;
                newCategories = newCategories.filter(category => {
                    return category.categoryId !== id;
                });

                setCategories(newCategories);
            })
            .catch((error) => console.log(error));
    }

    const edit = (category) => {
        editCategory.current = category;
        setShouldEditModalOpen(!shouldEditModalOpen);
    }

    const sendEditRequest = async (categoryId, name) => {
        await UpdateCategory(authContext.authState?.token, categoryId, name)
            .then(result => {
                let index = categories.findIndex((obj => obj.categoryId == categoryId));
                categories[index].name = name;
            })
            .catch((error) => console.log(error));
        setShouldEditModalOpen(false);
    }

    const addNewCategory = () => {
        setShouldAddModalOpen(true);
    }

    const sendAddRequest = async (Name) => {
        if (Name.length === 0) return;
        await AddCategory(authContext.authState?.token, Name.trim())
            .then(async result => {
                await GetAllCategories(authContext.authState?.token)
                    .then(result => {
                        setCategories(result.data)
                    })
                    .catch((error) => console.log(error));
            })
            .catch((error) => console.log(error));

        setShouldAddModalOpen(false);
    }

    const closeEditModal = () => {
        setShouldEditModalOpen(false);
    }

    const closeAddModal = () => {
        setShouldAddModalOpen(false);
    }

    return (
        <div className="categories__container">
            <div className="category add-new" onClick={addNewCategory}><i class="fa fa-plus" aria-hidden="true"></i></div>
            {categories ? categories.map((category, index) => (
                <div key={index} className="category">
                    <NavLink to={`/category/${category.name}`} className="category-link">
                        <div className="category-name">{category.name}</div>
                        <div className="category-id">ID: {category.categoryId}</div>
                        <div className="category-date-created">Date Created: {new Date(category.dateCreated).toISOString().slice(0, 10)}</div>
                    </NavLink>
                    <div className="edit" onClick={() => edit(category)}><i class="fas fa-pen-square"></i></div>
                    <div className="delete" onClick={() => remove(category.categoryId)}><i class="fa fa-times" aria-hidden="true"></i></div>
                </div>
            ))
                : <>Empty</>
            }
            <BasicModal isOpen={shouldEditModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeEditModal}>
                <EditDocument category={editCategory.current} closeEditModal={closeEditModal} sendRequest={sendEditRequest} />
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