import React, { useState, useEffect } from 'react';
import axios from 'axios';
import MonitorDetails from './MonitorDetails';
import './MonitorsView.css';

const MonitorsView = () => {
    const [monitors, setMonitors] = useState([]);
    const [totalMonitors, setTotalMonitors] = useState(0);
    const [selectedMonitorId, setSelectedMonitorId] = useState(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [filters, setFilters] = useState({
        isactive: '',
        name: '',
        action: '',
        query: ''
    });
    const [showAddForm, setShowAddForm] = useState(false);
    const [newMonitor, setNewMonitor] = useState({ name: '', action: '', alerts: [] });
    const [pendingSelection, setPendingSelection] = useState(false);

    useEffect(() => {
        fetchMonitors();
    }, [page, pageSize, filters]);

    useEffect(() => {
        if (pendingSelection && monitors.length > 0) {
            handleSelect(monitors[monitors.length - 1].id);
            setPendingSelection(false);
        }
    }, [monitors]);

    const fetchMonitors = async () => {
        try {
            const response = await axios.get('/api/Monitor', {
                params: {
                    isActive: filters.isActive,
                    name: filters.name,
                    action: filters.action,
                    query: filters.query,
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

    const handleFilterChange = (e) => {
        setFilters({ ...filters, [e.target.name]: e.target.value });
    };

    const handleSelect = (monitorId) => {
        if (selectedMonitorId === monitorId) {
            setSelectedMonitorId(null); // Hide details if the same monitor is clicked again
        } else {
            setSelectedMonitorId(monitorId); // Show details for the clicked monitor
        }
    };

    const handlePageChange = (newPage) => {
        setPage(newPage);
    };

    const handleAddMonitor = async () => {
        try {
            await axios.post('/api/Monitor', newMonitor);
            setShowAddForm(false);
            setNewMonitor({ name: '', action: '', alerts: [] });
            setPendingSelection(true);
            await fetchMonitors();
        } catch (error) {
            console.error('Error adding monitor:', error);
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setNewMonitor({ ...newMonitor, [name]: value });
    };

    const handleClose = () => {
        setSelectedMonitorId(null);
        fetchMonitors();
    };

    console.log("Monitors data:", monitors);

    return (
        <div>
            <h2>Monitors</h2>    
            <div>
                <div className="div-filter-element">
                    <select
                        name="isActive"
                        value={filters.isActive}
                        onChange={handleFilterChange}>
                        <option value="">All</option>
                        <option value="True">Active</option>
                        <option value="False">Inactive</option>
                    </select>
                </div>
                <div className="div-filter-element">
                    <select
                        name="pageSize"
                        value={pageSize}
                        onChange={(e) => setPageSize(parseInt(e.target.value))}>
                        <option value="10">10</option>
                        <option value="20">20</option>
                        <option value="50">50</option>
                    </select>
                </div>
                <div className="div-filter-element">
                    <input
                        type="text"
                        name="name"
                        placeholder="Name"
                        value={filters.name}
                        onChange={handleFilterChange}
                    />
                </div>
                <div className="div-filter-element">
                    <input
                        type="text"
                        name="query"
                        placeholder="Query"
                        value={filters.query}
                        onChange={handleFilterChange}
                    />
                </div>
                <div className="div-filter-element">
                    <select
                        name="action"
                        value={filters.action}
                        onChange={handleFilterChange}>
                        <option value="">All</option>
                        <option value="NotDefined">NotDefined</option>
                        <option value="Email">Email</option>
                        <option value="SMS">SMS</option>
                    </select>
                </div>
                <br/>
                <button onClick={() => handleAddMonitor()}>Add Monitor</button>
            </div>
            <div>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Action</th>
                            <th>Alerts</th>
                        </tr>
                    </thead>
                    <tbody>
                        {monitors.map((monitor) => (
                            <React.Fragment key={monitor.id}>
                                <tr>
                                    <td>{monitor.name}</td>
                                    <td>{monitor.action}</td>
                                    <td>{monitor.alerts.length}</td>
                                    <td>
                                        <button onClick={() => handleSelect(monitor.id)}>View Details</button>
                                    </td>
                                </tr>
                                {selectedMonitorId === monitor.id && (
                                    <tr>
                                        <td colSpan="4">
                                            <MonitorDetails monitor={monitor}
                                                onClose={handleClose}
                                                fetchMonitors={fetchMonitors}
                                            />

                                        </td>
                                    </tr>
                                )}
                            </React.Fragment>
                        ))}
                    </tbody>
                </table>
                <div>
                    <button className="button-navigator" onClick={() => handlePageChange(page - 1)} disabled={page === 1}>Previous</button>
                    <button className="button-navigator" onClick={() => handlePageChange(page + 1)} disabled={page * pageSize >= totalMonitors}>Next</button>
                </div>
            </div>
        </div>
    );
};

export default MonitorsView;
