import React from 'react';
import { useParams } from 'react-router';

const Entry = () => {
    const { id } = useParams();
    return (
        <>
            entry
        </>
    )
}

export default Entry;