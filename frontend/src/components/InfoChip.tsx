import React, { useState, useEffect, useRef } from 'react';
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
    const inputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        setInputValue(value);
    }, [value]);

    const handleEditClick = () => {
        setEditing(true);
        setTimeout(() => {
            if (inputRef.current) {
                inputRef.current.select();
            }
        }, 0);
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
        <div className="info-chip" onClick={editable ? handleEditClick : undefined}>
            <span className="info-chip-title">{title}</span>
            {editable && editing ? (
                <input
                    type="text"
                    ref={inputRef}
                    value={inputValue}
                    onChange={handleChange}
                    onBlur={handleBlur}
                    autoFocus
                    className="info-chip-input"
                />
            ) : (
                <span className="info-chip-value">
                    {inputValue} {postfix}
                </span>
            )}
        </div>
    );
};

export default InfoChip;
