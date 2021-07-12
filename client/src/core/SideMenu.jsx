import React, { useEffect, useState } from 'react';
import './SideMenu.css';

const SideMenu = () => {
    const [expandPlus, setExpandPlus] = useState(false);
    const [expandMinus, setExpandMinus] = useState(false);

    // useEffect(() => {
    //     console.log(expandPlus);
    //     console.log(expandMinus);   
    // }, [expandPlus, expandMinus]);

    const togglePlus = () => {
        setExpandPlus(!expandPlus);
    }

    const toggleMinus = () => {
        setExpandMinus(!expandMinus);
    }

    return(
        <div className="nav-side">
            <div className="nav-side-title">
                <p>Your Categories</p>
            </div>
            <div className="nav-side-controlls">
                <div onClick={togglePlus} className="basic-button nav-side-button"><i class="fa fa-plus" aria-hidden="true"></i></div>
                <div onClick={toggleMinus} className="basic-button nav-side-button"><i class="fa fa-minus" aria-hidden="true"></i></div>
            </div>
            <div className="nav-side__container">
                <div className="item">Item</div>
                <div className="item">Item</div>
                <div className="item">Item</div>
                <div className="item">Item</div>
                <div className="item">Item</div>
                <div className="item">Item</div>
            </div>
        </div>
    )
}

export default SideMenu;