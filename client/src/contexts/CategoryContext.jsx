import React, { createContext, useContext, useEffect, useState } from 'react';
import { AddCategory, GetAllCategories, RemoveCategory, UpdateCategory } from '../api/Categories';
import { AuthContext } from '../auth/AuthContext';
import { warningModal } from '../core/Modals';

const CategoryContext = createContext();

const CategoryProvider = ({ children }) => {
    const authContext = useContext(AuthContext);
    const [categories, setCategories] = useState([]);

    // fetch categories on init
    useEffect(() => {
        ContextGetAllCategories().then(result => setCategories(result));
    }, []);

    async function ContextAddCategory(Name) {
        if (Name.length === 0)
            return;

        await AddCategory(authContext.authState?.token, Name.trim())
            .then(() => {
                ContextGetAllCategories().then(result => setCategories(result));
            })
            .catch((error) => console.error(error));
    }

    async function ContextRemoveCategory(Id) {
        await RemoveCategory(authContext.authState?.token, Id)
            .then(async () => {
                let newCategories = categories;
                newCategories = newCategories.filter(category => {
                    return category.categoryId !== Id;
                })

                setCategories(newCategories);
            })
            .catch((error) => console.error(error));
    }

    async function ContextEditCategory(categoryId, newName) {
        if (newName.length === 0)
            return;

        await UpdateCategory(authContext.authState?.token, categoryId, newName)
            .then(() => {
                let index = categories.findIndex((obj => obj.categoryId == categoryId));
                categories[index].name = newName;
            })
            .catch((error) => console.log(error));
    }

    async function ContextGetAllCategories() {
        let result = await GetAllCategories(authContext.authState?.token).then(result => { return result.data })
            .catch((error) => console.error(error));

        return result;
    }

    const value = {
        categories,
        setCategories: (categoryData) => setCategories(categoryData),
        ContextAddCategory,
        ContextEditCategory,
        ContextRemoveCategory,
        ContextGetAllCategories
    }

    return (
        <CategoryContext.Provider value={value}>
            {children}
        </CategoryContext.Provider>
    )
}

export { CategoryContext, CategoryProvider }