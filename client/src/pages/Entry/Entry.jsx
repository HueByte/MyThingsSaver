import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router';
import { GetOneEntry } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';

const Entry = () => {
    const authContext = useContext(AuthContext);
    const { categoryId, entryId } = useParams();
    const [entry, setEntry] = useState({});

    useEffect(() => console.log(entry), [entry]);

    useEffect(async () => {
        await GetOneEntry(authContext.authState?.token, entryId)
            .then(result => setEntry(result))
            .catch(error => console.error(error));
    }, []);

    return (
        <div className="entry__container">

        </div>
    )
}

export default Entry;