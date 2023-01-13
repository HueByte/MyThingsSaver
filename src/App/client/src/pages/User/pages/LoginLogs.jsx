import { useEffect, useState } from "react";
import { LoginLogService } from "../../../api";
import "./LoginLogs.scss";
import Loader from "../../../components/Loaders/Loader";

const LoginLogsPage = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [logs, setLogs] = useState([{}]);

  useEffect(() => {
    (async () => {
      let result = await LoginLogService.getApiLoginLog();

      if (result.isSuccess) {
        setLogs(result.data);
      }

      setIsLoading(false);
    })();
  }, []);
  return (
    <>
      {isLoading ? (
        <div style={{ height: "200px" }}>
          <Loader />
        </div>
      ) : (
        <div className="panel">
          <div className="panel-name">Login logs</div>
          <div className="logs-container">
            {logs.map((log) => (
              <div key={log.id} className="log">
                <div className="log-dic">
                  <div className="log-key log-id">ID: </div>
                  <div className="log-value">{log.id}</div>
                </div>
                <div className="log-dic">
                  <div className="log-key log-ip">IP address:</div>
                  <div className="log-value">{log.ipAddress}</div>
                </div>
                <div className="log-dic">
                  <div className="log-key log-date">Date: </div>
                  <div className="log-value">
                    {new Date(log.loginDate).toDateString()}
                  </div>
                </div>
                <div className="log-dic">
                  <div className="log-key log-location">Location:</div>
                  <div className="log-value">{log.location}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </>
  );
};

export default LoginLogsPage;
