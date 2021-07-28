import React, { useContext, useEffect, useState } from 'react';
import { GetAllCategories, RemoveCategory } from '../../api/Categories';
import { AuthContext } from '../../auth/AuthContext';
import { NavLink as div, NavLink } from 'react-router-dom';
import './Categories.css';

const Categories = () => {
    const authContext = useContext(AuthContext);
    const [categories, setCategories] = useState([]);

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

    return (
        <div className="categories__container">
            {categories ? categories.map((category, index) => (
                <div key={index} className="category">
                    <NavLink to={`/category/${category.name}`} className="category-link">
                        <div className="category-name">{category.name}</div>
                        <div className="category-id">{category.categoryId}</div>
                        <div className="category-date-created">{new Date(category.dateCreated).toISOString().slice(0, 10)}</div>
                    </NavLink>
                    <div className="delete" onClick={() => remove(category.categoryId)}><i class="fa fa-times" aria-hidden="true"></i></div>
                </div>
            ))
                : <>Empty</>
            }
        </div>
    )
}

export default Categories;