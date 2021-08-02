import React, { useContext, useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { NavLink } from 'react-router-dom';
import { GetAllEntries } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';
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
        <div className="entries__container enter-animation">
            <div className="entries-menu">
                <div className="basic-button entry-button"><i class="fa fa-plus" aria-hidden="true"></i></div>
            </div>
            {entries.map((entry, index) => (
                <div className="entry" key={index}>
                    <div className="entry-image">{entry.image.length !== 0 ? <img src={`${entry.image}`} /> : <i class="fas fa-sticky-note"></i>}</div>
                    <NavLink className="entry-name" to={`/entry/${id}/${entry.categoryEntryId}`}>
                        <span className="ellipsis">{entry.categoryEntryName}</span>
                    </NavLink>
                    <div className="entry-date">{new Date(entry.createdOn).toISOString().slice(0, 10)}</div>
                    <div className="entry-size">500 KB</div>
                    <div className="entry-menu">Edit</div>
                    <div className="entry-menu">Remove</div>
                </div>
            ))}
        </div>
    )
}

export default Category;