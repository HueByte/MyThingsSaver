import { useEffect, useState } from "react";
import { AdminService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import DefaultAvatar from "../../../assets/DefaultAvatar.png";
import { getSize } from "../../../core/Lib";
import AwesomeDebouncePromise from "awesome-debounce-promise";

const sendUserGetCallback = async (prompt) => {
  let result = [{}];
  if (!prompt && prompt.length === 0)
    result = await AdminService.getApiAdminManagementUsers();
  else
    result = await AdminService.getApiAdminSearchUser({
      prompt: prompt,
    });

  console.log("Sending request");

  return result;
};

const performUserSearch = AwesomeDebouncePromise(sendUserGetCallback, 500);

const UserManagementPage = () => {
  const [users, setUsers] = useState([{}]);
  const [userPromptLoading, setUserPromptLoading] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    (async () => {
      let result = await AdminService.getApiAdminManagementUsers();
      setUsers(result.data);
      setIsLoading(false);
    })();
  }, []);

  useEffect(() => {
    setUserPromptLoading(false);
  }, [users]);

  const colors = [
    { color: "#00fa9a", fontColor: "#000" },
    { color: "#c62368", fontColor: "#FFF" },
    { color: "#7300ff", fontColor: "#FFF" },
  ];

  const getRoleStyle = () => {
    let colorSet = colors[Math.floor(Math.random() * colors.length)];
    return { backgroundColor: colorSet.color, color: colorSet.fontColor };
  };

  return (
    <div className="flex flex-col gap-4">
      {isLoading ? (
        <div>
          <Loader />
        </div>
      ) : (
        <>
          <UserSearchInput setUsers={setUsers} loading={setUserPromptLoading} />
          <div className="flex h-[calc(10rem_*_4)] flex-col gap-2 overflow-y-auto px-2 md:h-fit">
            {userPromptLoading ? (
              <div className="relative h-full w-full">
                <Loader />
              </div>
            ) : (
              users?.map((user) => (
                <div
                  key={user.id}
                  className="flex h-40 w-full flex-row items-center gap-4 rounded-xl bg-backgroundColor p-4 transition duration-300 hover:cursor-pointer hover:bg-backgroundColorLight"
                >
                  <div className="h-24 w-24 flex-shrink-0">
                    <img
                      src={user.avatarUrl ?? DefaultAvatar}
                      alt="avatar"
                      className="h-full w-full rounded-full"
                    />
                  </div>
                  <div className="flex flex-1 flex-col gap-1 truncate">
                    <div className="mts-text-gradient text-xl">
                      {user.username}
                    </div>
                    <div className="flex flex-row justify-between">
                      <div className="truncate">{user.email ?? "No Email"}</div>
                      <div className="sm:hidden">
                        {new Date(
                          user.accountCreatedDate + "Z"
                        ).toLocaleDateString()}
                      </div>
                    </div>
                    <div className="text-accent7">
                      {getSize(user.accountSize)}
                    </div>
                    <div className="flex flex-row gap-2 font-bold">
                      {user.roles?.length ? (
                        user.roles?.map((role) => (
                          <div
                            className="grid place-items-center rounded-md p-1"
                            style={getRoleStyle()}
                          >
                            {role}
                          </div>
                        ))
                      ) : (
                        <div
                          className="grid place-items-center rounded-md p-1"
                          style={getRoleStyle()}
                        >
                          None
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              ))
            )}
          </div>
        </>
      )}
    </div>
  );
};

const UserSearchInput = ({ setUsers, loading }) => {
  const [userPrompt, setUserPrompt] = useState();
  const [isUserPromptLoading, setIsUserPromptLoading] = useState(false);

  useEffect(() => {
    (async () => {
      loading(true);

      let result = await performUserSearch(userPrompt);
      setUsers(result.data);
    })();
  }, [userPrompt]);

  return (
    <input
      type="text"
      placeholder="Search user"
      className="mts-input mx-2"
      value={userPrompt}
      onChange={(e) => setUserPrompt(e.target.value)}
    />
  );
};

export default UserManagementPage;
