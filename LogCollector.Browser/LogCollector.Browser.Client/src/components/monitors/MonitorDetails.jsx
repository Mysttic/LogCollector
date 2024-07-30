import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './MonitorDetails.css';

const MonitorDetails = ({ monitor, onClose }) => {
    const [editMode, setEditMode] = useState(false);
    const [monitorData, setMonitorData] = useState(monitor);
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setMonitorData({ ...monitorData, [name]: value });
    };

    const handleSave = async () => {
        try {
            await axios.put(`/api/Monitor/${monitor.id}`, monitorData);
            setEditMode(false);
        } catch (error) {
            console.error('Error updating monitor', error);
        }
    };

    const handleCancel = () => {
        setMonitorData(monitor);
        setEditMode(false);
    };

    const handleDelete = async () => {
        try {
            await axios.delete(`/api/Monitor/${monitor.id}`);
            onClose(); // Close details view
            navigate('/'); // Redirect to list view or update the state
        } catch (error) {
            console.error('Error deleting monitor', error);
        }
    };

    return (
        <div className="monitor-details-view">
            <button onClick={onClose}>Close</button>
            {editMode ? (
                <div>
                    <input
                        type="text"
                        name="name"
                        value={monitorData.name}
                        onChange={handleChange}
                    />
                    {/* Add more fields as needed */}
                    <button onClick={handleSave}>Save</button>
                    <button onClick={handleCancel}>Cancel</button>
                </div>
            ) : (
                    <div>
                        <table>
                            <tbody>
                                <tr>
                                    <td>Name</td>
                                    <td><b>{monitor.name}</b></td>
                                </tr>
                                <tr>
                                    <td>Action</td>
                                    <td><b>{monitor.action}</b></td>
                                </tr>
                            </tbody>
                        </table>
                        <h4>Alerts</h4>
                        <table>
                        <thead>
                                <tr>
                                    <th>Message</th>
                                    <th>Created At</th>
                                </tr>
                            </thead>
                            <tbody>
                                {monitor.alerts.map((alert) => (
                                    <tr key={alert.id}>
                                        <td>{alert.message}</td>
                                        <td>{alert.createdAt}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    <button onClick={() => setEditMode(true)}>Edit</button>
                    <button onClick={handleDelete}>Delete</button>
                </div>
            )}
        </div>
    );
};

export default MonitorDetails;
