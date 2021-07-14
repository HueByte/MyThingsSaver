import React, { useEffect, useState, useRef } from 'react';
import { NavLink } from 'react-router-dom';
import './SideMenu.css';

const SideMenu = () => {
    const [categories, setCategories] = useState(null);
    const categoryInput = useRef();

    useEffect(() => {
        // fake feed category
        categoryInput.current = document.getElementById('newCategoryInput');
        setCategories(fakeCategorySeed());
    }, [])

    const inputHandler = (event) => {
        if (event.key === "Enter") 
            addCategory();
    }

    const addCategory = async () => {
        //request to API
        // let newCategory = categoryInput.current.value;
        // setCategories(data => ([...data, newCategory]))
        if(categoryInput.current.value.length === 0) return;
        await setCategories(data => ([...data, categoryInput.current.value]))
        categoryInput.current.value = '';
    }

    return (
        <div className="nav-side">
            <div className="nav-side-title">
                <p>Your Categories</p>
            </div>
            <div className="nav-side-controlls">
                <div onClick={addCategory} className="basic-button nav-side-button"><i class="fa fa-plus" aria-hidden="true"></i></div>
                <input id="newCategoryInput" onKeyDown={inputHandler} className="basic-input nav-side-input" type="text" placeholder="Category name" />
                {/* <div onClick={toggleMinus} className="basic-button nav-side-button"><i class="fa fa-minus" aria-hidden="true"></i></div> */}
            </div>
            <div className="nav-side__container">
                {categories ? categories.map((category, index) => (
                    <NavLink activeClassName="active" to={`/category/${category}`} key={index} className="item">{category}</NavLink>
                ))
                    :
                    <div style={{textAlign: 'center', fontSize: 'large'}}>Empty</div>
                }
            </div>
        </div>
    )
}

const fakeCategorySeed = () => {
    return ['Item', 'Item', 'Cats', 'Cows', 'Dogs', 'Games'];
}

export default SideMenu;