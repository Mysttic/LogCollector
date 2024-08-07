import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './MonitorDetails.css';

const MonitorDetails = ({ monitor, onClose, fetchMonitors }) => {
    const [monitorData, setMonitorData] = useState(monitor);
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setMonitorData({ ...monitorData, [name]: value });
    };

    const formatDate = (date) => {
        return new Date(date).toLocaleString();
    };

    const handleSave = async () => {
        try {
            await axios.put(`/api/Monitor/${monitor.id}`, monitorData);
            fetchMonitors(); // Refresh list view
            onClose(); // Close details view
        } catch (error) {
            console.error('Error updating monitor', error);
        }
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
            <div>
                <table className="table-full-width">
                    <tr>
                        <td>
                            <label>Is active:</label>
                        </td>
                        <td>
                            <input 
                                className="checkbox"
                                type="checkbox"
                                name="isActive"
                                value={monitorData.isActive}
                                onChange={handleChange}
                                placeholder="IsActive" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Name:</label>
                        </td>
                        <td>
                            <input
                                type="text"
                                name="name"
                                value={monitorData.name}
                                onChange={handleChange}
                                placeholder="Name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Description:</label>
                        </td>
                        <td>
                            <textarea
                                className="textarea"
                                type="text"
                                name="description"
                                value={monitorData.description}
                                onChange={handleChange}
                                placeholder="Description" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Query:</label>
                        </td>
                        <td>
                            <textarea
                                className="textarea"
                                type="text"
                                name="query"
                                value={monitorData.query}
                                onChange={handleChange}
                                placeholder="Query" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Action:</label>
                        </td>
                        <td>
                            <select
                                className="select"
                                name="action"
                                value={monitorData.action}
                                onChange={handleChange}>
                                    <option value="NotDefined">NotDefined</option>
                                    <option value="Email">Email</option>
                                    <option value="SMS">SMS</option>
                                </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Last invoke:</label>
                        </td>
                        <td>
                            <label>{formatDate(monitorData.lastInvoke)}</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Created at:</label>
                        </td>
                        <td>
                            <label>{formatDate(monitorData.createdAt)}</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Updated at:</label>
                        </td>
                        <td>
                            <label>{formatDate(monitorData.updatedAt)}</label>
                        </td>
                    </tr>
                </table>
                    
                <button onClick={handleSave}>Save</button>
                <button onClick={onClose}>Cancel</button>
            </div>
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
            <button onClick={handleDelete} className="delete-button">Delete</button>
        </div>
    );
};

export default MonitorDetails;
