const DropdownItem = ({ onClick, children }) => {
  return (
    <div className="option" onClick={onClick}>
      {children}
    </div>
  );
};

export default DropdownItem;
