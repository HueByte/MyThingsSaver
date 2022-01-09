import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { useContext } from "react";
import { AuthContext } from "../../auth/AuthContext";
import { CategoryContext } from "../../contexts/CategoryContext";
import "./Explorer.scss";

const Explorer = () => {
  const categoryContext = useContext(CategoryContext);
  const [subCategories, setSubCategories] = useState([]);
  const auth = useContext(AuthContext);

  useEffect(() => {
    console.log(categoryContext.categories);
  }, []);

  useEffect(() => console.log(subCategories), [subCategories]);

  // const populateRoot = async (category) => {
  //   // console.log(category);
  //   var result = await categoryContext.ContextGetAllSubCategories(
  //     category.categoryId
  //   );

  //   setSubCategories(result);
  //   console.log(result);
  // };

  return (
    <div className="categories__wrapper">
      <div className="container">
        <div className="left-menu border-gradient border-gradient-purple">
          <div className="item ellipsis">{auth.authState?.username}</div>
          {categoryContext.categories ? (
            <>
              {categoryContext.categories.map((category, index) => {
                if (category.level == 0) {
                  return (
                    <>
                      <Item key={index} category={category} />
                      {categoryContext.categories.map((subCategory, index) => {
                        if (
                          subCategory.parentCategoryId == category.categoryId
                        ) {
                          return <Item key={index} category={subCategory} />;
                        }
                      })}
                    </>
                  );
                }
              })}
            </>
          ) : (
            <></>
          )}
        </div>
        <div className="content">
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fas fa-sticky-note"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
              <div className="size">md</div>
            </div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fas fa-sticky-note"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
              <div className="size">md</div>
            </div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fas fa-sticky-note"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
              <div className="size">md</div>
            </div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fas fa-sticky-note"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
              <div className="size">md</div>
            </div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fas fa-sticky-note"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
              <div className="size">md</div>
            </div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Explorer;

const determineDepth = (category) => {
  if (category.level > 0) {
    const style = {
      marginLeft: `${category.level * 5 + 10}px`,
      border: "none",
    };
    return style;
  }
};

const Item = ({ category }) => {
  return (
    <div className="item ellipsis" style={determineDepth(category)}>
      {category.name}
    </div>
  );
};
