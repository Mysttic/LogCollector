import React, { useState, useEffect } from 'react';
import axios from 'axios';
import AlertDetails from './AlertDetails';
import './AlertsView.css';
import { useLocation } from 'react-router-dom';

const AlertsView = () => {
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const initialMonitorId = queryParams.get('monitorId') || 0;
    const [alerts, setAlerts] = useState([]);
    const [monitors, setMonitors] = useState([]); 
    const [totalAlerts, setTotalAlerts] = useState(0);
    const [selectedAlertId, setSelectedAlertId] = useState(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [filters, setFilters] = useState({
        message: '',
        content: '',
        monitorId: initialMonitorId 
    });
    const [pendingSelection, setPendingSelection] = useState(false);
     

    useEffect(() => {
        fetchAlerts();
        fetchMonitors(); 
    }, [page, pageSize, filters]);

    useEffect(() => {
        if (pendingSelection && alerts.length > 0) {
            handleSelect(alerts[alerts.length - 1].id);
            setPendingSelection(false);
        }
    }, [alerts]);

    const fetchAlerts = async () => {
        try {
            const response = await axios.get('/api/Alert', {
                params: {
                    message: filters.message,
                    content: filters.content,
                    monitorId: filters.monitorId,
                    pageNumber: page,
                    pageSize: pageSize,
                    startIndex: ((page - 1) * pageSize)
                }
            });
            console.log('Response:', response.data);
            if (response.data && response.data.pagedAlertResult && response.data.pagedAlertResult.items) {
                setAlerts(response.data.pagedAlertResult.items);
                setTotalAlerts(response.data.totalCount || 0);
            } else {
                console.error('Unexpected response structure:', response.data);
                setAlerts([]);
            }
        } catch (error) {
            console.error('Error fetching alerts:', error);
        }
    };

    const fetchMonitors = async () => {
        try {
            const response = await axios.get('/api/Monitor');
            if (response.data && response.data.pagedMonitorResult && response.data.pagedMonitorResult.items) {
                setMonitors(response.data.pagedMonitorResult.items);
            }
        } catch (error) {
            console.error('Error fetching monitors:', error);
            setMonitors([]);
        }
    };

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters({ ...filters, [name]: value });
    };

    const handleSelect = (alertId) => {
        if (selectedAlertId === alertId) {
            setSelectedAlertId(null); 
        } else {
            setSelectedAlertId(alertId); 
        }
    };

    const handlePageChange = (newPage) => {
        setPage(newPage);
    };

    const handleClose = () => {
        setSelectedAlertId(null);
        fetchAlerts();
    };

    return (
        <div>
            <h2>Alerts</h2>
            <div>
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
                        name="message"
                        placeholder="Message"
                        value={filters.message}
                        onChange={handleFilterChange}
                    />
                </div>
                <div className="div-filter-element">
                    <input
                        type="text"
                        name="content"
                        placeholder="Content"
                        value={filters.content}
                        onChange={handleFilterChange}
                    />
                </div>
                <div className="div-filter-element">
                    <select
                        name="monitorId"
                        value={filters.monitorId}
                        onChange={handleFilterChange}>
                        <option value="">Select Monitor</option>
                        {Array.isArray(monitors) ? monitors.map((monitor) => (
                            <option key={monitor.id} value={monitor.id}>
                                {monitor.name}
                            </option>
                        )) : (
                            <option>No monitors available</option>
                        )}
                    </select>
                </div>
            </div>
            <div>
                <table className="table">
                    <thead>
                        <tr>
                            <th>MonitorId</th>
                            <th>Message</th>
                            <th>Content</th>
                        </tr>
                    </thead>
                    <tbody>
                        {alerts.map((alert) => (
                            <React.Fragment key={alert.id}>
                                <tr>
                                    <td>{alert.monitorId}</td>
                                    <td>{alert.message}</td>
                                    <td>
                                        <button onClick={() => handleSelect(alert.id)}>View Details</button>
                                    </td>
                                </tr>
                                {selectedAlertId === alert.id && (
                                    <tr>
                                        <td colSpan="4">
                                            <AlertDetails alert={alert}
                                                onClose={handleClose}
                                                fetchAlerts={fetchAlerts}
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
                    <button className="button-navigator" onClick={() => handlePageChange(page + 1)} disabled={page * pageSize >= totalAlerts}>Next</button>
                </div>
            </div>
        </div>
    );
};

export default AlertsView;
