import { useEffect, useState } from "react";
import { AdminService, LoginLogService } from "../../../api";
import "./LoginLogs.scss";
import Loader from "../../../components/Loaders/Loader";
import { Outlet, useNavigate, useParams } from "react-router";
import { NavLink } from "react-router-dom";
import { AiFillCaretLeft, AiFillCaretRight } from "react-icons/ai";

const LoginLogsPage = ({ isAdmin }) => {
  const pageSize = 5;
  const { page } = useParams();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    (async () => {
      await fetchLogsCount();
      setIsLoading(false);
    })();
  }, [isAdmin]);

  // executed after the count is fetched
  useEffect(() => {
    if (isLoading) return;
    if (page > totalPages || page <= 0) changePage();
    else document.getElementById(`log-page-${page}`)?.scrollIntoView();
  }, [page, isLoading]);

  const fetchLogsCount = async () => {
    let logsCount = {};

    if (isAdmin) logsCount = await AdminService.getApiAdminLoginLogsCount();
    else logsCount = await LoginLogService.getApiLoginLogCount();

    let totalPages = Math.ceil(logsCount?.data / pageSize);

    setTotalPages(totalPages);

    if (page > totalPages || page <= 0) {
      navigate("1", { replace: true });
    }
  };

  const changePage = (direction = 0) => {
    let newPage = parseInt(page) + direction;

    if (newPage > totalPages || newPage < 1) {
      return;
    }

    navigate(
      isAdmin
        ? `/account/admin/logs/${newPage}`
        : `/account/user/logs/${newPage}`
    );

    document.getElementById(`log-page-${newPage}`).scrollIntoView();
  };

  const renderButtons = () => {
    let buttons = [];

    for (let index = 0; index < totalPages; index++) {
      buttons.push(
        <NavLink
          id={`log-page-${index + 1}`}
          to={isAdmin ? `${index + 1}` : `${index + 1}`}
          key={index}
          className={`mts-button item${page === index + 1 ? "active" : ""}`}
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
            <Outlet
              context={{
                logsPerPage: pageSize,
              }}
            />
          </div>
          <div className="logs-buttons-container">
            <div
              className="mts-button direction-button"
              onClick={() => changePage(-1)}
            >
              <AiFillCaretLeft />
            </div>
            <div className="logs-buttons">{renderButtons()}</div>
            <div
              className="mts-button direction-button"
              onClick={() => changePage(1)}
            >
              <AiFillCaretRight />
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default LoginLogsPage;
