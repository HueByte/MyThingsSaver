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
    <div className="legal-notice-container flex h-full w-full flex-col items-center justify-center gap-8 overflow-y-auto overflow-x-hidden p-8 md:px-0 md:py-4">
      <NavLink
        to="/"
        className="mts-btn-primary mts-bg-gradient-r w-[200px] hover:text-textColor"
      >
        Go back
      </NavLink>
      {isLoading ? (
        <Loader />
      ) : (
        <div className="legal-notice mts-bg-gradient-r w-[1024px] rounded-2xl bg-altBackgroundColor p-1 lg:w-[100%] lg:rounded-none">
          <MEDitor.Markdown source={legalNotice} />
        </div>
      )}
    </div>
  );
};

export default LegalNoticePage;
