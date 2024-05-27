import React, { useState, useEffect } from 'react';
import '../styles/InfoChip.css';

interface InfoChipProps {
    title: string;
    value: any;
    postfix: string;
    editable?: boolean;
    onChange?: (value: string) => void;
}

const InfoChip: React.FC<InfoChipProps> = ({ title, value, postfix, editable = false, onChange }) => {
    const [editing, setEditing] = useState(false);
    const [inputValue, setInputValue] = useState(value);

    useEffect(() => {
        setInputValue(value);
    }, [value]);

    const handleEditClick = () => {
        setEditing(true);
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(e.target.value);
        if (onChange) {
            onChange(e.target.value);
        }
    };

    const handleBlur = () => {
        setEditing(false);
    };

    return (
        <div className="info-chip">
            <span className="info-chip-title">{title}</span>
            {editable && editing ? (
                <input
                    type="text"
                    value={inputValue}
                    onChange={handleChange}
                    onBlur={handleBlur}
                    autoFocus
                    className="info-chip-input"
                />
            ) : (
                <span className="info-chip-value" onClick={editable ? handleEditClick : undefined}>
                    {inputValue} {postfix}
                </span>
            )}
        </div>
    );
};

export default InfoChip;
