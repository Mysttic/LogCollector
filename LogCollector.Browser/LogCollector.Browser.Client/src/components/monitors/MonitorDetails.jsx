import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './MonitorDetails.css';

const MonitorDetails = ({ monitor, onClose, fetchMonitors }) => {
    const [editMode, setEditMode] = useState(false);
    const [monitorData, setMonitorData] = useState(monitor);
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setMonitorData({ ...monitorData, [name]: value });
    };

    const handleSave = async () => {
        try {
            const response = await axios.put(`/api/Monitor/${monitor.id}`, monitorData);
            setMonitorData(monitorData);
            setEditMode(false);
            fetchMonitors(); // Refresh list view
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
            fetchMonitors(); // Refresh list view
            onClose(); // Close details view
            navigate('/monitors'); // Redirect to list view or update the state
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
                    <input
                        type="text"
                        name="action"
                        value={monitorData.action}
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
                                    <td><b>{monitorData.name}</b></td>
                                </tr>
                                <tr>
                                    <td>Action</td>
                                    <td><b>{monitorData.action}</b></td>
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
