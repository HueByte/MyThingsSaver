import { useEffect, useState } from "react";
import Loader from "../../components/Loaders/Loader";
import MEDitor from "@uiw/react-md-editor";
import "./LegalNotice.scss";
import { NavLink } from "react-router-dom";
import { LegalNoticeService } from "../../api";

const LegalNoticePage = () => {
  const [legalNotice, setLegalNotice] = useState("");
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    (async () => {
      let result = await LegalNoticeService.getApiLegalNotice();
      setLegalNotice(result.data);
      setIsLoading(false);
    })();
  }, []);

  return (
    <div className="legal-notice-container">
      <NavLink to="/" className="mts-button gradient-background-r nav-button">
        Go back
      </NavLink>
      {isLoading ? (
        <Loader />
      ) : (
        <div className="legal-notice border-gradient">
          <MEDitor.Markdown source={legalNotice} />
        </div>
      )}
    </div>
  );
};

export default LegalNoticePage;
