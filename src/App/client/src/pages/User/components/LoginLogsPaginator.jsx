import { useContext, useEffect, useState } from "react";
import {
  AiFillBug,
  AiFillCalendar,
  AiFillCloud,
  AiFillCode,
  AiFillHome,
  AiFillIdcard,
} from "react-icons/ai";
import { useNavigate, useOutletContext, useParams } from "react-router";
import { AdminService, LoginLogService } from "../../../api";
import { Role } from "../../../api/Roles";
import Loader from "../../../components/Loaders/Loader";
import { AuthContext } from "../../../contexts/AuthContext";
import "../pages/LoginLogs.scss";

const LoginLogsPaginatorPage = ({ isAdmin }) => {
  const authContext = useContext(AuthContext);
  const { logsPerPage } = useOutletContext();
  const { page } = useParams();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [logs, setLogs] = useState([{}]);

  useEffect(() => {
    (async () => {
      setIsLoading(true);
      await fetchLogs();
      setIsLoading(false);
    })();
  }, [page]);

  const fetchLogs = async () => {
    let result = {};
    if (isAdmin) {
      try {
        result = await AdminService.getApiAdminLoginLogs({
          page: page,
          pageSize: logsPerPage,
        });
      } catch (err) {
        navigate("1", { replace: true });
      }
    } else {
      result = await LoginLogService.getApiLoginLogPaginated({
        page: page,
        pageSize: logsPerPage,
      });
    }

    if (result.isSuccess) {
      setLogs(result.data);
    }
  };

  return (
    <>
      {isLoading ? (
        <Loader />
      ) : (
        <>
          {logs.map((log) => (
            <div key={log.id} className="log">
              <div className="log-dic">
                <div className="log-key log-id">
                  <AiFillBug /> ID:
                </div>
                <div className="log-value">{log.id}</div>
              </div>
              {isAdmin && authContext.isInRole([Role.Admin]) ? (
                <>
                  <div className="log-dic">
                    <div className="log-key log-user">
                      <AiFillCode /> User ID:
                    </div>
                    <div className="log-value">{log.userId}</div>
                  </div>
                  <div className="log-dic">
                    <div className="log-key log-user">
                      <AiFillIdcard /> Username:
                    </div>
                    <div className="log-value">{log.userName}</div>
                  </div>
                </>
              ) : (
                <></>
              )}
              <div className="log-dic">
                <div className="log-key log-ip">
                  <AiFillCloud /> IP address:
                </div>
                <div className="log-value">{log.ipAddress}</div>
              </div>
              <div className="log-dic">
                <div className="log-key log-date">
                  <AiFillCalendar /> Date:
                </div>
                <div className="log-value">
                  {new Date(log.loginDate + "Z").toLocaleString()}
                </div>
              </div>
              <div className="log-dic">
                <div className="log-key log-location">
                  <AiFillHome /> Location:
                </div>
                <div className="log-value">{log.location}</div>
              </div>
            </div>
          ))}
        </>
      )}
    </>
  );
};

export default LoginLogsPaginatorPage;
