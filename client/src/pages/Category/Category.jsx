import React, { useContext, useEffect, useRef, useState } from 'react';
import { useParams } from 'react-router';
import { NavLink } from 'react-router-dom';
import { AddOneEntry, DeleteOneEntry, GetAllEntries } from '../../api/Entries';
import { AuthContext } from '../../auth/AuthContext';
import { BasicModal } from '../../components/BasicModal/BasicModal';
import { successModal, warningModal } from '../../core/Modals';
import './Category.css';

const Category = () => {
    const authContext = useContext(AuthContext);
    const [entries, setEntries] = useState([]);
    const [isAddModalOpen, setIsAddModalOpen] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const entryToDelete = useRef();
    const { categoryId, entryId } = useParams();


    useEffect(async () => {
        await GetAllEntries(authContext.authState?.token, entryId)
            .then(result => setEntries(result.data))
            .catch((error) => console.error(error));
        console.log(categoryId + entryId);
    }, [entryId]);

    // Add entry
    const addEntry = async (name) => {
        if (name.length === 0) {
            warningModal('Name cannot be empty');
            setIsAddModalOpen(false);
            return;
        }

        await AddOneEntry(authContext.authState?.token, name, entryId)
            .then(async () => {
                await GetAllEntries(authContext.authState?.token, entryId)
                    .then(result => setEntries(result.data))
                    .catch((error) => console.error(error));
            })
            .catch((error) => console.error(error));

        setIsAddModalOpen(false);
    }

    const closeAddModal = () => setIsAddModalOpen(false);

    // Remove entry
    const invokeDeleteModal = (category) => {
        entryToDelete.current = category;
        setIsDeleteModalOpen(true);
    }
    const closeDeleteModal = () => setIsDeleteModalOpen(false);

    const removeEntry = async (entryId) => {
        await DeleteOneEntry(authContext.authState?.token, entryId)
            .then(async () => {
                let newEntries = entries;
                newEntries = newEntries.filter(entry => {
                    return entry.categoryEntryId !== entryId;
                })

                setEntries(newEntries);
                successModal('Successfully removed entry')
            })
            .catch((error) => console.error(error));

        closeDeleteModal();
    }

    return (
        <div className="entries__container enter-animation">
            <div className="entries-menu">
                <div className="category-name">
                    <p className="ellipsis" style={{ textAlign: 'start' }}>{categoryId}</p>
                </div>
                <div className="basic-button entry-button-add" onClick={() => setIsAddModalOpen(true)}>
                    <i class="fa fa-plus" aria-hidden="true"></i>
                </div>
            </div>
            {entries.length > 0 ? entries.map((entry, index) => (
                <div className="entry" key={index}>
                    <div className="entry-image">{entry.image && entry.image.length !== 0 ? <img src={`${entry.image}`} /> : <i class="fas fa-sticky-note"></i>}</div>
                    <NavLink className="entry-name" to={`/entry/${entryId}/${entry.categoryEntryId}`}>
                        <span className="ellipsis">{entry.categoryEntryName}</span>
                    </NavLink>
                    <div className="entry-date">{new Date(entry.lastUpdatedOn + 'Z').toLocaleDateString()}</div>
                    <div className="entry-size">{entry.size} B</div>
                    <div className="entry-menu-buttons">
                        <NavLink to={`/entry/${entryId}/${entry.categoryEntryId}`} className="entry-menu">Show</NavLink>
                        <div className="entry-menu" onClick={() => invokeDeleteModal(entry)}>Remove</div>
                    </div>
                </div>
            )) : <>empty</>}
            <BasicModal isOpen={isAddModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeAddModal}>
                <AddModal addEntry={addEntry} closeAddModal={closeAddModal} />
            </BasicModal>
            <BasicModal isOpen={isDeleteModalOpen} shouldCloseOnOverlayClick={true} onRequestClose={closeDeleteModal}>
                <DeleteModal entry={entryToDelete.current} onDelete={removeEntry} closeDeleteModal={closeDeleteModal} />
            </BasicModal>
        </div>
    )
}

const AddModal = ({ addEntry, closeAddModal }) => {
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
                <div className="basic-button" onClick={closeAddModal}>Close</div>
            </div>
        </div>
    )
}

const DeleteModal = ({ entry, onDelete, closeDeleteModal }) => {
    return (
        <div>
            <p style={{ fontSize: 'larger', fontWeight: 'bold' }}>
                Are you sure you want to delete <span className="ellipsis" style={{ color: 'var(--Rose)' }}>{entry.categoryEntryName}</span>
            </p>
            <div className="modal-menu-buttons">
                <div className="basic-button accept" onClick={() => onDelete(entry.categoryEntryId)}>Yes</div>
                <div className="basic-button close" onClick={closeDeleteModal}>No</div>
            </div>
        </div>
    )
}

export default Category;