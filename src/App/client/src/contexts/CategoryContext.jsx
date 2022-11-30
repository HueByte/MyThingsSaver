import React, { createContext, useContext, useEffect, useState } from "react";
import { CategoriesService } from "../api/services/CategoriesService.ts";
// import CategoriesRepository from "../api/repositories/CategoriesRepository";
import { AuthContext } from "../auth/AuthContext";
import Loader from "../components/Loaders/Loader";

const CategoryContext = createContext();

const CategoryProvider = ({ children }) => {
  const authContext = useContext(AuthContext);
  const [categories, setCategories] = useState([]);
  const [isFetching, setIsFetching] = useState(true);
  // const [subCategories, setSubCategories] = useState([]);

  // fetch categories on init
  useEffect(() => {
    (async () => {
      if (authContext.isAuthenticated()) {
        await ContextGetAllCategories().then((result) => setCategories(result));
      }
    })();

    setIsFetching(false);
  }, []);

  async function ContextAddCategory(name, parentId) {
    if (name.length === 0) return;

    let result = await CategoriesService.postApiCategories({
      requestBody: {
        categoryParentId: parentId,
        name: name.trim(),
      },
    });

    let categoriesResult = await ContextGetAllCategories();
    setCategories(categoriesResult);

    // return await CategoriesRepository.Add(name.trim(), parentId)
    //   .then((result) => {
    //     ContextGetAllCategories().then((result) => setCategories(result));
    //     return result?.isSuccess;
    //   })
    //   .catch((error) => console.error(error));
  }

  async function ContextRemoveCategory(categoryId) {
    let result = await CategoriesService.deleteApiCategories({
      requestBody: { categoryId: categoryId },
    });

    let newCategories = categories.filter((category) => {
      return category.categoryId !== categoryId;
    });

    setCategories(newCategories);

    return result?.isSuccess;

    // return await CategoriesRepository.Remove(categoryId)
    //   .then(async (result) => {
    //     let newCategories = categories;
    //     newCategories = newCategories.filter((category) => {
    //       return category.categoryId !== categoryId;
    //     });

    //     setCategories(newCategories);
    //     return result?.isSuccess;
    //   })
    //   .catch((error) => console.error(error));
  }

  // TODO: is it necessary?
  async function ContextRemoveChildCategory(categoryId) {
    await CategoriesService.deleteApiCategories({
      requestBody: { categoryId: categoryId },
    });

    let newCategogries = await ContextGetAllCategories();

    setCategories(newCategogries);

    // await CategoriesRepository.Remove(categoryId);

    // let newCategogries = await ContextGetAllCategories();
    // setCategories(newCategogries);
  }

  // TODO: Edited entity doesn't update on sidebar
  async function ContextEditCategory(categoryId, newName) {
    if (newName.length === 0) return;

    let result = await CategoriesService.putApiCategories({
      requestBody: {
        categoryId: categoryId,
        name: newName,
      },
    });

    let newCategories = [...categories];
    let index = newCategories.findIndex((obj) => obj.categoryId == categoryId);

    newCategories[index].name = newName;

    setCategories(newCategories);

    return result?.isSuccess;

    // return await CategoriesRepository.Update(categoryId, newName)
    //   .then((result) => {
    //     // needed for quick refresh of data
    //     let newCategories = [...categories];
    //     let index = newCategories.findIndex(
    //       (obj) => obj.categoryId == categoryId
    //     );

    //     newCategories[index].name = newName;

    //     setCategories(newCategories);
    //     return result?.isSuccess;
    //   })
    //   .catch((error) => console.log(error));
  }

  async function ContextEditChildCategory(
    categoryId,
    parentCategoryId,
    newName
  ) {
    if (newName.length === 0) return;

    let result = await CategoriesService.putApiCategories({
      requestBody: {
        categoryId: categoryId,
        categoryParentId: parentCategoryId,
        name: newName,
      },
    });

    let newCategories = await ContextGetAllCategories();

    setCategories(newCategories);

    // await CategoriesRepository.Update(categoryId, newName);

    // let newCategories = await ContextGetAllCategories();
    // setCategories(newCategories);
  }

  async function ContextGetAllCategories() {
    let result = await CategoriesService.getApiCategoriesAll();

    return result?.data;

    // let result = await CategoriesRepository.GetAll()
    //   .then((result) => {
    //     return result?.data;
    //   })
    //   .catch((error) => console.error(error));

    // return result;
  }

  async function ContextGetAllRootCategories() {
    let result = await CategoriesService.getApiCategoriesAllRoot();

    return result?.data;

    // let result = await CategoriesRepository.GetRoot()
    //   .then((result) => {
    //     return result?.data;
    //   })
    //   .catch((error) => console.error(error));

    // return result;
  }

  async function ContextGetAllSubCategories(parentID) {
    let result = await CategoriesService.getApiCategoriesAllSub();

    return result?.data;

    // let result = await CategoriesRepository.GetSub(parentID)
    //   .then((result) => {
    //     return result?.data;
    //   })
    //   .catch((error) => console.error(error));

    // return result;
  }

  const value = {
    categories,
    setCategories: (categoryData) => setCategories(categoryData),
    ContextAddCategory,
    ContextEditCategory,
    ContextEditChildCategory,
    ContextRemoveCategory,
    ContextRemoveChildCategory,
    ContextGetAllCategories,
    ContextGetAllRootCategories,
    ContextGetAllSubCategories,
  };

  if (isFetching) return <Loader />;

  return (
    <CategoryContext.Provider value={value}>
      {children}
    </CategoryContext.Provider>
  );
};

export { CategoryContext, CategoryProvider };
