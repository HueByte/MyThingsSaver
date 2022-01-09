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
                      <Item index={index} category={category} />
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

const Item = ({ index, category }) => {
  const [showChilds, setShowChilds] = useState(false);
  return (
    <>
      <div
        key={index}
        className="item ellipsis"
        onClick={() => setShowChilds(!showChilds)}
        style={determineDepth(category)}
      >
        {category.name}
      </div>
      {showChilds ? (
        category.childCategories?.map((subCategory, index) => {
          return <Item index={index} category={subCategory} />;
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
