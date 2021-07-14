import React from 'react';
import { useParams } from 'react-router';

const Category = () => {
    const { name } = useParams();
    return(
        <>
            {name}
        </>
    )
}

export default Category;