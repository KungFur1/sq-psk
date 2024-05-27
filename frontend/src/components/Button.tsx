import React from "react";
import "../styles/Button.css";

interface ButtonProps {
    text: string;
    onClick?: () => void;
    type?: "button" | "submit" | "reset";
    className?: string;
}

const Button: React.FC<ButtonProps> = ({ text, onClick, type = "button", className }) => {
    return (
        <button onClick={onClick} type={type} className={`${className} outter`}>
                {text}
        </button>
    );
};

export default Button;
