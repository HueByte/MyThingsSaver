import React, { createContext, useContext, useEffect, useState } from 'react';
import { AddCategory, GetAllCategories, RemoveCategory, UpdateCategory } from '../api/Categories';
import { AuthContext } from '../auth/AuthContext';
import Loader from '../components/Loaders/Loader';
import { warningModal } from '../core/Modals';

const CategoryContext = createContext();

const CategoryProvider = ({ children }) => {
    const authContext = useContext(AuthContext);
    const [categories, setCategories] = useState([]);
    const [isFetching, setIsFetching] = useState(true);

    // fetch categories on init
    useEffect(async () => {
        if (authContext.isAuthenticated())
            await ContextGetAllCategories().then(result => setCategories(result));
        setIsFetching(false);
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

    // TODO: Edited entity doesn't update on sidebar
    async function ContextEditCategory(categoryId, newName) {
        if (newName.length === 0)
            return;

        await UpdateCategory(authContext.authState?.token, categoryId, newName)
            .then(() => {
                // needed for quick refresh of data
                let newCategories = [...categories];
                let index = newCategories.findIndex((obj => obj.categoryId == categoryId));
                newCategories[index].name = newName;

                setCategories(newCategories);
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

    if (isFetching) return <Loader />

    return (
        <CategoryContext.Provider value={value}>
            {children}
        </CategoryContext.Provider>
    )
}

export { CategoryContext, CategoryProvider }