import React, { useState, useEffect } from 'react';
import axios from 'axios';
import MonitorDetails from './MonitorDetails';
import './MonitorsView.css';

const MonitorsView = () => {
    const [monitors, setMonitors] = useState([]);
    const [totalMonitors, setTotalMonitors] = useState(0);
    const [selectedMonitor, setSelectedMonitor] = useState(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [showAddForm, setShowAddForm] = useState(false);
    const [newMonitor, setNewMonitor] = useState({ name: '', action: '', alerts: [] });

    useEffect(() => {
        fetchMonitors();
    }, [page, pageSize]);

    const fetchMonitors = async () => {
        try {
            const response = await axios.get('/api/Monitor', {
                params: {
                    pageNumber: page,
                    pageSize: pageSize,
                    startIndex: ((page - 1) * pageSize)
                }
            });
            console.log('Response:', response.data);
            setMonitors(response.data.pagedMonitorResult.items || []);
            setTotalMonitors(response.data.totalCount || 0);
        } catch (error) {
            console.error('Error fetching monitors:', error);
        }
    };

    const handleSelect = (monitor) => {
        setSelectedMonitor(monitor);
        };

    const handlePageChange = (newPage) => {
        setPage(newPage);
    };

    const handleAddMonitor = async () => {
        try {
            const response = await axios.post('/api/Monitor', newMonitor);
            setShowAddForm(false);
            setNewMonitor({ name: '', action: '', alerts: [] });
            fetchMonitors();
        }
        catch (error) {
            console.error('Error adding monitor:', error);
        }
    }

    const handleChange = (e) => {
        const { name, value } = e.target;
        setNewMonitor({ ...newMonitor, [name]: value });
    };

    const handleClose = () => {
        setSelectedMonitor(null);
        fetchMonitors();
    }


    console.log("Monitors data:", monitors);

    return (
        <div>
            <h2>Monitors</h2>
            <button onClick={() => setShowAddForm(true)}>Add Monitor</button> {/* Przycisk do pokazania formularza */}
            {showAddForm && (
                <div className="add-monitor-form">
                    <h3>Add New Monitor</h3>
                    <input
                        type="text"
                        name="name"
                        value={newMonitor.name}
                        onChange={handleChange}
                        placeholder="Name"
                    />
                    <input
                        type="text"
                        name="action"
                        value={newMonitor.action}
                        onChange={handleChange}
                        placeholder="Action"
                    />
                    <button onClick={handleAddMonitor}>Save</button>
                    <button onClick={() => setShowAddForm(false)}>Cancel</button>
                </div>
            )}
            <div className="monitor-container">
                <table className="monitor-table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Action</th>
                            <th>Alerts</th>
                        </tr>
                    </thead>
                    <tbody>
                        {monitors.map((monitor) => (
                            <tr key={monitor.id} onClick={() => handleSelect(monitor)}>
                                <td>{monitor.name}</td>
                                <td>{monitor.action}</td>
                                <td>{monitor.alerts.length}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <div className="monitor-details">
                    {selectedMonitor && (
                        <MonitorDetails
                            monitor={selectedMonitor}
                            onClose={() => handleClose()}
                            fetchMonitors={fetchMonitors}
                        />
                    )}
                </div>
            </div>
        </div>
    );
};

export default MonitorsView;
