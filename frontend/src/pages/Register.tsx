import React, { useState } from 'react';
import Header from '../components/Header';
import '../styles/Form.css';
import defaultServerConfig from "../common/server-info.ts";
import {useNavigate} from "react-router-dom";

// TODO: handle 409 CONFLICT
const Register: React.FC = () => {
    const navigate = useNavigate();

    const { apiUrl } = defaultServerConfig;

    const [formData, setFormData] = useState({
        email: '',
        password: '',
        userName: '',
        firstName: '',
        lastName: ''
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
            const response = await fetch(`${apiUrl}/api/auth/sign-up`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });

            if (!response.ok) {
                console.error(response)
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
                <h2>Create account</h2>
                <form className="register-form" onSubmit={handleSubmit}>
                    <label>
                        Username
                        <input
                            type="text"
                            name="userName"
                            placeholder="Your username"
                            value={formData.userName}
                            onChange={handleChange}
                        />
                    </label>
                    <label>
                        First name
                        <input
                            type="text"
                            name="firstName"
                            placeholder="Your first name"
                            value={formData.firstName}
                            onChange={handleChange}
                        />
                    </label>
                    <label>
                        Last name
                        <input
                            type="text"
                            name="lastName"
                            placeholder="Your last name"
                            value={formData.lastName}
                            onChange={handleChange}
                        />
                    </label>
                    <label>
                        Email
                        <input
                            type="email"
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
                    <button type="submit">Register</button>
                </form>
            </main>
        </div>
    );
};

export default Register;
