import React, { useContext, useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import { GetRecentEntries } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';
import Loader from '../../components/Loaders/Loader';
import './HomePage.css';

const HomePage = () => {
    const authContext = useContext(AuthContext);
    const [entries, setEntries] = useState([]);
    const [isFetching, setFetching] = useState(true);

    useEffect(async () => {
        await GetRecentEntries(authContext.authState?.token)
            .then(result => {
                console.log(result);
                if (result.isSuccess)
                    setEntries(result.data);
            })
            .catch((error) => console.error(error));

        setFetching(false);
    }, []);

    useEffect(() => console.log(entries), [entries]);

    return (
        <div className="homepage__container">
            {isFetching ? <Loader /> :
                <>
                    {entries.map((entry, index) => (
                        <NavLink to={`/entry/${entry.category.categoryId}/${entry.categoryEntryId}`} className="entry" key={index}>
                            <div className="entry-image"><i class="fas fa-sticky-note"></i></div>
                            <div className="entry-name"><span className="ellipsis">{entry.categoryEntryName}</span></div>
                            <div className="entry-date">{new Date(entry.createdOn).toISOString().slice(0, 10)}</div>
                            <div className="entry-size">500 KB</div>
                        </NavLink>
                    ))
                    }
                </>
            }
        </div>
    )
}

export default HomePage;