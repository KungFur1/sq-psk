import React, { useState } from 'react';
import Header from '../components/Header';
import '../styles/Form.css';
import defaultServerConfig from "../common/server-info.ts";
import {useNavigate} from "react-router-dom";

const Login: React.FC = () => {
    const { apiUrl } = defaultServerConfig;
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        email: null,
        password: null,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await fetch(`${apiUrl}/api/auth/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });

            if (!response.ok) {
                console.error(response)
                console.error((await response.json()))
                throw new Error('Network response was not ok');
            }

            const result = await response.json();
            localStorage.setItem('sessionInfo', JSON.stringify(
                {
                    expiresAt: Date.now() + (60 * 60 * 1000),
                    key: result.sessionKey,
                })
            );
            console.log('Success:', result);
        } catch (error) {
            console.error('Error:', error);
        }
        navigate('/');
    };

    return (
        <div>
            <Header />
            <main className="register-form-container">
                <h2>Log in</h2>
                <form className="register-form" onSubmit={handleSubmit}>
                    <label>
                        Username
                        <input
                            type="text"
                            name="email"
                            placeholder="Your email"
                            value={formData.email}
                            onChange={handleChange}
                        />
                    </label>
                    <label>
                        Password
                        <input
                            type="password"
                            name="password"
                            placeholder="Your password"
                            value={formData.password}
                            onChange={handleChange}
                        />
                    </label>
                    <button type="submit">Login</button>
                </form>
            </main>
        </div>
    );
};

export default Login;
