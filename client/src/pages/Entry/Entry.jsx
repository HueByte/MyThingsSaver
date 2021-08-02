import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router';
import { GetOneEntry } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';
import Loader from '../../components/Loaders/Loader';
import ReactMarkdown from 'react-markdown';
import MEDitor from '@uiw/react-md-editor';
import './Entry.css';

const test = '*Hello world* \n**Hello World** \n```js\nlet x = \'tete\'';


const Entry = () => {
    const authContext = useContext(AuthContext);
    const { categoryId, entryId } = useParams();
    const [entry, setEntry] = useState();
    const [testingValue, setTestingValue] = useState(test);
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => console.log(entry), [entry]);

    useEffect(async () => {
        await GetOneEntry(authContext.authState?.token, entryId)
            .then(result => setEntry(result.data))
            .catch(error => console.error(error));
    }, []);

    const switchEdit = () => setIsEditing(!isEditing);

    return (
        <div className="entry__container">
            {entry ? <>
                <div className="basic-info">
                    <div className="basic-info-left">
                        <div className="icon"><i class="fas fa-sticky-note"></i></div>
                        <div className="name">{entry.categoryEntryName}</div>
                        <div className="date">{new Date(entry.createdOn).toISOString().slice(0, 10)}</div>
                    </div>
                    <div className="basic-info-right">
                        <div className="basic-button entry-button" onClick={switchEdit}>Edit</div>
                        <div className="basic-button entry-button">Delete</div>
                    </div>
                </div>
                <div className="entry-content">
                    {isEditing ?
                        <MEDitor value={testingValue} onChange={setTestingValue} />
                        :
                        <MEDitor.Markdown source={testingValue} />
                    }
                </div>
            </> : <Loader />}
        </div >
    )
}

export default Entry;