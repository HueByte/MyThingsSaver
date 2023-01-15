import { useEffect, useState } from "react";
import {
  AiFillBug,
  AiFillCalendar,
  AiFillCloud,
  AiFillHome,
} from "react-icons/ai";
import { useOutletContext, useParams } from "react-router";
import { LoginLogService } from "../../../api";
import Loader from "../../../components/Loaders/Loader";
import "../pages/LoginLogs.scss";

const LoginLogsPaginatorPage = () => {
  const { logsPerPage } = useOutletContext();
  const { page } = useParams();
  const [isLoading, setIsLoading] = useState(true);
  const [logs, setLogs] = useState([{}]);

  useEffect(() => {
    (async () => {
      let result = await LoginLogService.getApiLoginLogPage({
        page: page,
        pageSize: logsPerPage,
      });

      if (result.isSuccess) {
        setLogs(result.data);
      }

      setIsLoading(false);
    })();
  }, [page]);

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
                  <AiFillBug /> ID:{" "}
                </div>
                <div className="log-value">{log.id}</div>
              </div>
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
                  {new Date(log.loginDate).toLocaleString()}
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
