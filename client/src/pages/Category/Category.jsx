import React, { useCallback, useContext, useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { GetAllEntries } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';
import Loader from '../../components/Loaders/Loader';
import './Category.css';

const Category = () => {
    const authContext = useContext(AuthContext);
    const [entries, setEntries] = useState([]);
    const { id } = useParams();

    useEffect(async () => {
        await GetAllEntries(authContext.authState?.token, id)
            .then(result => setEntries(result.data))
            .catch((error) => console.error(error));
    }, [id]);

    useEffect(() => console.log(entries), [entries]);

    return (
        <div className="entries__container">
            {id}
            <div className="entries-menu">
                <div className="basic-button entry-button"><i class="fa fa-plus" aria-hidden="true"></i></div>
            </div>
            {entries.map((entry, index) => (
                <div className="entry" key={index}>
                    <div className="entry-image">{entry.image.length !== 0 ? <img src="${entry.image}" /> : <i class="fas fa-sticky-note"></i>}</div>
                    <div className="entry-name">{entry.categoryEntryName}</div>
                    <div className="entry-date">{entry.content}</div>
                </div>
            ))}
        </div>
    )
}

export default Category;