import React from "react";
import "./Explorer.scss";

const Explorer = () => {
  return (
    <div className="categories__wrapper">
      <div className="container">
        <div className="left-menu">
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
            <div className="icon">
              <i class="fa fa-folder" aria-hidden="true"></i>
            </div>
            <div className="text ellipsis">I'm some item</div>
            <div className="date">11/11/2020</div>
            <div className="size">100 B</div>
            <div className="actions">
              <i class="fas fa-pen-square"></i>
              <i class="fa fa-times" aria-hidden="true"></i>
            </div>
          </div>
          <div className="item ellipsis">I'm some item</div>
          <div className="item ellipsis">I'm some item</div>
          <div className="item ellipsis">I'm some item</div>
          <div className="item ellipsis">I'm some item</div>
        </div>
      </div>
    </div>
  );
};

export default Explorer;
