import React, { useEffect, useState } from 'react';
import '../styles/Header.css';
import Button from './Button.tsx';

const Header: React.FC = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const logout = () => {
        setIsLoggedIn(false);
        localStorage.removeItem('sessionInfo');
    }

    useEffect(() => {
        const { expiresAt } = JSON.parse(localStorage.getItem('sessionInfo') ?? JSON.stringify({ sessionKey: '', expiresAt: 0 }));
        (expiresAt < Date.now()) ? logout() : setIsLoggedIn(true);
    }, []);

    return (
        <header>
            <h1>Slay Queen Recipes</h1>
            <nav>
                <a href="/">Home</a>
                {!isLoggedIn && <a href="/register">Register</a>}
                {!isLoggedIn && <a href="/login">Login</a>}
                {isLoggedIn && <a href="/create">Create Recipe</a>}
                {isLoggedIn && <a href="/my-recipes">My Recipes</a>}
                {isLoggedIn && <Button text="Log out" onClick={() => logout()}/>}
            </nav>
        </header>
    );
};

export default Header;
