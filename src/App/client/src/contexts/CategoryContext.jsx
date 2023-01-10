import { createContext, useContext, useEffect, useState } from "react";
import { CategoriesService } from "../api/services/CategoriesService.ts";
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

    if (result.isSuccess) {
      let categoriesResult = await ContextGetAllCategories();
      setCategories(categoriesResult);
    }
  }

  async function ContextRemoveCategory(categoryId) {
    let result = await CategoriesService.deleteApiCategories({
      requestBody: { categoryId: categoryId },
    });

    let newCategories = categories.filter((category) => {
      return category.categoryId !== categoryId;
    });

    setCategories(newCategories);

    return result;
  }

  async function ContextRemoveChildCategory(categoryId) {
    let result = await CategoriesService.deleteApiCategories({
      requestBody: { categoryId: categoryId },
    });

    let newCategogries = await ContextGetAllCategories();

    setCategories(newCategogries);

    return result;
  }

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

    return result;
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

    return result;
  }

  async function ContextGetAllCategories() {
    let result = await CategoriesService.getApiCategoriesAll();

    return result?.data;
  }

  async function ContextGetAllRootCategories() {
    let result = await CategoriesService.getApiCategoriesAllRoot();

    return result?.data;
  }

  async function ContextGetAllSubCategories(parentID) {
    let result = await CategoriesService.getApiCategoriesAllSub();

    return result?.data;
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
