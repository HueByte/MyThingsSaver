import React, { useEffect, useState, useRef, useContext } from 'react';
import { NavLink } from 'react-router-dom';
import { AddCategory, GetAllCategories, RemoveCategory } from '../api/Categories';
import { AuthContext } from '../auth/AuthContext';
import { successModal } from './Modals';
import './SideMenu.css';

const SideMenu = () => {
    const authContext = useContext(AuthContext);
    const [categories, setCategories] = useState([]);
    const categoryInput = useRef();

    useEffect(async () => {
        categoryInput.current = document.getElementById('newCategoryInput');

        await GetAllCategories(authContext.authState?.token)
            .then(result => {
                setCategories(result.data);
            })
            .catch((error) => console.log(error))
    }, [])

    const addNewCategory = async () => {
        // TODO - sort by date
        if (categoryInput.current.value.length === 0) return;
        await AddCategory(authContext.authState?.token, categoryInput.current.value.trim())
            .then(result => {
                setCategories(data => ([...data, { name: categoryInput.current.value.trim() }]));
            })
            .catch((error) => console.log(error))

        categoryInput.current.value = '';
    }


    const inputHandler = (event) => {
        if (event.key === "Enter")
            addNewCategory();
    }

    return (
        <div className="nav-side">
            <div className="nav-side-title">
                <p>Your Categories</p>
            </div>
            <div className="nav-side-controlls">
                <div onClick={addNewCategory} className="basic-button nav-side-button"><i class="fa fa-plus" aria-hidden="true"></i></div>
                <input id="newCategoryInput" onKeyDown={inputHandler} className="basic-input nav-side-input" type="text" placeholder="Category name" />
                {/* <div onClick={toggleMinus} className="basic-button nav-side-button"><i class="fa fa-minus" aria-hidden="true"></i></div> */}
            </div>
            <div className="nav-side__container">
                {categories ? categories.map((category, index) => (
                    <NavLink activeClassName="active" to={`/category/${category.name}`} key={index} className="item">
                        {category.name}
                    </NavLink>
                ))
                    :
                    <div style={{ textAlign: 'center', fontSize: 'large' }}>Empty</div>
                }
            </div>
        </div>
    )
}

export default SideMenu;