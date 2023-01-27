export const capitalizeRole = (role) => {
  return role.charAt(0).toUpperCase() + role.slice(1);
};

export const getSize = (size) => {
  if (size < 1024) return size + " B";
  else if (size < 1024 * 1024) return (size / 1024).toFixed(2) + " KB";
  else if (size < 1024 * 1024 * 1024)
    return (size / 1024 / 1024).toFixed(2) + " MB";
  else return (size / 1024 / 1024 / 1024).toFixed(2) + " GB";
};
