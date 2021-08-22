import React from "react";

const Loader = () => {
  return (
    <div className="loader" style={loaderClass}>
      <div className="lds-ring">
        <div></div>
        <div></div>
        <div></div>
        <div></div>
      </div>
    </div>
  );
};

const loaderClass = {
  width: "100%",
  height: "100%",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  zIndex: "9999",
};

export default Loader;
