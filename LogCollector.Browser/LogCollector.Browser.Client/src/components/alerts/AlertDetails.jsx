import React, { useState } from 'react';
import axios from 'axios';
import './AlertDetails.css'; 

const AlertDetails = ({ alert, onClose, fetchAlerts }) => {
    const [alertData, setAlertData] = useState(alert);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setAlertData(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    const formatDate = (date) => {
        return new Date(date).toLocaleString();
    };

    const handleSave = async () => {
        try {
            await axios.put(`/api/Alert/${alert.id}`, alertData);  // PUT do zaktualizowania alertu
            fetchAlerts(); // Odœwie¿ listê alertów
            onClose(); // Zamknij widok szczegó³ów alertu
        } catch (error) {
            console.error('Error updating alert', error);
        }
    };

    const handleDelete = async () => {
        try {
            await axios.delete(`/api/Alert/${alert.id}`);  // DELETE do usuniêcia alertu
            fetchAlerts(); // Odœwie¿ listê alertów
            onClose(); // Zamknij widok szczegó³ów alertu
        } catch (error) {
            console.error('Error deleting alert', error);
        }
    };

    return (
        <div className="alert-details-view">
            <h4>Alert Details</h4>
            <table className="table-full-width">
                <tbody>
                    <tr>
                        <td>
                            <label>Message:</label>
                        </td>
                        <td>
                            <input
                                type="text"
                                name="message"
                                value={alertData.message}
                                onChange={handleChange}
                                placeholder="Message" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Content:</label>
                        </td>
                        <td>
                            <textarea
                                className="textarea"
                                name="content"
                                value={alertData.content}
                                onChange={handleChange}
                                placeholder="Content" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Created At:</label>
                        </td>
                        <td>
                            <label>{formatDate(alertData.createdAt)}</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Updated At:</label>
                        </td>
                        <td>
                            <label>{formatDate(alertData.updatedAt)}</label>
                        </td>
                    </tr>
                </tbody>
            </table>

            <div className="buttons">
                <button onClick={handleSave}>Save</button>
                <button onClick={onClose}>Cancel</button>
                <button onClick={handleDelete} className="delete-button">Delete</button>
            </div>
        </div>
    );
};

export default AlertDetails;
