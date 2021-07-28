import React, { useContext, useEffect, useRef, useState } from 'react';
import { GetAllCategories, RemoveCategory, UpdateCategory } from '../../api/Categories';
import { AuthContext } from '../../auth/AuthContext';
import { NavLink as div, NavLink } from 'react-router-dom';
import './Categories.css';
import { BasicModal } from '../../components/BasicModal/BasicModal';

const Categories = () => {
    const authContext = useContext(AuthContext);
    const [categories, setCategories] = useState([]);
    const [shouldModalOpen, setShouldModalOpen] = useState(false);
    const editCategory = useRef();

    useEffect(async () => {
        await GetAllCategories(authContext.authState?.token)
            .then(result => {
                var data = result.data;
                data =
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
        setShouldModalOpen(!shouldModalOpen);
    }

    const sendEditRequest = async (categoryId, name) => {
        await UpdateCategory(authContext.authState?.token, categoryId, name)
            .then(result => {
                let index = categories.findIndex((obj => obj.categoryId == categoryId));
                categories[index].name = name;
                // setCategories({...categories})
            })
            .catch((error) => console.log(error));
        setShouldModalOpen(false);
    }

    const closeModal = () => {
        setShouldModalOpen(false);
    }

    return (
        <div className="categories__container">
            {categories ? categories.map((category, index) => (
                <div key={index} className="category">
                    <NavLink to={`/category/${category.name}`} className="category-link">
                        <div className="category-name">{category.name}</div>
                        <div className="category-id">{category.categoryId}</div>
                        <div className="category-date-created">{new Date(category.dateCreated).toISOString().slice(0, 10)}</div>
                    </NavLink>
                    <div className="edit" onClick={() => edit(category)}><i class="fas fa-edit"></i></div>
                    <div className="delete" onClick={() => remove(category.categoryId)}><i class="fa fa-times" aria-hidden="true"></i></div>
                </div>
            ))
                : <>Empty</>
            }
            <BasicModal isOpen={shouldModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeModal}>
                <EditDocument category={editCategory.current} closeModal={closeModal} sendRequest={sendEditRequest} />
            </BasicModal>
        </div>
    )
}

const EditDocument = ({ category, closeModal, sendRequest }) => {
    const modalInput = useRef();
    useEffect(() => modalInput.current = document.getElementById('edit-modal-input'), []);

    return (
        <div className="edit-modal">
            <div className="edit-info">Editing: {category.name}</div>
            <div className="edit-menu">
                <label>New Name: </label>
                <input id="edit-modal-input" type="text" className="basic-input" />
                <div className="edit-menu-buttons">
                    <div className="basic-button accept" onClick={() => sendRequest(category.categoryId, modalInput.current.value)}>Accept</div>
                    <div className="basic-button close" onClick={closeModal}>Close</div>
                </div>
            </div>
        </div>
    )
}

export default Categories;