import { useEffect, useState } from "react";
import { AdminService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";

const UserManagementPage = () => {
  const [users, setUsers] = useState([{}]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    (async () => {
      let result = await AdminService.getApiAdminManagementUsers();
      setUsers(result.data);
      setIsLoading(false);
    })();
  }, []);

  useEffect(() => console.log(users), [users]);

  return (
    <div>
      {isLoading ? (
        <div>
          <Loader />
        </div>
      ) : (
        <>
          {users?.map((user) => (
            <>
              <div key={user.id} className="temp">
                {user.username}
              </div>
            </>
          ))}
        </>
      )}
    </div>
  );
};

export default UserManagementPage;
