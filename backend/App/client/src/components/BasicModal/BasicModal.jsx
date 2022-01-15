import React, { Children, useState } from "react";
import ReactModal from "react-modal";
import "./BasicModal.scss";

const customStyles = {
  overlay: {
    zIndex: "999",
    backgroundColor: "rgba(1,1,1,0)",
    backdropFilter: "blur(2px)",
  },
  content: {
    top: "50%",
    left: "50%",
    right: "auto",
    bottom: "auto",
    marginRight: "-50%",
    transform: "translate(-50%, -50%)",
    backgroundColor: "rgb(27,27,27)",
    border: "1px solid #a7a7a7",
    borderRadius: "10px",
  },
};

export const BasicModal = ({ children, ...params }) => {
  return (
    <ReactModal style={customStyles} {...params}>
      <div className="basic-modal">{children}</div>
    </ReactModal>
  );
};
