import { useEffect, useState } from "react";
import { LoginLogService } from "../../../api";
import "./LoginLogs.scss";
import Loader from "../../../components/Loaders/Loader";
import { Outlet, useNavigate, useParams } from "react-router";
import { NavLink } from "react-router-dom";

const LoginLogsPage = () => {
  const pageSize = 5;
  const { page } = useParams();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    (async () => {
      let logsCount = await LoginLogService.getApiLoginLogCount();
      let totalPages = Math.ceil(logsCount.data / pageSize);

      setTotalPages(totalPages);

      if (page > totalPages || page <= 0) {
        navigate("1", { replace: true });
      }

      setIsLoading(false);
    })();
  }, []);

  const renderButtons = () => {
    let buttons = [];

    for (let index = 0; index < totalPages; index++) {
      buttons.push(
        <NavLink
          to={`${index + 1}`}
          key={index}
          className={`mts-button item ${page === index + 1 ? "active" : ""}`}
        >
          {index + 1}
        </NavLink>
      );
    }

    return buttons;
  };

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
            <Outlet context={{ logsPerPage: pageSize }} />
          </div>
          <div className="action-buttons">{renderButtons()}</div>
        </div>
      )}
    </>
  );
};

export default LoginLogsPage;
