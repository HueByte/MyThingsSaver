import React, { useContext, useEffect, useRef, useState } from 'react';
import { useParams } from 'react-router';
import { NavLink } from 'react-router-dom';
import { AddOneEntry, DeleteOneEntry, GetAllEntries } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';
import { BasicModal } from '../../components/BasicModal/BasicModal';
import { warningModal } from '../../core/Modals';
import './Category.css';

const Category = () => {
    const authContext = useContext(AuthContext);
    const [entries, setEntries] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const { id } = useParams();


    useEffect(async () => {
        await GetAllEntries(authContext.authState?.token, id)
            .then(result => setEntries(result.data))
            .catch((error) => console.error(error));
    }, [id]);

    useEffect(() => console.log(entries), [entries]);

    const addEntry = async (name) => {
        if (name.length === 0) {
            warningModal('Name cannot be empty');
            setIsModalOpen(false);
            return;
        }

        await AddOneEntry(authContext.authState?.token, name, id)
            .then(async () => {
                await GetAllEntries(authContext.authState?.token, id)
                    .then(result => setEntries(result.data))
                    .catch((error) => console.error(error));
            })
            .catch((error) => console.error(error));

        setIsModalOpen(false);
    }

    const removeEntry = async (entryId) => {
        await DeleteOneEntry(authContext.authState?.token, entryId)
            .then(async () => {
                let newEntries = entries;
                newEntries = newEntries.filter(entry => {
                    return entry.categoryEntryId !== entryId;
                })

                setEntries(newEntries);
            })
            .catch((error) => console.error(error));
    }

    const closeModal = () => setIsModalOpen(false);

    return (
        <div className="entries__container enter-animation">
            <div className="entries-menu">
                <div className="basic-button entry-button" onClick={() => setIsModalOpen(true)}><i class="fa fa-plus" aria-hidden="true"></i></div>
            </div>
            {entries.length > 0 ? entries.map((entry, index) => (
                <div className="entry" key={index}>
                    <div className="entry-image">{entry.image && entry.image.length !== 0 ? <img src={`${entry.image}`} /> : <i class="fas fa-sticky-note"></i>}</div>
                    <NavLink className="entry-name" to={`/entry/${id}/${entry.categoryEntryId}`}>
                        <span className="ellipsis">{entry.categoryEntryName}</span>
                    </NavLink>
                    <div className="entry-date">{new Date(entry.lastUpdatedOn + 'Z').toLocaleDateString()}</div>
                    <div className="entry-size">{entry.size} B</div>
                    <div className="entry-menu-buttons">
                        <NavLink to={`/entry/${id}/${entry.categoryEntryId}`} className="entry-menu">Show</NavLink>
                        <div className="entry-menu" onClick={() => removeEntry(entry.categoryEntryId)}>Remove</div>
                    </div>
                </div>
            )) : <>empty</>}
            <BasicModal isOpen={isModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeModal}>
                <AddModal addEntry={addEntry} closeModal={closeModal} />
            </BasicModal>
        </div>
    )
}

const AddModal = ({ addEntry, closeModal }) => {
    const entryName = useRef();
    useEffect(() => entryName.current = document.getElementById('add-name-input'), []);

    const handleEnter = (event) => {
        if (event.key === "Enter") addEntry(entryName.current.value)
    }

    return (
        <div className="modal-container">
            <div className="modal-field">
                <div className="field-name">Name</div>
                <input id="add-name-input" type="text" className="basic-input" onKeyDown={handleEnter} />
            </div>
            <div className="modal-menu">
                <div className="basic-button" onClick={() => addEntry(entryName.current.value)}>Add</div>
                <div className="basic-button" onClick={closeModal}>Close</div>
            </div>
        </div>
    )
}

export default Category;