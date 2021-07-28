import React, { useContext, useEffect, useRef, useState } from 'react';
import { GetAllCategories, RemoveCategory } from '../../api/Categories';
import { AuthContext } from '../../auth/AuthContext';
import { NavLink as div, NavLink } from 'react-router-dom';
import './Categories.css';
import { BasicModal } from '../../components/BasicModal/BasicModal';

const Categories = () => {
    const authContext = useContext(AuthContext);
    const [categories, setCategories] = useState([]);
    const [modalIsOpen, setIsOpen] = useState(false);
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

    const edit = async (category) => {
        editCategory.current = category;
        setIsOpen(!modalIsOpen);
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
                    <div className="edit" onClick={() => edit(category)}><i class="fa fa-times" aria-hidden="true"></i></div>
                    <div className="delete" onClick={() => remove(category.categoryId)}><i class="fa fa-times" aria-hidden="true"></i></div>
                </div>
            ))
                : <>Empty</>
            }
            <BasicModal isOpen={modalIsOpen}>
                <EditDocument category={editCategory.current} />
            </BasicModal>
        </div>
    )
}

const EditDocument = ({ category }) => {
    return (
        <>
            <p>Editing Category: {category.name}</p>
            <label>New Name:</label>
            <input type="text" className="basic-input" />
        </>
    )
}

export default Categories;