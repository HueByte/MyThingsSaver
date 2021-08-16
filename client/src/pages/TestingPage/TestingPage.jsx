import React, { useEffect } from 'react';

const TestingPage = () => {
    useEffect(() => {
        localStorage.clear();
    })

    return (
        <>
            Testing Page
        </>
    )
}

export default TestingPage;