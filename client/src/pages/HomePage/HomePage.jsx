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
                    {entries.length > 0 ? entries.map((entry, index) => (
                        <NavLink to={`/entry/${entry.category.categoryId}/${entry.categoryEntryId}`} className="entry" key={index}>
                            <div className="entry-image"><i class="fas fa-sticky-note"></i></div>
                            <div className="entry-name"><span className="ellipsis">{entry.categoryEntryName}</span></div>
                            <div className="entry-category"><span className="ellipsis">Category: {entry.category.name}</span></div>
                            <div className="entry-date">Created on: {new Date(entry.createdOn).toISOString().slice(0, 10)}</div>
                            <div className="entry-date">Last modified on: {new Date(entry.createdOn).toISOString().slice(0, 10)}</div>
                            <div className="entry-size">Size: 500 KB</div>
                        </NavLink>
                    ))
                        :
                        <div style={{ display: 'grid', placeItems: 'center', width: '100%', height: '100%' }}>
                            <div>
                                <p style={{ textAlign: 'center' }}>Such empty</p>
                                <img src="https://www.pinclipart.com/picdir/big/550-5506338_pepe-the-frog-transparent-png-transparent-pepe-png.png" style={{ width: '100px' }} />
                            </div>
                        </div>
                    }
                </>
            }
        </div>
    )
}

export default HomePage;