import React, { useEffect, useState, useContext } from 'react';
import { Redirect, useParams } from 'react-router';
import { DeleteOneEntry, GetOneEntry, UpdateOneEntry } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';
import Loader from '../../components/Loaders/Loader';
import MEDitor from '@uiw/react-md-editor';
import './Entry.css';

const Entry = () => {
    const authContext = useContext(AuthContext);
    const { categoryId, entryId } = useParams();
    const [entry, setEntry] = useState();
    const [name, setName] = useState();
    const [editValue, setEditValue] = useState();
    const [isEditing, setIsEditing] = useState(false);
    const [shouldRedirect, setShouldRedirect] = useState(false);


    useEffect(async () => {
        await GetOneEntry(authContext.authState?.token, entryId)
            .then(result => {
                if (!result.isSuccess) {
                    setShouldRedirect(true);
                    return;
                }

                setEntry(result.data);
                setName(result.data.categoryEntryName);
                setEditValue(result.data.content);
            })
            .catch(error => console.error(error));
    }, []);

    const switchEdit = () => setIsEditing(!isEditing);

    const sendUpdate = async () => {
        await UpdateOneEntry(authContext.authState?.token, entryId, name, editValue)
            .catch(error => console.error(error));
        setIsEditing(false);
    }

    const removeEntry = async () => {
        await DeleteOneEntry(authContext.authState?.token, entryId)
            .then(() => setShouldRedirect(true))
            .catch((error) => console.error(error));
    }

    if (shouldRedirect) return <Redirect to={`/category/${categoryId}`} />
    return (
        <div className="entry__container">
            {entry ? <>
                <div className="basic-info">
                    <div className="basic-info-left">
                        <div className="icon"><i class="fas fa-sticky-note"></i></div>
                        <div className="name"><span className="ellipsis">{name}</span></div>
                        <div className="date">{new Date(entry.createdOn).toISOString().slice(0, 10)}</div>
                    </div>
                    <div className="basic-info-right">
                        <div className={`basic-button entry-button${isEditing ? '' : ' hide'}`} onClick={sendUpdate}>Accept</div>
                        <div className="basic-button entry-button" onClick={switchEdit}>{isEditing ? 'close' : 'edit'}</div>
                        <div className="basic-button entry-button" onClick={removeEntry}>Delete</div>
                    </div>
                </div>
                <div className="entry-content">
                    {isEditing ?
                        <MEDitor value={editValue} onChange={setEditValue} commands={[]} height={500} preview={'live'} />
                        :
                        <MEDitor.Markdown source={editValue} />
                    }
                </div>
            </> : <Loader />}
        </div >
    )
}

export default Entry;