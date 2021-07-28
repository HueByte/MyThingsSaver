import React, { Children } from 'react';
import ReactModal from 'react-modal';
import Modal from 'react-modal';

const customStyles = {
    overlay: {
        zIndex: '999',
        backgroundColor: 'rgba(1,1,1,0)',
        // filter: 'blur(10px)'
        backdropFilter: 'blur(2px)'
    },
    content: {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        marginRight: '-50%',
        transform: 'translate(-50%, -50%)',
        backgroundColor: 'rgb(27,27,27)',
    },
};

export const BasicModal = ({ children, ...params }) => {
    return (
        <ReactModal style={customStyles} {...params}>
            {children}
        </ReactModal>
    )
}

