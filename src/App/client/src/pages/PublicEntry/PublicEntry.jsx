import { useEffect } from "react";
import { useParams } from "react-router";
import { EntriesService } from "../../api";

const PublicEntryPage = () => {
  const { link } = useParams();

  useEffect(() => {
    (async () => {
      let result = await EntriesService.getApiEntriesPublic({ url: link });

      console.log(result);
    })();
  }, []);

  return <>"Hello"</>;
};

export default PublicEntryPage;
