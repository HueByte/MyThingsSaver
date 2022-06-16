import React from "react";
import ReactModal from "react-modal";
import "./BasicModal.scss";
import theme from "../../styles/_theme.scss";

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
    transform: "translate(-50%, -50%)",
    backgroundColor: theme.background,
    border: `1px solid ${theme.darkerBackground}`,
    borderRadius: "10px",
    width: "90vw",
    maxWidth: "600px",
    minHeight: "20vh",
  },
};

export const BasicModal = ({ children, ...params }) => {
  return (
    <ReactModal style={customStyles} {...params}>
      <div className="basic-modal">{children}</div>
    </ReactModal>
  );
};
