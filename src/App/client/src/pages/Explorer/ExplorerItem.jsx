import { useEffect } from "react";
import { useState } from "react";
import { FaAngleDown, FaAngleRight } from "react-icons/fa";
import { NavLink } from "react-router-dom";

const Item = ({ category, recentPath, setCurrentContextItem }) => {
  const [showChilds, setShowChilds] = useState(false);

  useEffect(() => {
    if (recentPath?.includes(category.categoryId)) {
      setShowChilds(true);
    }
  }, []);

  return (
    <>
      <NavLink
        to={`/explore/${category.categoryId}`}
        className="item ellipsis"
        style={determineDepth(category)}
        onContextMenu={(e) => {
          e.stopPropagation();
          setCurrentContextItem(category);
        }}
      >
        {category.childCategories ? (
          showChilds ? (
            <FaAngleDown
              onClick={(e) => {
                e.preventDefault();
                setShowChilds(!showChilds);
              }}
            />
          ) : (
            <FaAngleRight
              onClick={(e) => {
                e.preventDefault();
                setShowChilds(!showChilds);
              }}
            />
          )
        ) : (
          <i style={{ marginLeft: "10px" }}></i>
        )}
        {category.name}
      </NavLink>
      {showChilds ? (
        category.childCategories?.map((subCategory) => {
          return (
            <Item
              key={subCategory.categoryId}
              category={subCategory}
              recentPath={recentPath}
              setCurrentContextItem={setCurrentContextItem}
            />
          );
        })
      ) : (
        <></>
      )}
    </>
  );
};

const determineDepth = (category) => {
  if (category.level > 0) {
    const style = {
      marginLeft: `${category.level * 5 + 10}px`,
      border: "none",
    };

    return style;
  }
};

export default Item;
