import React from "react";
import { useEffect } from "react";
import { useContext } from "react";
import { CategoryContext } from "../../contexts/CategoryContext";
import "./Explorer.scss";

const Explorer = () => {
  const categoryContext = useContext(CategoryContext);

  useEffect(() => {}, []);

  return (
    <div className="categories__wrapper">
      <div className="container">
        <div className="left-menu border-gradient border-gradient-purple">
          <div className="item ellipsis">cat</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
          <div className="item ellipsis">category something something</div>
        </div>
        <div className="content">
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fa fa-folder" aria-hidden="true"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
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
            </div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fa fa-folder" aria-hidden="true"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
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
            </div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
          <div className="item">
            <div className="information">
              <div className="icon">
                <i class="fa fa-folder" aria-hidden="true"></i>
              </div>
              <div className="text ellipsis">
                I'm some item reeeeeeeeeeeeeeeeeeeeeeeee
              </div>
              <div className="date">11/11/2020</div>
              <div className="size">100 B</div>
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
